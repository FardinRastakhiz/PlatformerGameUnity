using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class BindPosition : MonoBehaviour
    {
        [SerializeField] private Transform targetBinding;
        [SerializeField] bool bindX;
        [SerializeField] bool bindY;
        [SerializeField] bool bindZ;

        private Coroutine updateCoroutine;
        private void Start()
        {
            if (targetBinding != null)
                updateCoroutine = StartCoroutine(UpdatePosition());
        }

        public void StartBinding(Transform targetBinding)
        {
            if (targetBinding == null) throw new ArgumentNullException($"'targetBinding' cannot be null");
            this.targetBinding = targetBinding;
            if (updateCoroutine != null) StopCoroutine(updateCoroutine);
            updateCoroutine = StartCoroutine(UpdatePosition());
        }

        public void StopBinding()
        {
            if (updateCoroutine != null) StopCoroutine(updateCoroutine);
        }

        IEnumerator UpdatePosition()
        {
            var targetBasePosition = targetBinding.position;
            var basePosition = transform.position;
            while (true)
            {
                transform.position = new Vector3(
                    bindX ? targetBinding.position.x - targetBasePosition.x : 0,
                    bindY ? targetBinding.position.y - targetBasePosition.y : 0,
                    bindZ ? targetBinding.position.z - targetBasePosition.z : 0
                    ) + basePosition;
                yield return null;
            }
        }
    }
}