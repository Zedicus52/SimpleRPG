using SimpleRPG.Abstraction;

namespace SimpleRPG.Core
{
    public class ActionScheduler
    {
        private IAction _currentAction;

        public void StartNewAction(IAction action)
        {
            _currentAction?.Cancel();
            _currentAction = action;
        }
    }
}