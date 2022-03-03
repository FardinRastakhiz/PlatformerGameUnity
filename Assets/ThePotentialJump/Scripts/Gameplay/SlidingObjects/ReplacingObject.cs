using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.Gameplay;
using ThePotentialJump.Utilities;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class ReplacingObject : MonoBehaviour
    {
        [SerializeField] private GameObject defaultReplaceObjectPrefab;
        [SerializeField] private float fadeOutDelay;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private Replacable replacable;

        private WaitForSeconds waitForDelay;
        private WaitForSeconds waitForFixedUpdate;
        
        private void Awake()
        {
            waitForDelay = new WaitForSeconds(fadeOutDelay);
            waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
            replacable.Replace += ReplaceTheObject;
        }
        public void ReplaceTheObject(object o, ReplaceObjectEventArgs e)
        {
            if (e.ReplacePrefab == null) e.ReplacePrefab = defaultReplaceObjectPrefab;
            if(e.ReplacePrefab == null)
            {
                Debug.LogError("there is nothing to replace!");
                return;
            }
            if(e.Parent == null)
            {
                Debug.LogError("Parent is null");
                return;
            }
            var placeObject = Instantiate(e.ReplacePrefab, e.Position, Quaternion.identity, e.Parent);
            SpritesGroup spritesGroup = placeObject.GetComponent<SpritesGroup>();
            if (spritesGroup == null)
                spritesGroup = placeObject.AddComponent<SpritesGroup>();
            StartCoroutine(FadeOut(spritesGroup));
        }

        IEnumerator FadeOut(SpritesGroup canvasGroup)
        {
            yield return waitForDelay;
            while (canvasGroup.Alpha > 0)
            {
                canvasGroup.Alpha -= Time.fixedDeltaTime / fadeOutDuration;
                yield return waitForFixedUpdate;
            }
        }
    }

}