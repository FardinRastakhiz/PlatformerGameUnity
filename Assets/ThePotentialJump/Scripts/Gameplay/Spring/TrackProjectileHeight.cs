using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class TrackProjectileHeight : MonoBehaviour
    {
        [SerializeField] private int reward = 5;
        [SerializeField] private int punishment = 2;
        [SerializeField] private float threshold = 0.5f;
        private WaitForSeconds waitForFixedUpdate;
        private float maxHeight = 0.0f;
        private DropZone dropZone;
        public void Awake()
        {
            waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
        }

        public IEnumerator BeginTracking(Transform Projectile, DropZone dropZone)
        {
            this.dropZone = dropZone;
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
                EconomySystem.Instance.Deposit(reward);
            }
            else if ((maxHeight < (dropZone.MinHeight - threshold)
                            && maxHeight > ((dropZone.MaxHeight - dropZone.MinHeight) + threshold))
                     || maxHeight > dropZone.MaxHeight + threshold)
            {
                EconomySystem.Instance.Withdraw(punishment);
            }
        }
    }
}