using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Objects
{
    public class ShowHideObject : MonoBehaviour
    {
        [SerializeField] protected TargetAction action;

        [ShowIf("@action != TargetAction.None")]
        [SerializeField] protected GameObject targetGameObject;

#if UNITY_EDITOR
        [ShowIf("@action != TargetAction.None")]
        [SerializeField] protected bool showConnection = true;

        [ShowIf("@action != TargetAction.None")]
        [SerializeField] protected Color connectionColor = Color.magenta;
#endif

        private void Awake() => Clear();

        protected virtual void Clear()
        {
        }

        protected virtual void Start()
        {
            if (action != TargetAction.None)
            {
                if (targetGameObject == null)
                {
                    Debug.LogWarning("WARNING - A Target GameObject has not been set.");
                    HighlightObject();
                }
            }

            switch (action)
            {
                case TargetAction.Show:
                    targetGameObject.SetActive(false);
                    break;
                case TargetAction.Hide:
                    targetGameObject.SetActive(true);
                    break;
                case TargetAction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void Update()
        {
        }

        protected void HighlightObject() => GetComponent<SpriteRenderer>().color = Color.red;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (targetGameObject == null || !showConnection)
                return;

            Gizmos.color = connectionColor;
            var position = transform.position;
            var targetPosition = targetGameObject.transform.position;
            Gizmos.DrawLine(position, targetPosition);
            Gizmos.DrawWireSphere(position, 0.5f);
            Gizmos.DrawWireSphere(targetPosition, 0.5f);
        }
#endif
    }
}