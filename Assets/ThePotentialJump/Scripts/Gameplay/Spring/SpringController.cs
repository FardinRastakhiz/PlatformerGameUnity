using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ThePotentialJump.Inputs;
using System.Collections;
using System;
using ThePotentialJump.Utilities;

namespace ThePotentialJump.Gameplay
{

    public class SpringController : MonoBehaviour
    {

        [SerializeField]
        SpringPhysics springPhysics;
        [SerializeField] SpringComponents springComponents;
        [SerializeField] EnergyUIContrller energyUIContrller;
        [SerializeField] JumpRuler ruler;

        private float compressMeasure = 0.0f;

        private void Awake()
        {
            energyUIContrller.SetupSlider(transform.position, GetEnergy(springComponents.MaxCompressCapacity));
            energyUIContrller.ValueFinalized += OnEnergyValueFinalized;
            energyUIContrller.ValueChanged += OnEnergyValueChanged;

            springComponents.SetupParameters(ruler);

            InputController.Instance.PressSpace += OnPressSpace;
            InputController.Instance.ReleaseSpace += OnReleaseSpace;
            HoldedEnergyChanged += OnEnergyValueChanged;
            springPhysics.SetParameters(springComponents.SetSpringSize, ruler);
        }


        private void OnEnergyValueFinalized(object o, HoldEnergyEventArgs e)
        {

            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            swingingCoroutine = StartCoroutine(springPhysics.Swing(GetAmplitude(e.Value)));
        }

        public void OnEnergyValueChanged(object o, HoldEnergyEventArgs e)
        {
            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            ruler.OnHideRuler();
            compressMeasure = GetAmplitude(e.Value);
            if (compressMeasure > springComponents.MaxCompressCapacity)
            {
                compressMeasure = springComponents.MaxCompressCapacity;
                springComponents.SetSpringSize(compressMeasure);
            }
            springComponents.SetSpringSize(compressMeasure);
        }

        private Coroutine swingingCoroutine;
        private Coroutine holdEnergyCoroutine;
        private void OnPressSpace(object o, EventArgs e)
        {
            if (isSwinging) return;
            holdEnergyCoroutine = StartCoroutine(HoldSpringEnergy());
        }

        private void OnReleaseSpace(object o, EventArgs e)
        {
            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            if (holdEnergyCoroutine != null)
                StopCoroutine(holdEnergyCoroutine);
            isSwinging = false;
            swingingCoroutine = StartCoroutine(springPhysics.Swing(GetAmplitude(holdedEnergy)));
        }

        [Space]
        [Header("Hold energy parameters")]
        [SerializeField] private float saveEnergyRate = 10.0f;
        private bool isSwinging;
        public event EventHandler<HoldEnergyEventArgs> HoldedEnergyChanged;
        private HoldEnergyEventArgs holdEnergyEventArgs = new HoldEnergyEventArgs();
        private WaitForSeconds waitFixedDeltaTime;
        private float holdedEnergy = 0;
        IEnumerator HoldSpringEnergy()
        {
            holdedEnergy = 0.0f;
            var addedSign = 1.0f;
            float maxEnergy = GetEnergy(springComponents.MaxCompressCapacity);
            while (!isSwinging)
            {
                holdedEnergy = holdedEnergy + Time.fixedDeltaTime * saveEnergyRate * addedSign;
                if ((holdedEnergy >= maxEnergy - Time.fixedDeltaTime && addedSign > 0)
                    || (holdedEnergy <= Time.fixedDeltaTime && addedSign < 0))
                    addedSign *= -1.0f;
                holdEnergyEventArgs.Value = holdedEnergy;
                energyUIContrller.SetUIValues(holdedEnergy);
                HoldedEnergyChanged?.Invoke(this, holdEnergyEventArgs);
                yield return waitFixedDeltaTime;
            }
        }


        public float GetEnergy(float compressMeasure)
        {
            return compressMeasure * compressMeasure * springPhysics.SpringConstant / 2.0f;
        }

        public float GetAmplitude(float energy)
        {
            return Mathf.Sqrt(2 * energy / springPhysics.SpringConstant);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Weight")
            {
                springPhysics.OnWeightCollided();
            }
        }
    }
}