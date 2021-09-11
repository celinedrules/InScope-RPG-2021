using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        public enum FacingDirection
        {
            Down = 0,
            Left = 1,
            Right = 2,
            Up = 3
        }

        [SerializeField] private float moveSpeed;
        [SerializeField] protected Transform hitBox;
        [SerializeField] protected Stat health;

        [SerializeField, LabelText("Initial Health")]
        protected float initHealth;

        private bool isAlive;
        private float speed;

        public GameObject upObject;
        public GameObject leftObject;
        public GameObject rightObject;
        public GameObject downObject;

        private Animator currentAnimator;
        private Animator downAnimator;
        private Animator upAnimator;
        private Animator rightAnimator;
        private Animator leftAnimator;
        private Rigidbody2D rBody;

        protected Coroutine SpellRoutine;

        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private bool IsMoving => MoveDirection != Vector2.zero;

        public Stat Health => health;

        public float MoveSpeed => moveSpeed;

        protected FacingDirection Facing { get; set; }

        public Vector2 MoveDirection { get; set; }

        public Transform Target { get; set; }
        public bool IsAttacking { get; set; }

        public bool IsAlive => Health.CurrentValue > 0;

        private void Move()
        {
            if (isAlive)
                rBody.velocity = MoveDirection.normalized * MoveSpeed;
        }

        protected virtual void Start()
        {
            Health.Init(initHealth, initHealth);
            rBody = GetComponent<Rigidbody2D>();

            // This is temporary
            if (GetType() != typeof(Enemy))
            {
                upObject.SetActive(false);
                leftObject.SetActive(false);
                rightObject.SetActive(false);
                downObject.SetActive(true);
                downAnimator = downObject.GetComponent<Animator>();
                upAnimator = upObject.GetComponent<Animator>();
                rightAnimator = rightObject.GetComponent<Animator>();
                leftAnimator = leftObject.GetComponent<Animator>();
                currentAnimator = downAnimator;
            }
            else
            {
                currentAnimator = GetComponent<Animator>();
            }

            isAlive = true;
        }

        protected virtual void Update()
        {
            speed = rBody.velocity.magnitude;

            HandleAnimations();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void HandleAnimations()
        {
            // This is temporary
            if (GetType() != typeof(Enemy))
            {
                switch (Facing)
                {
                    case FacingDirection.Up:
                        upObject.SetActive(true);
                        rightObject.SetActive(false);
                        leftObject.SetActive(false);
                        downObject.SetActive(false);

                        currentAnimator = upAnimator;
                        break;
                    case FacingDirection.Right:
                        upObject.SetActive(false);
                        rightObject.SetActive(true);
                        leftObject.SetActive(false);
                        downObject.SetActive(false);

                        currentAnimator = rightAnimator;
                        break;
                    case FacingDirection.Down:
                        upObject.SetActive(false);
                        rightObject.SetActive(false);
                        leftObject.SetActive(false);
                        downObject.SetActive(true);

                        currentAnimator = downAnimator;
                        break;
                    case FacingDirection.Left:
                        upObject.SetActive(false);
                        rightObject.SetActive(false);
                        leftObject.SetActive(true);
                        downObject.SetActive(false);

                        currentAnimator = leftAnimator;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (IsAlive)
            {
                SetAnimatorParameter(Speed, speed);
            }
            else
            {
                SetAnimatorParameter(Die);
            }
        }

        public virtual void TakeDamage(float damage, Transform source)
        {
            Health.CurrentValue -= damage;

            if (Health.CurrentValue <= 0)
            {
                MoveDirection = Vector2.zero;
                rBody.velocity = MoveDirection;
                isAlive = false;
            }
        }

        public AnimatorStateInfo GetAnimatorStateInfo(int layerIndex) =>
            currentAnimator.GetCurrentAnimatorStateInfo(layerIndex);

        protected void SetAnimatorParameter(string param, bool value) => currentAnimator.SetBool(param, value);

        private void SetAnimatorParameter(int param, bool value) => currentAnimator.SetBool(param, value);

        //protected void SetAnimatorParameter(string param, float value) => anim.SetFloat(param, value);
        private void SetAnimatorParameter(int param, float value) => currentAnimator.SetFloat(param, value);

        //protected void SetAnimatorParameter(string param, int value) => anim.SetInteger(param, value);
        //protected void SetAnimatorParameter(int param, int value) => anim.SetInteger(param, value);
        public void SetAnimatorParameter(string param) => currentAnimator.SetTrigger(param);
        public void SetAnimatorParameter(int param) => currentAnimator.SetTrigger(param);

        protected float GetAnimationParameter(string param) => currentAnimator.GetFloat(param);
        //protected void SetAnimatorParameter(int param) => anim.SetTrigger(param);
    }
}