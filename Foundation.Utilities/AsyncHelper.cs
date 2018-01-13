namespace Foundation.Utilities
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class AsyncHelper
    {
        /// <summary>
        /// Tries the provided operation, wrapping the execution in standard try/catch trappings.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="handleException">handler delegate for exceptional failure</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        public static async Task TryAsync(Func<CancellationToken, Task> operation, Action<Exception> handleException, CancellationToken cancellationToken)
        {
            try
            {
                await operation(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                handleException(exception);
            }
        }

        /// <summary>
        /// Tries the provided operation, wrapping the execution in standard try/catch trappings.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="handleException">handler delegate for exceptional failure</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        public static async Task<T> TryAsync<T>(Func<CancellationToken, Task<T>> operation, Func<Exception, T> handleException, CancellationToken cancellationToken)
        {
            try
            {
                return await operation(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                return handleException(exception);
            }
        }
    }
}
