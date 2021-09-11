using UnityEngine;

namespace States.Enemy
{
    public class EvadeState : IState
    {
        private Character.Enemy enemy;
    
        public void Enter(Character.Enemy parent)
        {
            enemy = parent;
        }

        public void Update()
        {
            enemy.MoveDirection = (enemy.StartPosition - enemy.transform.position).normalized;
            enemy.transform.position =
                Vector2.MoveTowards(enemy.transform.position, enemy.StartPosition, enemy.MoveSpeed * Time.deltaTime);

            float distance = Vector2.Distance(enemy.StartPosition, enemy.transform.position);
        
            if(distance <= 0)
                enemy.ChangeState(new IdleState());
        }

        public void Exit()
        {
            enemy.MoveDirection = Vector2.zero;
        
            if(enemy.ResetOutOfRange)
                enemy.Reset();
        }
    }
}