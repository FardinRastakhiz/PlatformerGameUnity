using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class TrackProjectileHeight : MonoBehaviour
    {
        [SerializeField] private int reward = 5;
        [SerializeField] private int punishment = 2;
        [SerializeField] private float threshold = 0.5f;
        [SerializeField] private UnityEvent DroppedWithCorrectHeight;
        [SerializeField] private UnityEvent DroppedWithWrongHeight;
        private WaitForSeconds waitForFixedUpdate;
        private float maxHeight = 0.0f;
        private DropZone dropZone;
        //public void Awake()
        //{
            //Debug.Log($"Awake1 {this.GetType().Name}");
            //waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
            //Debug.Log($"Awake2 {this.GetType().Name}");
            //Debug.Log($"");
            //Debug.Log($"");

            //UnityEngine.Object[] objects;

            //objects = UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour));
            //foreach (MonoBehaviour monoBehaviour in objects)
            //{
            //    Debug.Log(monoBehaviour.GetType().Name + " is attached to " + monoBehaviour.gameObject.name);
            //}
            //Debug.Log($"");
            //Debug.Log($"");
        //}


        public IEnumerator BeginTracking(Transform Projectile, DropZone dropZone)
        {
            this.dropZone = dropZone;
            maxHeight = 0;
            while (Projectile)
            {
                var projectileHeight = Projectile.position.y - dropZone.BaseHeight;
                if (projectileHeight > maxHeight) maxHeight = projectileHeight;
                yield return waitForFixedUpdate;
            }
        }

        public void OnProjectileDestroyed(object o, EventArgs e)
        {
            if (maxHeight > (dropZone.MinHeight - threshold) && maxHeight < (dropZone.MaxHeight + threshold))
            {
                EconomySystem.Instance?.Deposit(reward);
                DroppedWithCorrectHeight?.Invoke();
            }
            else if ((maxHeight < (dropZone.MinHeight - threshold)
                            && maxHeight > ((dropZone.MaxHeight - dropZone.MinHeight) + threshold))
                     || maxHeight > dropZone.MaxHeight + threshold)
            {
                EconomySystem.Instance?.Withdraw(punishment);
                DroppedWithWrongHeight?.Invoke();
            }
        }
    }
}