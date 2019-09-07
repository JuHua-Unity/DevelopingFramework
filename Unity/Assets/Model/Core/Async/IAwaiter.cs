using System.Runtime.CompilerServices;

namespace Model
{
    public enum AwaiterStatus
    {
        /// <summary>The operation has not yet completed.</summary>
        Pending = 0,

        /// <summary>The operation completed successfully.</summary>
        Succeeded = 1,

        /// <summary>The operation completed with an error.</summary>
        Faulted = 2,

        /// <summary>The operation completed due to cancellation.</summary>
        Canceled = 3
    }

    public interface IAwaiter : ICriticalNotifyCompletion
    {
        AwaiterStatus Status { get; }
        bool IsCompleted { get; }
        void GetResult();
    }

    public interface IAwaiter<out T> : IAwaiter
    {
        new T GetResult();
    }
}