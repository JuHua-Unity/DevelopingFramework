using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Async
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

        [DebuggerHidden] public AwaiterStatus Status => this.awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden] public bool IsCompleted => this.awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public void GetResult()
        {
            if (this.awaiter != null)
            {
                this.awaiter.GetResult();
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
            if (this.awaiter == null && other.awaiter == null)
            {
                return true;
            }

            if (this.awaiter != null && other.awaiter != null)
            {
                return this.awaiter == other.awaiter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.awaiter == null)
            {
                return 0;
            }

            return this.awaiter.GetHashCode();
        }

        public override string ToString()
        {
            return this.awaiter == null ? "()"
                : this.awaiter.Status == AwaiterStatus.Succeeded ? "()"
                : "(" + this.awaiter.Status + ")";
        }

        public struct Awaiter : IAwaiter
        {
            private readonly Task task;

            [DebuggerHidden]
            public Awaiter(Task task)
            {
                this.task = task;
            }

            [DebuggerHidden] public bool IsCompleted => this.task.IsCompleted;

            [DebuggerHidden] public AwaiterStatus Status => this.task.Status;

            [DebuggerHidden]
            public void GetResult()
            {
                this.task.GetResult();
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (this.task.awaiter != null)
                {
                    this.task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (this.task.awaiter != null)
                {
                    this.task.awaiter.UnsafeOnCompleted(continuation);
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
            this.awaiter = null;
        }

        [DebuggerHidden]
        public Task(IAwaiter<T> awaiter)
        {
            this.result = default;
            this.awaiter = awaiter;
        }

        [DebuggerHidden] public AwaiterStatus Status => this.awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden] public bool IsCompleted => this.awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public T Result
        {
            get
            {
                if (this.awaiter == null)
                {
                    return this.result;
                }

                return this.awaiter.GetResult();
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
            if (this.awaiter == null && other.awaiter == null)
            {
                return EqualityComparer<T>.Default.Equals(this.result, other.result);
            }

            if (this.awaiter != null && other.awaiter != null)
            {
                return this.awaiter == other.awaiter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.awaiter == null)
            {
                if (this.result == null)
                {
                    return 0;
                }

                return this.result.GetHashCode();
            }

            return this.awaiter.GetHashCode();
        }

        public override string ToString()
        {
            return this.awaiter == null ? this.result.ToString()
                : this.awaiter.Status == AwaiterStatus.Succeeded ? this.awaiter.GetResult().ToString()
                : "(" + this.awaiter.Status + ")";
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

            [DebuggerHidden] public bool IsCompleted => this.task.IsCompleted;

            [DebuggerHidden] public AwaiterStatus Status => this.task.Status;

            [DebuggerHidden]
            void IAwaiter.GetResult()
            {
                GetResult();
            }

            [DebuggerHidden]
            public T GetResult()
            {
                return this.task.Result;
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (this.task.awaiter != null)
                {
                    this.task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (this.task.awaiter != null)
                {
                    this.task.awaiter.UnsafeOnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }
}