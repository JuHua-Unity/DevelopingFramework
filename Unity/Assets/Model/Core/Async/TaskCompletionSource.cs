using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Model
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

        AwaiterStatus IAwaiter.Status => (AwaiterStatus)state;

        bool IAwaiter.IsCompleted => state != Pending;

        public Task Task => new Task(this);

        void IAwaiter.GetResult()
        {
            switch (state)
            {
                case Succeeded:
                    return;
                case Faulted:
                    exception?.Throw();
                    exception = null;
                    return;
                case Canceled:
                    {
                        exception?.Throw(); // guranteed operation canceled exception.
                        exception = null;
                        throw new OperationCanceledException();
                    }
                default:
                    throw new NotSupportedException("Task还没结束 请使用 await ！");
            }
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action action)
        {
            continuation = action;
            if (state != Pending)
            {
                TryInvokeContinuation();
            }
        }

        private void TryInvokeContinuation()
        {
            continuation?.Invoke();
            continuation = null;
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
            if (state != Pending)
            {
                return false;
            }

            state = Succeeded;

            TryInvokeContinuation();
            return true;

        }

        public bool TrySetException(Exception e)
        {
            if (state != Pending)
            {
                return false;
            }

            state = Faulted;

            exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;

        }

        public bool TrySetCanceled()
        {
            if (state != Pending)
            {
                return false;
            }

            state = Canceled;

            TryInvokeContinuation();
            return true;

        }

        public bool TrySetCanceled(OperationCanceledException e)
        {
            if (state != Pending)
            {
                return false;
            }

            state = Canceled;

            exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;

        }

        void INotifyCompletion.OnCompleted(Action action)
        {
            ((ICriticalNotifyCompletion)this).UnsafeOnCompleted(action);
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

        bool IAwaiter.IsCompleted => state != Pending;

        public Task<T> Task => new Task<T>(this);

        AwaiterStatus IAwaiter.Status => (AwaiterStatus)state;

        T IAwaiter<T>.GetResult()
        {
            switch (state)
            {
                case Succeeded:
                    return value;
                case Faulted:
                    exception?.Throw();
                    exception = null;
                    return default;
                case Canceled:
                    {
                        exception?.Throw(); // guranteed operation canceled exception.
                        exception = null;
                        throw new OperationCanceledException();
                    }
                default:
                    throw new NotSupportedException("Task还没结束 请使用 await ！");
            }
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action action)
        {
            continuation = action;
            if (state != Pending)
            {
                TryInvokeContinuation();
            }
        }

        private void TryInvokeContinuation()
        {
            continuation?.Invoke();
            continuation = null;
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
            if (state != Pending)
            {
                return false;
            }

            state = Succeeded;

            value = result;
            TryInvokeContinuation();
            return true;

        }

        public bool TrySetException(Exception e)
        {
            if (state != Pending)
            {
                return false;
            }

            state = Faulted;

            exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;

        }

        public bool TrySetCanceled()
        {
            if (state != Pending)
            {
                return false;
            }

            state = Canceled;

            TryInvokeContinuation();
            return true;

        }

        public bool TrySetCanceled(OperationCanceledException e)
        {
            if (state != Pending)
            {
                return false;
            }

            state = Canceled;

            exception = ExceptionDispatchInfo.Capture(e);
            TryInvokeContinuation();
            return true;

        }

        void IAwaiter.GetResult()
        {
            ((IAwaiter<T>)this).GetResult();
        }

        void INotifyCompletion.OnCompleted(Action action)
        {
            ((ICriticalNotifyCompletion)this).UnsafeOnCompleted(action);
        }
    }
}