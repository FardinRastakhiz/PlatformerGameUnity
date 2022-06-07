using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ThePotentialJump.Inputs;
using System.Collections;
using System;
using ThePotentialJump.Utilities;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{

    public class SpringController : MonoBehaviour
    {

        [SerializeField] SpringPhysics springPhysics;
        [SerializeField] SpringComponents springComponents;
        [SerializeField] EnergyUIController energyUIController;
        [SerializeField] JumpRuler ruler;
        [SerializeField] private bool isDisabled = false;

        private float compressMeasure = 0.0f;
        private WaitForSeconds waitFixedDeltaTime;

        private void Awake()
        {
            energyUIController.SetupSlider(transform.position, GetEnergy(springComponents.MaxCompressCapacity));
            energyUIController.ValueFinalized += OnEnergyValueFinalized;
            energyUIController.ValueChanged += OnEnergyValueChanged;

            springComponents.SetupParameters(ruler);
            waitFixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);

            HoldedEnergyChanged += OnEnergyValueChanged;
            springPhysics.SetParameters(springComponents.SetSpringSize, ruler);

        }
        private void Start()
        {
            InputController.Instance.PressSpace += OnPressSpace;
            InputController.Instance.ReleaseSpace += OnReleaseSpace;
            if (isDisabled) DisableSpring();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Weight")
            {
                springPhysics.OnWeightCollided();
            }
        }

        public void AddProjectile(Projectile2D projectile, Sprite springSprite, bool resetConstant = false)
        {
            springPhysics.AddProjectile(projectile, resetConstant);
            if (resetConstant)
            {
                energyUIController.SetupSlider(transform.position, GetEnergy(springComponents.MaxCompressCapacity));
                springComponents.ChangeSpring(springSprite);
            }
        }

        public void EnableSpring()
        {
            isDisabled = false;
            StartCoroutine(energyUIController.Enable());
        }

        public void DisableSpring()
        {
            isDisabled = true;
            springPhysics.Disable();
            if(energyUIController!=null && this.gameObject.activeSelf) StartCoroutine(energyUIController.Disable());
        }

        public UnityEvent SprignCompressing;
        private void OnEnergyValueChanged(object o, HoldEnergyEventArgs e)
        {
            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            ruler.OnHideRuler();
            compressMeasure = GetAmplitude(e.Value);
            if (compressMeasure > springComponents.MaxCompressCapacity)
            {
                compressMeasure = springComponents.MaxCompressCapacity;
                //springComponents.SetSpringSize(compressMeasure);
            }
            springComponents.SetSpringSize(compressMeasure);
            SprignCompressing?.Invoke();
        }

        private Coroutine swingingCoroutine;
        private Coroutine holdEnergyCoroutine;


        [Space]
        [Header("Hold energy parameters")]
        [SerializeField] private float saveEnergyRate = 10.0f;
        private bool isSwinging;
        public event EventHandler<HoldEnergyEventArgs> HoldedEnergyChanged;
        private HoldEnergyEventArgs holdEnergyEventArgs = new HoldEnergyEventArgs();
        private float holdedEnergy = 0;
        private IEnumerator HoldSpringEnergy()
        {
            holdedEnergy = 0.0f;
            var addedSign = 1.0f;
            float maxEnergy = GetEnergy(springComponents.MaxCompressCapacity);
            while (!isSwinging && !isDisabled)
            {
                holdedEnergy = holdedEnergy + Time.fixedDeltaTime * saveEnergyRate * addedSign;
                if ((holdedEnergy >= maxEnergy - Time.fixedDeltaTime && addedSign > 0)
                    || (holdedEnergy <= Time.fixedDeltaTime && addedSign < 0))
                    addedSign *= -1.0f;
                holdEnergyEventArgs.Value = holdedEnergy;
                energyUIController.SetUIValues(holdedEnergy);
                HoldedEnergyChanged?.Invoke(this, new HoldEnergyEventArgs { Value = holdedEnergy });
                yield return waitFixedDeltaTime;
            }
        }


        private void OnPressSpace(object o, EventArgs e)
        {
            if (isSwinging || isDisabled) return;
            holdEnergyCoroutine = StartCoroutine(HoldSpringEnergy());
        }
        private void OnReleaseSpace(object o, EventArgs e)
        {
            if (isDisabled) return;
            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            if (holdEnergyCoroutine != null)
                StopCoroutine(holdEnergyCoroutine);
            isSwinging = false;
            swingingCoroutine = StartCoroutine(springPhysics.Swing(GetAmplitude(holdedEnergy)));
        }

        private void OnEnergyValueFinalized(object o, HoldEnergyEventArgs e)
        {
            if (isDisabled) return;
            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            swingingCoroutine = StartCoroutine(springPhysics.Swing(GetAmplitude(e.Value)));
        }


        private float GetEnergy(float compressMeasure)
        {
            return compressMeasure * compressMeasure * springPhysics.SpringConstant / 2.0f;
        }

        private float GetAmplitude(float energy)
        {
            return Mathf.Sqrt(2 * energy / springPhysics.SpringConstant);
        }

    }
}