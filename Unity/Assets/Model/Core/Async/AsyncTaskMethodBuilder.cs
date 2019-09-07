using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;

namespace Model
{
    public struct AsyncTaskMethodBuilder
    {
        private TaskCompletionSource tcs;
        private Action moveNext;

        // 1. Static Create method.
        [DebuggerHidden]
        public static AsyncTaskMethodBuilder Create()
        {
            AsyncTaskMethodBuilder builder = new AsyncTaskMethodBuilder();
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public Task Task
        {
            get
            {
                if (tcs != null)
                {
                    return tcs.Task;
                }

                if (moveNext == null)
                {
                    return Task.CompletedTask;
                }

                tcs = new TaskCompletionSource();
                return tcs.Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            if (tcs == null)
            {
                tcs = new TaskCompletionSource();
            }

            if (exception is OperationCanceledException ex)
            {
                tcs.TrySetCanceled(ex);
            }
            else
            {
                tcs.TrySetException(exception);
            }
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult()
        {
            if (moveNext == null)
            {
            }
            else
            {
                if (tcs == null)
                {
                    tcs = new TaskCompletionSource();
                }

                tcs.TrySetResult();
            }
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : INotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (tcs == null)
                {
                    tcs = new TaskCompletionSource(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.OnCompleted(moveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : ICriticalNotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (tcs == null)
                {
                    tcs = new TaskCompletionSource(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.UnsafeOnCompleted(moveNext);
        }

        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }

    public struct AsyncTaskMethodBuilder<T>
    {
        private T result;
        private TaskCompletionSource<T> tcs;
        private Action moveNext;

        // 1. Static Create method.
        [DebuggerHidden]
        public static AsyncTaskMethodBuilder<T> Create()
        {
            var builder = new AsyncTaskMethodBuilder<T>();
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public Task<T> Task
        {
            get
            {
                if (tcs != null)
                {
                    return new Task<T>(tcs);
                }

                if (moveNext == null)
                {
                    return new Task<T>(result);
                }

                tcs = new TaskCompletionSource<T>();
                return tcs.Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            if (tcs == null)
            {
                tcs = new TaskCompletionSource<T>();
            }

            if (exception is OperationCanceledException ex)
            {
                tcs.TrySetCanceled(ex);
            }
            else
            {
                tcs.TrySetException(exception);
            }
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult(T ret)
        {
            if (moveNext == null)
            {
                result = ret;
            }
            else
            {
                if (tcs == null)
                {
                    tcs = new TaskCompletionSource<T>();
                }

                tcs.TrySetResult(ret);
            }
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : INotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (tcs == null)
                {
                    tcs = new TaskCompletionSource<T>(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.OnCompleted(moveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : ICriticalNotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (tcs == null)
                {
                    tcs = new TaskCompletionSource<T>(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.UnsafeOnCompleted(moveNext);
        }

        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}