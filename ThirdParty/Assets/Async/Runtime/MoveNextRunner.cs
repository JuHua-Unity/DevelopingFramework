using System.Runtime.CompilerServices;

namespace Async
{
    internal class MoveNextRunner<TStateMachine> where TStateMachine : IAsyncStateMachine
    {
        public TStateMachine StateMachine;

        //[DebuggerHidden]
        public void Run()
        {
            this.StateMachine.MoveNext();
        }
    }
}