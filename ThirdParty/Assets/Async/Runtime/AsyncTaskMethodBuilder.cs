using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;

namespace Async
{
    public struct AsyncTaskMethodBuilder
    {
        private TaskCompletionSource tcs;
        private Action moveNext;

        // 1. Static Create method.
        [DebuggerHidden]
        public static AsyncTaskMethodBuilder Create()
        {
            var builder = new AsyncTaskMethodBuilder();
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public Task Task
        {
            get
            {
                if (this.tcs != null)
                {
                    return this.tcs.Task;
                }

                if (this.moveNext == null)
                {
                    return Task.CompletedTask;
                }

                this.tcs = new TaskCompletionSource();
                return this.tcs.Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            if (this.tcs == null)
            {
                this.tcs = new TaskCompletionSource();
            }

            if (exception is OperationCanceledException ex)
            {
                this.tcs.TrySetCanceled(ex);
            }
            else
            {
                this.tcs.TrySetException(exception);
            }
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult()
        {
            if (this.moveNext == null)
            {
            }
            else
            {
                if (this.tcs == null)
                {
                    this.tcs = new TaskCompletionSource();
                }

                this.tcs.TrySetResult();
            }
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (this.moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new TaskCompletionSource(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                this.moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.OnCompleted(this.moveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (this.moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new TaskCompletionSource(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                this.moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.UnsafeOnCompleted(this.moveNext);
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
                if (this.tcs != null)
                {
                    return new Task<T>(this.tcs);
                }

                if (this.moveNext == null)
                {
                    return new Task<T>(this.result);
                }

                this.tcs = new TaskCompletionSource<T>();
                return this.tcs.Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            if (this.tcs == null)
            {
                this.tcs = new TaskCompletionSource<T>();
            }

            if (exception is OperationCanceledException ex)
            {
                this.tcs.TrySetCanceled(ex);
            }
            else
            {
                this.tcs.TrySetException(exception);
            }
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult(T ret)
        {
            if (this.moveNext == null)
            {
                this.result = ret;
            }
            else
            {
                if (this.tcs == null)
                {
                    this.tcs = new TaskCompletionSource<T>();
                }

                this.tcs.TrySetResult(ret);
            }
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (this.moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new TaskCompletionSource<T>(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                this.moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.OnCompleted(this.moveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (this.moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new TaskCompletionSource<T>(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                this.moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.UnsafeOnCompleted(this.moveNext);
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