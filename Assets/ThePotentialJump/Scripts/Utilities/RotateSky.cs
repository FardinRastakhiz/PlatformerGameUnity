using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePotentialJump.EditorUtilities
{
    public class RotateSky : MonoBehaviour
    {
        [SerializeField] private MeshRenderer skyRenderer;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Vector2 offsetSpeed;
        private Vector2 textureOffset;

        private Transform skyTransform;
        private Material skyMaterial;
        // Start is called before the first frame update
        void Start()
        {
            skyTransform = skyRenderer.transform;
            skyMaterial = skyRenderer.sharedMaterial;
            textureOffset = skyMaterial.GetTextureOffset("_MainTex");
        }

        private void FixedUpdate()
        {
            var eulerAngles = skyTransform.localEulerAngles;
            eulerAngles.y += Time.fixedDeltaTime * rotationSpeed;
            skyTransform.localEulerAngles = eulerAngles;
            textureOffset += offsetSpeed * Time.fixedDeltaTime;
            skyMaterial.SetTextureOffset("_MainTex", textureOffset);
        }
    }
}