using System;
using System.Threading;

namespace Model
{
    public partial struct Task
    {
        public static Task CompletedTask => new Task();

        public static Task FromException(Exception ex)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            tcs.TrySetException(ex);
            return tcs.Task;
        }

        public static Task<T> FromException<T>(Exception ex)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetException(ex);
            return tcs.Task;
        }

        public static Task<T> FromResult<T>(T value)
        {
            return new Task<T>(value);
        }

        public static Task FromCanceled()
        {
            return CanceledTaskCache.Task;
        }

        public static Task<T> FromCanceled<T>()
        {
            return CanceledTaskCache<T>.Task;
        }

        public static Task FromCanceled(CancellationToken token)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            tcs.TrySetException(new OperationCanceledException(token));
            return tcs.Task;
        }

        public static Task<T> FromCanceled<T>(CancellationToken token)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetException(new OperationCanceledException(token));
            return tcs.Task;
        }

        private static class CanceledTaskCache
        {
            public static readonly Task Task;

            static CanceledTaskCache()
            {
                TaskCompletionSource tcs = new TaskCompletionSource();
                tcs.TrySetCanceled();
                Task = tcs.Task;
            }
        }

        private static class CanceledTaskCache<T>
        {
            public static readonly Task<T> Task;

            static CanceledTaskCache()
            {
                var taskCompletionSource = new TaskCompletionSource<T>();
                taskCompletionSource.TrySetCanceled();
                Task = taskCompletionSource.Task;
            }
        }
    }
}