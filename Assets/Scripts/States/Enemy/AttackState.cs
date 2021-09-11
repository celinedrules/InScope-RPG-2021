using System.Collections;
using UnityEngine;

namespace States.Enemy
{
    public class AttackState : IState
    {
        private Character.Enemy enemy;
        private const float AttackCooldown = 3.0f;
        private const float ExtraRange = 0.1f;

        public void Enter(Character.Enemy parent)
        {
            this.enemy = parent;
        }

        public void Update()
        {
            if (enemy.AttackTime >= AttackCooldown && !enemy.IsAttacking)
            {
                enemy.AttackTime = 0;
                enemy.StartCoroutine(Attack());
            }
        
            if (enemy.Target != null)
            {
                float distance = Vector2.Distance(enemy.Target.position, enemy.transform.position);

                if (distance > enemy.AttackRange + ExtraRange && !enemy.IsAttacking)
                {
                    enemy.ChangeState(new FollowState());
                }
            }
            else
            {
                enemy.ChangeState(new IdleState());
            }
        }

        public void Exit()
        {
        
        }

        private IEnumerator Attack()
        {
            enemy.IsAttacking = true;
        
            enemy.SetAnimatorParameter("Attack");

            yield return new WaitForSeconds(enemy.GetAnimatorStateInfo(0).length);

            enemy.IsAttacking = false;
        }
    }
}