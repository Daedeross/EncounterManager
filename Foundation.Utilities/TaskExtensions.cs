namespace Foundation.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        public static Task Then(this Task first, Func<Task> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            var tcs = new TaskCompletionSource<object>();
            Compose(first, tcs, (t1, s1) =>
            {
                Compose(next(), s1, (t2, s2) =>
                {
                    s2.TrySetResult(null);
                });
            });
            return tcs.Task;
        }

        public static Task<TOut> Then<TOut>(this Task first, Func<TOut> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            var tcs = new TaskCompletionSource<TOut>();
            Compose(first, tcs, (t1, s1) =>
            {
                s1.TrySetResult(next());
            });
            return tcs.Task;
        }

        public static Task<TOut> Then<TOut>(this Task first, Func<Task<TOut>> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            var tcs = new TaskCompletionSource<TOut>();
            Compose(first, tcs, (t1, s1) =>
            {
                Compose(next(), s1, (t2, s2) =>
                {
                    s2.TrySetResult(t2.Result);
                });
            });
            return tcs.Task;
        }

        public static Task Then<TIn>(this Task<TIn> first, Func<TIn, Task> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            var tcs = new TaskCompletionSource<object>();
            Compose(first, tcs, (t1, s1) =>
            {
                Compose(next(t1.Result), s1, (t2, s2) =>
                {
                    s2.TrySetResult(null);
                });
            });
            return tcs.Task;
        }

        public static Task<TOut> Then<TIn, TOut>(this Task<TIn> first, Func<TIn, Task<TOut>> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            var tcs = new TaskCompletionSource<TOut>();
            Compose(first, tcs, (t1, s1) =>
            {
                Compose(next(t1.Result), s1, (t2, s2) =>
                {
                    s2.TrySetResult(t2.Result);
                });
            });
            return tcs.Task;
        }

        public static Task<TOut> Then<TIn, TOut>(this Task<TIn> first, Func<TIn, TOut> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            var tcs = new TaskCompletionSource<TOut>();
            Compose(first, tcs, (t1, source) =>
            {
                var result = next(t1.Result);
                tcs.TrySetResult(result);
            });
            return tcs.Task;
        }

        public static Task Iterate(IEnumerable<Task> asyncIterator)
        {
            if (asyncIterator == null) throw new ArgumentNullException(nameof(asyncIterator));

            var enumerator = asyncIterator.GetEnumerator();
            if (enumerator == null) throw new InvalidOperationException("Invalid enumerable - GetEnumerator returned null");

            var tcs = new TaskCompletionSource<object>();
            tcs.Task.ContinueWith(_ => enumerator.Dispose(), TaskContinuationOptions.ExecuteSynchronously);

            Action<Task, TaskCompletionSource<object>> recursiveBody = null;
            recursiveBody = (_, s) => {
                if (enumerator.MoveNext())
                {
                    Compose(enumerator.Current, s, recursiveBody);
                }
                else
                {
                    s.TrySetResult(null);
                }
            };

            recursiveBody(null, tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Composes a single task that waits for execution of all mapped values in parallel.
        /// Applies a map function <paramref name="next"/> to every value in enumerable <paramref name="source"/>
        /// then waits for all to finish executing in parallel.
        /// </summary>
        /// <typeparam name="TIn">The type of the <see cref="IEnumerable{T}"/>, and also the input type to map function <paramref name="next"/></typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/> of type <typeparamref name="TIn"/></param>
        /// <param name="next">Map function to be applied to each item in <paramref name="source"/>.
        /// Returns an unvalued <see cref="Task"/> to be waited on in parallel</param>
        /// <returns>An unvalued <see cref="Task"/> that waits until all parallel tasks are done</returns>
        public static Task Parallel<TIn>(this IEnumerable<TIn> source, Func<TIn, Task> next)
        {
            Args.NotNull(source, nameof(source));
            Args.NotNull(next, nameof(next));
            return Task.WhenAll(source.Select(next));
        }

        /// <summary>
        /// Composes a single task that waits for <paramref name="first"/> to finish then maps all resulting values in parallel.
        /// Applies a map function <paramref name="next"/> to every value in result <see cref="IEnumerable{T}"/> from <paramref name="first"/>
        /// then waits for all to finish executing in parallel.
        /// </summary>
        /// <typeparam name="TIn">The type of the <see cref="IEnumerable{T}"/> returned by task <paramref name="first"/>, and input for map function <paramref name="next"/></typeparam>
        /// <param name="first">Initial <see cref="Task{TResult}"/> whos value is an enumerable of values to be mapped in parallel</param>
        /// <param name="next">Map function to be applied to each item in result <see cref="IEnumerable{T}"/> from <paramref name="first"/>.
        /// Returns a <see cref="Task"/> to be waited on in parallel</param>
        /// <returns>An unvalued <see cref="Task"/> that waits until all parallel tasks are done</returns>
        public static Task Parallel<TIn>(this Task<IEnumerable<TIn>> first, Func<TIn, Task> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            return ComposeParallelTask(first, next);
        }

        /// <summary>
        /// Composes a single task that waits for execution of all mapped values in parallel.
        /// Applies a map function <paramref name="next"/> to every value in enumerable <paramref name="source"/>
        /// then waits for all to finish executing in parallel.
        /// </summary>
        /// <typeparam name="TIn">The type of the <see cref="IEnumerable{T}"/>, and also the input type to map function <paramref name="next"/></typeparam>
        /// <typeparam name="TOut">The type of the resulting array of values output from map function <paramref name="next"/></typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/> of type <typeparamref name="TIn"/></param>
        /// <param name="next">Map function to be applied to each item in <paramref name="source"/>.
        /// Returns a valued <see cref="Task{T}"/> to be waited on in parallel</param>
        /// <returns>A Task that waits until all parallel tasks finish, then returns a array of all results</returns>
        public static Task<TOut[]> Parallel<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, Task<TOut>> next)
        {
            Args.NotNull(source, nameof(source));
            Args.NotNull(next, nameof(next));
            return Task.WhenAll(source.Select(next));
        }

        /// <summary>
        /// Composes a single task that waits for <paramref name="first"/> to finish then maps all resulting values in parallel.
        /// Applies a map function <paramref name="next"/> to every value in result <see cref="IEnumerable{T}"/> from <paramref name="first"/>
        /// then waits for all to finish executing in parallel.
        /// </summary>
        /// <typeparam name="TIn">The type of the <see cref="IEnumerable{T}"/> returned by task <paramref name="first"/>, and input for map function <paramref name="next"/></typeparam>
        /// <typeparam name="TOut">The type of the resulting array of values output from map function <paramref name="next"/></typeparam>
        /// <param name="first">Initial <see cref="Task{TResult}"/> whos value is an enumerable of values to be mapped to parallel tasks</param>
        /// <param name="next">Map function to be applied to each item in result <see cref="IEnumerable{T}"/> from <paramref name="first"/>.
        /// Returns a valued <see cref="Task{T}"/> to be waited on in parallel</param>
        /// <returns>A Task that waits until all parallel tasks finish, then returns a array of all results</returns>
        public static Task<TOut[]> Parallel<TIn, TOut>(this Task<IEnumerable<TIn>> first, Func<TIn, Task<TOut>> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            return ComposeParallelTask(first, next);
        }

        public static Task<TOut[]> Parallel<TIn, TOut>(this Task<List<TIn>> first, Func<TIn, Task<TOut>> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            return ComposeParallelTask(first, next);
        }

        public static Task<TOut[]> Parallel<TIn, TOut>(this Task<TIn[]> first, Func<TIn, Task<TOut>> next)
        {
            Args.NotNull(first, nameof(first));
            Args.NotNull(next, nameof(next));
            return ComposeParallelTask(first, next);
        }

        #region Helpers

        private static void Compose<TOut>(Task first, TaskCompletionSource<TOut> tcs, Action<Task, TaskCompletionSource<TOut>> next)
        {
            first.ContinueWith(t =>
            {
                if (t.IsFaulted) ApplyExceptionsToSource(t, tcs);
                else if (t.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        next(t, tcs);
                    }
                    catch (Exception exc) { tcs.TrySetException(exc); }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private static void Compose<TIn, TOut>(Task<TIn> first, TaskCompletionSource<TOut> tcs, Action<Task<TIn>, TaskCompletionSource<TOut>> next)
        {
            first.ContinueWith(t =>
            {
                if (t.IsFaulted) ApplyExceptionsToSource(t, tcs);
                else if (t.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        next(t, tcs);
                    }
                    catch (Exception exc) { tcs.TrySetException(exc); }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private static Task ComposeParallelTask<TEnumeration, TIn>(Task<TEnumeration> first, Func<TIn, Task> next)
            where TEnumeration : IEnumerable<TIn>
        {
            return first.Then(all => Task.WhenAll(all.Select(next)));
        }

        private static Task<TOut[]> ComposeParallelTask<TEnumeration, TIn, TOut>(Task<TEnumeration> first, Func<TIn, Task<TOut>> next)
            where TEnumeration : IEnumerable<TIn>
        {
            return first.Then(all => Task.WhenAll(all.Select(next)));
        }

        private static void ApplyExceptionsToSource<T>(Task task, TaskCompletionSource<T> source)
        {
            if (task?.Exception?.InnerExceptions != null)
            {
                source.TrySetException(task.Exception.InnerExceptions);
            }
        }

        #endregion Helpers
    }
}
