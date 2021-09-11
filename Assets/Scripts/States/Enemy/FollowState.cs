using UnityEngine;

namespace States.Enemy
{
    public class FollowState : IState
    {
        private Character.Enemy enemy;

        public void Enter(Character.Enemy parent)
        {
            enemy = parent;
        }

        public void Update()
        {
            if (enemy.Target != null)
            {
                var position = enemy.transform.position;
                enemy.MoveDirection = (enemy.Target.transform.position - position).normalized;

                position = Vector2.MoveTowards(position, enemy.Target.position, enemy.MoveSpeed * Time.deltaTime);
                enemy.transform.position = position;

                float distance = Vector2.Distance(enemy.Target.position, enemy.transform.position);

                if (distance <= enemy.AttackRange)
                {
                    enemy.ChangeState(new AttackState());
                }
            }

            if (!enemy.InRange)
            {
                if (enemy.ResetOutOfRange)
                    enemy.ChangeState(new EvadeState());
                else
                    enemy.ChangeState(new IdleState());
            }
        }

        public void Exit()
        {
            enemy.MoveDirection = Vector2.zero;
        }
    }
}