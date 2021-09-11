using Character;
using Managers;
using UnityEngine;

namespace Spell
{
    public class SpellScript : MonoBehaviour
    {
        [SerializeField] private float speed;
        private SpriteRenderer sprite;
        private Rigidbody2D rBody;
        private Vector2 direction;
        private bool isShooting;
        private bool isLockedOn;
        private static readonly int Impact = Animator.StringToHash("Impact");
        private int damage;
        private Transform source;

        private Transform Target { get; set; }

        private void Awake() => sprite = GetComponent<SpriteRenderer>();

        private void Start()
        {
            rBody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Transform target, int damageAmount, Transform source)
        {
            Target = target;
            damage = damageAmount;
            this.source = source;
        }

        private void FixedUpdate()
        {
            if (!isShooting)
                return;

            if (isLockedOn && Target != null)
                direction = Target.position - transform.position;

            rBody.velocity = direction.normalized * speed;
            float angle = Mathf.Atan2(direction.y, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void Shoot(Vector2 shootDir, bool lockedOn)
        {
            direction = shootDir;
            isLockedOn = lockedOn;
            isShooting = true;
            sprite.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("HitBox") && other.gameObject.name != "Player")
            {
                if (isLockedOn)
                    if (other.transform != Target)
                        return;

                if (Target == null)
                {
                    UiManager.Instance.ShowTargetFrame(other.gameObject.GetComponent<Enemy>());
                }

                Character.Character c = other.GetComponent<Character.Character>();

                if (c != null)
                {
                    speed = 0;
                    c.TakeDamage(damage, source);
                    GetComponent<Animator>().SetTrigger(Impact);
                    rBody.velocity = Vector2.zero;
                }
            }
        }
    }
}