/*
 * https://github.com/loekd/ServiceFabric.PubSubActors/blob/master/LICENSE.md
 */
namespace Foundation.ServiceFabric
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Data;

    /// <summary>
    /// Provides retry support when using the <see cref="ReliableStateManager"/>.
    /// </summary>
    public static class TimeoutRetryHelper
    {
        private const int DefaultMaxAttempts = 10;
        private static readonly TimeSpan InitialDelay = TimeSpan.FromMilliseconds(200);
        private static readonly TimeSpan MinimumDelay = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Executes the provided callback in a StateManager transaction, with retry + exponential back-off. Returns the result.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="stateManager">State Manager to use.</param>
        /// <param name="operation">Operation to execute with retry.</param>
        /// <param name="cancellationToken">Cancellation support. (optional)</param>
        /// <param name="maxAttempts">#Attempts to execute <paramref name="operation"/> (optional)</param> 
        /// <param name="initialDelay">First delay between attempts. Later on this will be exponentially grow. (optional)</param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteInTransaction<TResult>(
            IReliableStateManager stateManager,
            Func<ITransaction, CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken = default(CancellationToken),
            int maxAttempts = DefaultMaxAttempts,
            TimeSpan? initialDelay = null)
        {
            Args.NotNull(stateManager, nameof(stateManager));
            Args.NotNull(operation, nameof(operation));
            if (maxAttempts <= 0) maxAttempts = DefaultMaxAttempts;
            if (initialDelay == null || initialDelay.Value < MinimumDelay)
            {
                initialDelay = InitialDelay;
            }

            return await Execute<object, TResult>(async (st, token) =>
            {
                TResult result;
                using (var transaction = stateManager.CreateTransaction())
                {
                    try
                    {
                        result = await operation(transaction, cancellationToken);
                    }
                    catch (TimeoutException)
                    {
                        transaction.Abort();
                        throw;
                    }
                }
                return result;
            }, null, cancellationToken, maxAttempts, initialDelay);
        }

        /// <summary>
        /// Executes the provided callback in a StateManager transaction, with retry + exponential back-off. Returns the result.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="stateManager">State Manager to use.</param>
        /// <param name="operation">Operation to execute with retry.</param>
        /// <param name="state">State passed to callback. (optional)</param>
        /// <param name="cancellationToken">Cancellation support. (optional)</param>
        /// <param name="maxAttempts">#Attempts to execute <paramref name="operation"/> (optional)</param> 
        /// <param name="initialDelay">First delay between attempts. Later on this will be exponentially grow. (optional)</param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteInTransaction<TState, TResult>(
            IReliableStateManager stateManager,
            Func<ITransaction, TState, CancellationToken, Task<TResult>> operation,
            TState state,
            CancellationToken cancellationToken = default(CancellationToken),
            int maxAttempts = DefaultMaxAttempts,
            TimeSpan? initialDelay = null)
        {
            Args.NotNull(stateManager, nameof(stateManager));
            Args.NotNull(operation, nameof(operation));
            if (maxAttempts <= 0) maxAttempts = DefaultMaxAttempts;
            if (initialDelay == null || initialDelay.Value < MinimumDelay)
            {
                initialDelay = InitialDelay;
            }

            return await Execute(async (st, token) =>
            {
                TResult result;
                using (var transaction = stateManager.CreateTransaction())
                {
                    result = await operation(transaction, state, token);
                }
                return result;
            }, state, cancellationToken, maxAttempts, initialDelay);
        }

        /// <summary>
        /// Executes the provided callback in a StateManager transaction, with retry + exponential back-off.
        /// </summary>
        /// <param name="stateManager">State Manager to use.</param>
        /// <param name="operation">Operation to execute with retry.</param>
        /// <param name="cancellationToken">Cancellation support. (optional)</param>
        /// <param name="maxAttempts">#Attempts to execute <paramref name="operation"/> (optional)</param> 
        /// <param name="initialDelay">First delay between attempts. Later on this will be exponentially grow. (optional)</param>
        /// <returns></returns>
        public static async Task ExecuteInTransaction(
            IReliableStateManager stateManager,
            Func<ITransaction, CancellationToken, Task> operation,
            CancellationToken cancellationToken = default(CancellationToken),
            int maxAttempts = DefaultMaxAttempts,
            TimeSpan? initialDelay = null)
        {
            Args.NotNull(stateManager, nameof(stateManager));
            Args.NotNull(operation, nameof(operation));
            if (maxAttempts <= 0) maxAttempts = DefaultMaxAttempts;
            if (initialDelay == null || initialDelay.Value < MinimumDelay)
            {
                initialDelay = InitialDelay;
            }

            await Execute<object>(async (st, token) =>
            {
                using (var transaction = stateManager.CreateTransaction())
                {
                    await operation(transaction, token);
                }
            }, null, cancellationToken, maxAttempts, initialDelay);
        }

        /// <summary>
        /// Executes the provided callback in a StateManager transaction, with retry + exponential back-off.
        /// </summary>
        /// <param name="stateManager">State Manager to use.</param>
        /// <param name="operation">Operation to execute with retry.</param>
        /// <param name="state">State passed to callback. (optional)</param>
        /// <param name="cancellationToken">Cancellation support. (optional)</param>
        /// <param name="maxAttempts">#Attempts to execute <paramref name="operation"/> (optional)</param> 
        /// <param name="initialDelay">First delay between attempts. Later on this will be exponentially grow. (optional)</param>
        /// <returns></returns>
        public static async Task ExecuteInTransaction<TState>(
            IReliableStateManager stateManager,
            Func<ITransaction, TState, CancellationToken, Task> operation,
            TState state,
            CancellationToken cancellationToken = default(CancellationToken),
            int maxAttempts = DefaultMaxAttempts,
            TimeSpan? initialDelay = null)
        {
            Args.NotNull(stateManager, nameof(stateManager));
            Args.NotNull(operation, nameof(operation));
            if (maxAttempts <= 0) maxAttempts = DefaultMaxAttempts;
            if (initialDelay == null || initialDelay.Value < MinimumDelay)
            {
                initialDelay = InitialDelay;
            }

            await Execute(async (st, token) =>
            {
                using (var transaction = stateManager.CreateTransaction())
                {
                    await operation(transaction, state, token);
                }
            }, state, cancellationToken, maxAttempts, initialDelay);
        }

        /// <summary>
        /// Executes the provided callback with retry + exponential back-off for <see cref="TimeoutException"/>.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="operation">Operation to execute with retry.</param>
        /// <param name="state">State passed to callback. (optional)</param>
        /// <param name="cancellationToken">Cancellation support. (optional)</param>
        /// <param name="maxAttempts">#Attempts to execute <paramref name="operation"/> (optional)</param> 
        /// <param name="initialDelay">First delay between attempts. Later on this will be exponentially grow. (optional)</param>
        /// <returns></returns>
        public static async Task<TResult> Execute<TState, TResult>(
            Func<TState, CancellationToken, Task<TResult>> operation,
            TState state,
            CancellationToken cancellationToken = default(CancellationToken),
            int maxAttempts = DefaultMaxAttempts,
            TimeSpan? initialDelay = null)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (maxAttempts <= 0) maxAttempts = DefaultMaxAttempts;
            if (initialDelay == null || initialDelay.Value < MinimumDelay)
                initialDelay = InitialDelay;

            var result = default(TResult);
            for (var attempts = 0; attempts < maxAttempts; attempts++)
            {
                try
                {
                    result = await operation(state, cancellationToken);
                    break;
                }
                catch (TimeoutException)
                {
                    if (attempts == DefaultMaxAttempts)
                    {
                        throw;
                    }
                }

                await Delay(attempts, initialDelay.Value, cancellationToken);
            }
            return result;
        }

        /// <summary>
        /// Executes the provided callback with retry + exponential back-off for <see cref="TimeoutException"/>.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="operation">Operation to execute with retry.</param>
        /// <param name="state">State passed to callback. (optional)</param>
        /// <param name="cancellationToken">Cancellation support. (optional)</param>
        /// <param name="maxAttempts">#Attempts to execute <paramref name="operation"/> (optional)</param> 
        /// <param name="initialDelay">First delay between attempts. Later on this will be exponentially grow. (optional)</param>
        /// <returns></returns>
        public static async Task Execute<TState>(
            Func<TState, CancellationToken, Task> operation,
            TState state,
            CancellationToken cancellationToken = default(CancellationToken),
            int maxAttempts = DefaultMaxAttempts,
            TimeSpan? initialDelay = null)
        {
            Args.NotNull(operation, nameof(operation));
            if (maxAttempts <= 0) maxAttempts = DefaultMaxAttempts;
            if (initialDelay == null || initialDelay.Value < MinimumDelay)
            {
                initialDelay = InitialDelay;
            }

            for (var attempts = 0; attempts < maxAttempts; attempts++)
            {
                try
                {
                    await operation(state, cancellationToken);
                    break;
                }
                catch (TimeoutException)
                {
                    if (attempts == DefaultMaxAttempts)
                    {
                        throw;
                    }
                }

                await Delay(attempts, initialDelay.Value, cancellationToken);
            }
        }

        /// <summary>
        /// Executes the provided callback with retry + exponential back-off for <see cref="TimeoutException"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="operation">Operation to execute with retry.</param>
        /// <param name="cancellationToken">Cancellation support. (optional)</param>
        /// <param name="maxAttempts">#Attempts to execute <paramref name="operation"/> (optional)</param> 
        /// <param name="initialDelay">First delay between attempts. Later on this will be exponentially grow. (optional)</param>
        /// <returns></returns>
        public static async Task<TResult> Execute<TResult>(
            Func<CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken = default(CancellationToken),
            int maxAttempts = DefaultMaxAttempts,
            TimeSpan? initialDelay = null)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (maxAttempts <= 0) maxAttempts = DefaultMaxAttempts;
            if (initialDelay == null || initialDelay.Value < MinimumDelay)
                initialDelay = InitialDelay;

            var result = default(TResult);
            for (var attempts = 0; attempts < maxAttempts; attempts++)
            {
                try
                {
                    result = await operation(cancellationToken);
                    break;
                }
                catch (TimeoutException)
                {
                    if (attempts == DefaultMaxAttempts)
                    {
                        throw;
                    }
                }

                await Delay(attempts, initialDelay.Value, cancellationToken);
            }
            return result;
        }

        internal static async Task Delay(int attempts, TimeSpan initialDelay, CancellationToken cancellationToken)
        {
            //exponential back-off
            var factor = (int)Math.Pow(2, attempts) + 1;
            var delay = new Random(Guid.NewGuid().GetHashCode()).Next((int)(initialDelay.TotalMilliseconds * 0.5D), (int)(initialDelay.TotalMilliseconds * 1.5D));
            await Task.Delay(factor * delay, cancellationToken);
        }
    }
}