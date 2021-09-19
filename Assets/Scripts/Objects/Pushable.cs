using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Objects
{
    public enum BlockDirections { Up, Down, Left, Right, UpDown, LeftRight, All }

    public enum TargetAction { Show, Hide, None }

    [RequireComponent(typeof(BoxCollider2D))]
    public class Pushable : ShowHideObject
    {
        [SerializeField] private bool isMovable;
        [SerializeField] private BlockDirections moveDirection;
        [SerializeField] private float waitTime = 0.75f;
        [SerializeField] private float moveSpeed = 0.75f;

        [ShowIf("@action != TargetAction.None")]
        [SerializeField] private bool requireExactPosition;

        [ShowIf("@action != TargetAction.None && requireExactPosition")]
        [SerializeField] private Vector2 targetPosition;

        private BoxCollider2D boxCollider;
        private Rigidbody2D rBody;
        private bool wasMoved;
        private bool isBeingInteractedWith;
        private bool startMove;
        private float currentTime;
        private Vector2 startLocation;
        private Vector2 toLocation;

        protected override void Clear()
        {
            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider2D>();

            if (boxCollider == null)
            {
                Debug.LogAssertion("ERROR - Missing component BoxCollider2D on " + gameObject + " at " +
                                   transform.position);

                HighlightObject();
            }

            if (rBody == null)
                rBody = gameObject.AddComponent<Rigidbody2D>();

            rBody.constraints = RigidbodyConstraints2D.FreezeAll;
            rBody.bodyType = RigidbodyType2D.Kinematic;

            startLocation = transform.position;
            currentTime = waitTime;
        }

        protected override void Update()
        {
            if (!startMove)
                return;

            transform.position = Vector2.MoveTowards(transform.position, toLocation, moveSpeed * Time.deltaTime);

            if (!transform.position.Equals(toLocation))
                return;

            startMove = false;

            switch (action)
            {
                case TargetAction.Show:
                    if (requireExactPosition)
                    {
                        if (transform.position.Equals(targetPosition))
                            targetGameObject.SetActive(true);
                    }
                    else
                    {
                        targetGameObject.SetActive(true);
                    }

                    break;
                case TargetAction.Hide:
                    if (requireExactPosition)
                    {
                        if (transform.position.Equals(targetPosition))
                            targetGameObject.SetActive(false);
                    }
                    else
                    {
                        targetGameObject.SetActive(false);
                    }

                    break;
                case TargetAction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Clear();
        }

        private void MoveRight(Collision2D other)
        {
            if (!(other.relativeVelocity.x > 0))
                return;

            currentTime -= Time.deltaTime;

            if (!(currentTime <= 0))
                return;

            isBeingInteractedWith = false;
            rBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            toLocation = new Vector2(startLocation.x + 1, startLocation.y);
            startMove = true;
        }

        private void MoveLeft(Collision2D other)
        {
            if (!(other.relativeVelocity.x < 0))
                return;

            currentTime -= Time.deltaTime;

            if (!(currentTime <= 0))
                return;

            isBeingInteractedWith = false;
            rBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            toLocation = new Vector2(startLocation.x - 1, startLocation.y);
            startMove = true;
        }

        private void MoveDown(Collision2D other)
        {
            if (!(other.relativeVelocity.y < 0))
                return;

            currentTime -= Time.deltaTime;

            if (!(currentTime <= 0))
                return;

            isBeingInteractedWith = false;
            rBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            toLocation = new Vector2(startLocation.x, startLocation.y - 1);
            startMove = true;
        }

        private void MoveUp(Collision2D other)
        {
            if (!(other.relativeVelocity.y > 0))
                return;

            currentTime -= Time.deltaTime;

            if (!(currentTime <= 0))
                return;

            isBeingInteractedWith = false;
            rBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            toLocation = new Vector2(startLocation.x, startLocation.y + 1);
            startMove = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player") || !(isMovable & !wasMoved) || startMove)
                return;

            isBeingInteractedWith = true;
            currentTime = waitTime;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.relativeVelocity == Vector2.zero)
                currentTime = waitTime;

            if (!other.gameObject.CompareTag("Player") || !isBeingInteractedWith)
                return;

            switch (moveDirection)
            {
                case BlockDirections.Up:
                    MoveUp(other);
                    break;
                case BlockDirections.Down:
                    MoveDown(other);
                    break;
                case BlockDirections.Left:
                    MoveLeft(other);
                    break;
                case BlockDirections.Right:
                    MoveRight(other);
                    break;
                case BlockDirections.UpDown:
                    MoveUp(other);
                    MoveDown(other);
                    break;
                case BlockDirections.LeftRight:
                    MoveLeft(other);
                    MoveRight(other);
                    break;
                case BlockDirections.All:
                    MoveUp(other);
                    MoveDown(other);
                    MoveLeft(other);
                    MoveRight(other);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && isBeingInteractedWith)
                isBeingInteractedWith = false;
        }
    }
}