using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Async
{
    [AsyncMethodBuilder(typeof(AsyncVoidMethodBuilder))]
    public struct Void
    {
        public void Coroutine()
        {
        }

        [DebuggerHidden]
        public Awaiter GetAwaiter()
        {
            return new Awaiter();
        }

        public struct Awaiter : ICriticalNotifyCompletion
        {
            [DebuggerHidden] public bool IsCompleted => true;

            [DebuggerHidden]
            public void GetResult()
            {
                throw new InvalidOperationException("请使用Continue()");
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
            }
        }
    }
}