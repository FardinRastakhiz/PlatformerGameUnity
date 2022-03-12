using System;
using ThePotentialJump.CharacterController;
using ThePotentialJump.Utilities;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class CharacterJumpTracker : MonoSingleton<CharacterJumpTracker>
    {
        private float baseHeight = 0.0f;
        public int maxHeight = 0;
        public event EventHandler<NewHeightEventArgs> NewHeightApproached;
        private void Awake()
        {
            baseHeight = transform.position.y;
            PlatformerCharacterController.Instance.JumpEnd += OnPlatformLanded;
        }
        public void OnPlatformLanded(object o, EventArgs e)
        {
            var newHeight = (int)Mathf.Ceil(transform.position.y - baseHeight);
            if (newHeight > maxHeight)
            {
                NewHeightApproached?.Invoke(this, new NewHeightEventArgs
                {
                    JumpedHeight = newHeight - maxHeight,
                    MaxHeightApproached = newHeight
                });
                Debug.Log($"maxHeight: {newHeight}");
                maxHeight = newHeight;
            }
        }

    }

}