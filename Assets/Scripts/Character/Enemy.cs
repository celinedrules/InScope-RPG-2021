using Inventory;
using States;
using States.Enemy;
using UI;
using UnityEngine;

namespace Character
{
    public class Enemy : Npc
    {
        [SerializeField] private CanvasGroup healthGroup;
        [SerializeField] private bool canFollow;
        [SerializeField] private bool resetOutOfRange;
        [SerializeField] private float initAggroRange;
        [SerializeField] private LootTable lootTable;

        private SpriteRenderer spriteRenderer;
        private Sprite currentSprite;
        private FieldOfView fov;
        private IState currentState;

        public float AttackRange { get; private set; }
        public float AttackTime { get; set; }
        public float AggroRange { get; set; }
        public Vector3 StartPosition { get; set; }

        public bool InRange => Vector2.Distance(transform.position, Target.position) < AggroRange;

        public bool ResetOutOfRange
        {
            get => resetOutOfRange;
            set => resetOutOfRange = value;
        }

        private void Awake()
        {
            StartPosition = transform.position;
            AggroRange = initAggroRange;
            AttackRange = 1.0f;
            ChangeState(new IdleState());
        }

        protected override void Start()
        {
            base.Start();

            spriteRenderer = GetComponent<SpriteRenderer>();
            currentSprite = GetComponent<SpriteRenderer>().sprite;
            fov = GetComponent<FieldOfView>();
            fov.FacingDirection = FacingDirection.Down;
        }

        protected override void Update()
        {
            if (IsAlive)
            {
                if (!IsAttacking)
                    AttackTime += Time.deltaTime;

                currentState.Update();

                if (!canFollow)
                    return;

                if (Target == null)
                    SetTarget(fov.GetPlayerInsideFOV()?.transform);
                //Target = fov.GetPlayerInsideFOV()?.transform;

                fov.FacingDirection = Facing;

                // Fix this!  We should be using animation events!
                currentSprite = spriteRenderer.sprite;

                if (currentSprite.name.ToLower().Contains("left"))
                    Facing = FacingDirection.Left;
                else if (currentSprite.name.ToLower().Contains("right"))
                    Facing = FacingDirection.Right;
                else if (currentSprite.name.ToLower().Contains("up"))
                    Facing = FacingDirection.Up;
                else
                    Facing = FacingDirection.Down;
            }

            base.Update();
        }

        public override Transform Select()
        {
            healthGroup.alpha = 1;

            return base.Select();
        }

        public override void Deselect()
        {
            healthGroup.alpha = 0;

            base.Deselect();
        }

        public override void TakeDamage(float damage, Transform source)
        {
            if (!(currentState is EvadeState))
            {
                SetTarget(source);
                base.TakeDamage(damage, source);

                OnHealthChanged(health.CurrentValue);
            }
        }

        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter(this);
        }

        public void SetTarget(Transform newTarget)
        {
            if (newTarget != null && Target == null && !(currentState is EvadeState))
            {
                float distance = Vector2.Distance(transform.position, newTarget.position);
                AggroRange = initAggroRange;
                AggroRange += distance;
                Target = newTarget;
            }
        }

        public void Reset()
        {
            Target = null;
            AggroRange = initAggroRange;
            health.CurrentValue = health.MaxValue;
            OnHealthChanged(health.CurrentValue);
        }

        public override void Interact()
        {
            if (!IsAlive)
            {
                //if (GameObject.Find("Player").GetComponent<FieldOfView>().InsideFOV(transform.position))
                //{
                lootTable.ShowLoot();
                //}
            }
        }

        public override void StopInteract()
        {
            LootWindow.Instance.Close();
        }
    }
}