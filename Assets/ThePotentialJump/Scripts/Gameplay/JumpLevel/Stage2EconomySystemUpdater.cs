using System;
using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.CharacterController;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class Stage2EconomySystemUpdater : MonoBehaviour
    {
        [SerializeField] private int RewardPer10Meter = 5;
        [SerializeField] private int punishment = 2;
        private void Awake()
        {
            CharacterJumpTracker.Instance.NewHeightApproached += OnNewHeightApproached;
            PlatformerCharacterController.Instance.HitCeiling += OnHitCeiling;


        }

        private void OnNewHeightApproached(object sender, NewHeightEventArgs e)
        {
            EconomySystem.Instance.Deposit((e.JumpedHeight * RewardPer10Meter / 10));
        }

        private void OnHitCeiling(object o, EventArgs e)
        {
            EconomySystem.Instance.Withdraw(punishment);
        }
    }

}