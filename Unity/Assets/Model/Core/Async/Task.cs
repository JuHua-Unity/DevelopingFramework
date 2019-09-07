using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Model
{
    /// <summary>
    /// Lightweight unity specified task-like object.
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncTaskMethodBuilder))]
    public partial struct Task : IEquatable<Task>
    {
        private readonly IAwaiter awaiter;

        [DebuggerHidden]
        public Task(IAwaiter awaiter)
        {
            this.awaiter = awaiter;
        }

        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public void GetResult()
        {
            if (awaiter != null)
            {
                awaiter.GetResult();
            }
        }

        public void Coroutine()
        {
        }

        [DebuggerHidden]
        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        public bool Equals(Task other)
        {
            if (awaiter == null && other.awaiter == null)
            {
                return true;
            }

            if (awaiter != null && other.awaiter != null)
            {
                return awaiter == other.awaiter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (awaiter == null)
            {
                return 0;
            }

            return awaiter.GetHashCode();
        }

        public override string ToString()
        {
            return awaiter == null ? "()"
                    : awaiter.Status == AwaiterStatus.Succeeded ? "()"
                    : "(" + awaiter.Status + ")";
        }

        public struct Awaiter : IAwaiter
        {
            private readonly Task task;

            [DebuggerHidden]
            public Awaiter(Task task)
            {
                this.task = task;
            }

            [DebuggerHidden]
            public bool IsCompleted => task.IsCompleted;

            [DebuggerHidden]
            public AwaiterStatus Status => task.Status;

            [DebuggerHidden]
            public void GetResult()
            {
                task.GetResult();
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.UnsafeOnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }

    /// <summary>
    /// Lightweight unity specified task-like object.
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncTaskMethodBuilder<>))]
    public struct Task<T> : IEquatable<Task<T>>
    {
        private readonly T result;
        private readonly IAwaiter<T> awaiter;

        [DebuggerHidden]
        public Task(T result)
        {
            this.result = result;
            awaiter = null;
        }

        [DebuggerHidden]
        public Task(IAwaiter<T> awaiter)
        {
            result = default;
            this.awaiter = awaiter;
        }

        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public T Result
        {
            get
            {
                if (awaiter == null)
                {
                    return result;
                }

                return awaiter.GetResult();
            }
        }

        public void Coroutine()
        {
        }

        [DebuggerHidden]
        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        public bool Equals(Task<T> other)
        {
            if (awaiter == null && other.awaiter == null)
            {
                return EqualityComparer<T>.Default.Equals(result, other.result);
            }

            if (awaiter != null && other.awaiter != null)
            {
                return awaiter == other.awaiter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (awaiter == null)
            {
                if (result == null)
                {
                    return 0;
                }

                return result.GetHashCode();
            }

            return awaiter.GetHashCode();
        }

        public override string ToString()
        {
            return awaiter == null ? result.ToString()
                    : awaiter.Status == AwaiterStatus.Succeeded ? awaiter.GetResult().ToString()
                    : "(" + awaiter.Status + ")";
        }

        public static implicit operator Task(Task<T> task)
        {
            if (task.awaiter != null)
            {
                return new Task(task.awaiter);
            }

            return new Task();
        }

        public struct Awaiter : IAwaiter<T>
        {
            private readonly Task<T> task;

            [DebuggerHidden]
            public Awaiter(Task<T> task)
            {
                this.task = task;
            }

            [DebuggerHidden]
            public bool IsCompleted => task.IsCompleted;

            [DebuggerHidden]
            public AwaiterStatus Status => task.Status;

            [DebuggerHidden]
            void IAwaiter.GetResult()
            {
                GetResult();
            }

            [DebuggerHidden]
            public T GetResult()
            {
                return task.Result;
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.UnsafeOnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }
}