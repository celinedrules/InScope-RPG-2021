using System;
using System.Collections;
using Control;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Spell;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class Player : Character
    {
        [SerializeField] private Camera mainCamera;
        [PropertyOrder(-1)] [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private FieldOfView fieldOfView;
        [SerializeField] private Stat mana;

        [SerializeField, LabelText("Initial Mana")]
        private float initMana;

        [SerializeField] private Transform[] exitPoints;
        [SerializeField] private bool isLockedOn;
        [SerializeField] private Sprite emptyHat;

        private static Player instance;
        private InputAction movement;
        private InputAction interact;
        private IInteractable interactable;

        //private Vector2 halfPlayerSize;

        public static Player Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Player>();

                return instance;
            }
        }

        private bool IsLockedOn => isLockedOn;

        public Sprite EmptyHat => emptyHat;

        private void Awake()
        {
            //Debug.Log(typeof(string).Assembly.ImageRuntimeVersion);
            var gameplayActionMap = inputActions.FindActionMap("Default");

            movement = gameplayActionMap.FindAction("Movement");
            interact = gameplayActionMap.FindAction("Interact");
        }

        protected override void Start()
        {
            //halfPlayerSize = GetComponent<SpriteRenderer>().bounds.size / 2;

            mana.Init(initMana, initMana);

            Facing = FacingDirection.Down;

            base.Start();
        }

        protected override void Update()
        {
            //  ClampPlayerMovement();

            base.Update();

            if (movement.triggered)
            {
                Facing = movement.ReadValue<Vector2>() switch
                {
                    { } v when v == Vector2.up => FacingDirection.Up,
                    { } v when v == Vector2.right => FacingDirection.Right,
                    { } v when v == Vector2.down => FacingDirection.Down,
                    { } v when v == Vector2.left => FacingDirection.Left,
                    _ => Facing
                };
            }

            fieldOfView.FacingDirection = Facing;

            MoveDirection = movement.ReadValue<Vector2>();

            if (MoveDirection != Vector2.zero && IsAttacking)
                StopAttack();

            if (interact.triggered)
                Interact();

            // Debug
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                Health.CurrentValue -= 10;
                mana.CurrentValue -= 10;
            }
            else if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                Health.CurrentValue += 10;
                mana.CurrentValue += 10;
            }
            
        }

        private void OnEnable()
        {
            movement.Enable();
            interact.Enable();
        }

        private void OnDisable()
        {
            movement.Disable();
            interact.Disable();
        }

        //private void ClampPlayerMovement()
        //{
        // Vector3 position = transform.position;
        // float distance = position.z - mainCamera.transform.position.z;
        // float leftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, distance)).x + halfPlayerSize.x;
        // float rightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, distance)).x - halfPlayerSize.x;
        // float topBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + halfPlayerSize.y;
        // float bottomBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - halfPlayerSize.y;
        // position.x = Mathf.Clamp(position.x, leftBorder, rightBorder);
        // position.y = Mathf.Clamp(position.y, topBorder, bottomBorder);
        // transform.position = position;
        //}

        [UsedImplicitly]
        public void CastSpell(string spellName)
        {
            if (IsLockedOn)
            {
                if (Target == null || !Target.GetComponentInParent<Character>().IsAlive)
                    return;

                if (fieldOfView.InsideFOV(Target.position))
                    SpellRoutine = StartCoroutine(CastSpellRoutine(spellName));
            }
            else
            {
                SpellRoutine = StartCoroutine(CastSpellRoutine(spellName));
            }
        }

        private IEnumerator CastSpellRoutine(string spellName)
        {
            Transform currentTarget = Target;

            Spell.Spell spell = SpellBook.Instance.CastSpell(spellName);

            //SetAnimatorParameter("Attack", true);

            IsAttacking = true;

            yield return new WaitForSeconds(spell.CastTime);

            SetAnimatorParameter("CastSpell");

            yield return new WaitForSeconds(.3f);
            SpellScript spellScript =
                Instantiate(spell.SpellPrefab, exitPoints[(int)Facing].position, Quaternion.identity)
                    .GetComponent<SpellScript>();

            if (IsLockedOn)
            {
                if (currentTarget != null && fieldOfView.InsideFOV(currentTarget.position))
                    spellScript.Initialize(currentTarget, spell.Damage, transform);
            }
            else
            {
                spellScript.Initialize(null, spell.Damage, transform);
            }

            spellScript.Shoot(GetFacingVector(), IsLockedOn);

            if (Target == null)
                Destroy(spellScript.gameObject, 3.0f);

            StopAttack();
        }

        private void StopAttack()
        {
            if (SpellRoutine == null)
                return;

            SpellBook.Instance.StopCasting();

            StopCoroutine(SpellRoutine);

            IsAttacking = false;
            //SetAnimatorParameter("Attack", false);
        }

        private Vector2 GetFacingVector()
        {
            return Facing switch
            {
                FacingDirection.Up => Vector2.up,
                FacingDirection.Right => Vector2.right,
                FacingDirection.Down => Vector2.down,
                FacingDirection.Left => Vector2.left,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Interact() => interactable?.Interact();

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Interactable"))
                interactable = other.GetComponent<IInteractable>();
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy") && !other.CompareTag("Interactable"))
                return;
            
            if (interactable == null)
                return;
                
            interactable.StopInteract();
            interactable = null;
        }
    }
}