using SimpleRPG.Abstraction;

namespace SimpleRPG.Core
{
    public class ActionScheduler
    {
        private IAction _currentAction;

        public void StartNewAction(IAction action)
        {
            if(_currentAction == action)
                return;
            
            _currentAction?.Cancel();
            _currentAction = action;
        }
    }
}