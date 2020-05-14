using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Async
{
    public class TaskCompletionSource : IAwaiter
    {
        // State(= AwaiterStatus)
        private const int Pending = 0;
        private const int Succeeded = 1;
        private const int Faulted = 2;
        private const int Canceled = 3;

        private int state;
        private ExceptionDispatchInfo exception;
        private Action continuation; // action or list

        AwaiterStatus IAwaiter.Status => (AwaiterStatus) this.state;

        bool IAwaiter.IsCompleted => this.state != Pending;

        public Task Task => new Task(this);

        void IAwaiter.GetResult()
        {
            switch (this.state)
            {
                case Succeeded:
                    return;
                case Faulted:
                    this.exception?.Throw();
                    this.exception = null;
                    return;
                case Canceled:
                {
                    this.exception?.Throw(); // guranteed operation canceled exception.
                    this.exception = null;
                    throw new OperationCanceledException();
                }
                default:
                    throw new NotSupportedException("Task还没结束 请使用 await ！");
            }
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action action)
        {
            this.continuation = action;
            if (this.state != Pending)
            {
                TryInvokeContinuation();
            }
        }

        private void TryInvokeContinuation()
        {
            this.continuation?.Invoke();
            this.continuation = null;
        }

        public void SetResult()
        {
            if (TrySetResult())
            {
                return;
            }

            throw new InvalidOperationException("Task已结束！");
        }

        public void SetException(Exception e)
        {
            if (TrySetException(e))
            {
                return;
            }

            throw new InvalidOperationException("Task已结束！");
        }

        public bool TrySetResult()
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Succeeded;

            TryInvokeContinuation();
            return true;
        }

        public bool TrySetException(Exception e)
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Faulted;

            this.exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;
        }

        public bool TrySetCanceled()
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Canceled;

            TryInvokeContinuation();
            return true;
        }

        public bool TrySetCanceled(OperationCanceledException e)
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Canceled;

            this.exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;
        }

        void INotifyCompletion.OnCompleted(Action action)
        {
            ((ICriticalNotifyCompletion) this).UnsafeOnCompleted(action);
        }
    }

    public class TaskCompletionSource<T> : IAwaiter<T>
    {
        // State(= AwaiterStatus)
        private const int Pending = 0;
        private const int Succeeded = 1;
        private const int Faulted = 2;
        private const int Canceled = 3;

        private int state;
        private T value;
        private ExceptionDispatchInfo exception;
        private Action continuation; // action or list

        bool IAwaiter.IsCompleted => this.state != Pending;

        public Task<T> Task => new Task<T>(this);

        AwaiterStatus IAwaiter.Status => (AwaiterStatus) this.state;

        T IAwaiter<T>.GetResult()
        {
            switch (this.state)
            {
                case Succeeded:
                    return this.value;
                case Faulted:
                    this.exception?.Throw();
                    this.exception = null;
                    return default;
                case Canceled:
                {
                    this.exception?.Throw(); // guranteed operation canceled exception.
                    this.exception = null;
                    throw new OperationCanceledException();
                }
                default:
                    throw new NotSupportedException("Task还没结束 请使用 await ！");
            }
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action action)
        {
            this.continuation = action;
            if (this.state != Pending)
            {
                TryInvokeContinuation();
            }
        }

        private void TryInvokeContinuation()
        {
            this.continuation?.Invoke();
            this.continuation = null;
        }

        public void SetResult(T result)
        {
            if (TrySetResult(result))
            {
                return;
            }

            throw new InvalidOperationException("Task已结束！");
        }

        public void SetException(Exception e)
        {
            if (TrySetException(e))
            {
                return;
            }

            throw new InvalidOperationException("Task已结束！");
        }

        public bool TrySetResult(T result)
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Succeeded;

            this.value = result;
            TryInvokeContinuation();
            return true;
        }

        public bool TrySetException(Exception e)
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Faulted;

            this.exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;
        }

        public bool TrySetCanceled()
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Canceled;

            TryInvokeContinuation();
            return true;
        }

        public bool TrySetCanceled(OperationCanceledException e)
        {
            if (this.state != Pending)
            {
                return false;
            }

            this.state = Canceled;

            this.exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;
        }

        void IAwaiter.GetResult()
        {
            ((IAwaiter<T>) this).GetResult();
        }

        void INotifyCompletion.OnCompleted(Action action)
        {
            ((ICriticalNotifyCompletion) this).UnsafeOnCompleted(action);
        }
    }
}