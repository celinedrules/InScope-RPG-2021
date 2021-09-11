namespace States.Enemy
{
    public class IdleState : IState
    {
        private Character.Enemy enemy;
    
        public void Enter(Character.Enemy parent)
        {
            enemy = parent;

            if (enemy.ResetOutOfRange)
                enemy.Reset();
        }

        public void Update()
        {
            if (enemy.Target != null)
            {
                enemy.ChangeState(new FollowState());
            }
        }

        public void Exit()
        {
        
        }
    }
}