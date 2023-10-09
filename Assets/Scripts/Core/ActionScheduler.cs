using SimpleRPG.Abstraction;

namespace SimpleRPG.Core
{
    public class ActionScheduler
    {
        private IAction _currentAction;
        private bool _canStartNewAction = true;

        public void StartNewAction(IAction action)
        {
            if(_canStartNewAction == false)
                return;
            
            if(_currentAction == action)
                return;
            
            _currentAction?.Cancel();
            _currentAction = action;
        }

        public void CancelCurrentAction() => _currentAction?.Cancel();

        public void SetPossibilityToStartNewAction(bool possibility) => _canStartNewAction = possibility;
    }
}