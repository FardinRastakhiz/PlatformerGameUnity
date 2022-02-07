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

        [Header("Spring parameters")]
        [SerializeField] private SpriteRenderer spring;
        [SerializeField] private float springConstant = 0.1f;
        [SerializeField] private float dampingFactor = 0.1f;
        [SerializeField] private Rigidbody2D loadedweight;
        [SerializeField] private float springHeadMass;

        private float IdleHeight = 7.0f;
        private float maxCompressCapacity = 3.5f;
        private float compressMeasure = 0.0f;

        private void Awake()
        {
            SetSlider();
            IdleHeight = spring.size.y;
            InputController.Instance.PressSpace += OnPressSpace;
            InputController.Instance.ReleaseSpace += OnReleaseSpace;
        }

        [Header("Energy slider parameters")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ExtendedSlider energySlider;
        [SerializeField] private Vector3 sliderOffset;
        private void SetSlider()
        {
            energySlider.value = 0;
            energySlider.maxValue = GetEnergy(maxCompressCapacity);
            energySlider.minValue = 0;
            energySlider.onValueChanged.AddListener(OnSliderDrag);
            energySlider.PointerReleased += OnSliderRelease;
            energySlider.transform.position = spring.transform.position + sliderOffset;
            inputField.onValueChanged.AddListener(OnInputValue);
        }

        private void OnSliderDrag(float value)
        {
            SetPotentialEnergy(value);
        }

        private void OnSliderRelease(object o, SliderEventArgs e)
        {

            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            swingingCoroutine = StartCoroutine(Swing(GetAmplitude(e.Value)));
        }

        private void OnInputValue(string inputValue)
        {
            var value = float.Parse(inputValue);
            SetPotentialEnergy(value);
        }

        public float SetPotentialEnergy(float energy)
        {
            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            compressMeasure = Mathf.Sqrt(2 * energy / springConstant);
            if (compressMeasure > maxCompressCapacity)
            {
                compressMeasure = maxCompressCapacity;
                spring.size = new Vector2(spring.size.x, IdleHeight - compressMeasure);
                return GetEnergy(compressMeasure);
            }
            spring.size = new Vector2(spring.size.x, IdleHeight - compressMeasure);

            return energy;
        }

        private Coroutine swingingCoroutine;
        private void OnPressSpace(object o, EventArgs e)
        {
            if (isSwinging) return;
            StartCoroutine(HoldSpringEnergy());
        }

        private void OnReleaseSpace(object o, EventArgs e)
        {
            if (swingingCoroutine != null)
                StopCoroutine(swingingCoroutine);
            swingingCoroutine = StartCoroutine(Swing(GetAmplitude(holdedEnergy)));
        }

        [Space]
        [Header("Hold energy parameters")]
        [SerializeField] private float swingThreshold = 0.1f;
        [SerializeField] private float saveEnergyRate = 10.0f;
        private bool isSwinging;
        public event EventHandler<HoldEnergyEventArgs> HoldedEnergyChanged;
        private HoldEnergyEventArgs holdEnergyEventArgs = new HoldEnergyEventArgs();
        private WaitForSeconds waitFixedDeltaTime;
        private float holdedEnergy = 0;
        IEnumerator HoldSpringEnergy()
        {
            holdedEnergy = 0.0f;
            float maxEnergy = GetEnergy(maxCompressCapacity);
            while (!isSwinging)
            {
                holdedEnergy = holdedEnergy >= maxEnergy ? maxEnergy : holdedEnergy + Time.fixedDeltaTime * saveEnergyRate;
                holdEnergyEventArgs.HoldedEnergy = holdedEnergy;
                HoldedEnergyChanged?.Invoke(this, holdEnergyEventArgs);
                yield return waitFixedDeltaTime;
            }
        }

        IEnumerator Swing(float startAmplitude)
        {
            isSwinging = true;
            float addedmass = loadedweight == null ? 0 : loadedweight.mass;
            float overalMass = addedmass + springHeadMass;
            var delta = dampingFactor * dampingFactor - 4 * springConstant * overalMass;
            if (delta > float.Epsilon)
                yield return OverDamepedSwing(startAmplitude, 0);
            else if (delta < -float.Epsilon)
                yield return UnderDampedSwing(startAmplitude, 0);
            else
                yield return CriticalSwing(startAmplitude, 0);
        }

        private IEnumerator OverDamepedSwing(float startAmplitude, float velocity)
        {
            var addedmass = loadedweight == null ? 0 : loadedweight.mass;
            var overalMass = addedmass + springHeadMass;
            var delta = dampingFactor * dampingFactor - 4 * springConstant * overalMass;
            var s1 = (-dampingFactor + Mathf.Sqrt(delta)) / (2 * overalMass);
            var s2 = (-dampingFactor - Mathf.Sqrt(delta)) / (2 * overalMass);
            var B = (velocity - startAmplitude * s1) / (s2 - s1);
            var A = startAmplitude - B;

            var t = 0.0f;
            var last_x = 0.0f;
            do
            {
                var x_t = A * Mathf.Exp(s1 * t) + B * Mathf.Exp(s2 * t);
                var v = Mathf.Abs(x_t - last_x) / Time.fixedDeltaTime;
                if (Mathf.Abs(x_t) < swingThreshold && Mathf.Abs(v) < swingThreshold)
                {
                    x_t = 0;
                    isSwinging = false;
                }
                spring.size = new Vector2(spring.size.x, IdleHeight - x_t);
                last_x = x_t;
                t += Time.fixedDeltaTime;
                yield return waitFixedDeltaTime;
            } while (isSwinging);
        }

        private IEnumerator CriticalSwing(float startAmplitude, float velocity)
        {
            var addedmass = loadedweight == null ? 0 : loadedweight.mass;
            var overalMass = addedmass + springHeadMass;
            var A = startAmplitude;
            var B = velocity + (A * dampingFactor) / (2 * overalMass);
            var expFactor = -dampingFactor / (2 * overalMass);
            var t = 0.0f;
            var last_x = 0.0f;
            do
            {
                var x_t = (A + B * t) * Mathf.Exp(expFactor * t);
                var v = Mathf.Abs(x_t - last_x) / Time.fixedDeltaTime;
                if (Mathf.Abs(x_t) < swingThreshold && Mathf.Abs(v) < swingThreshold)
                {
                    x_t = 0;
                    isSwinging = false;
                }
                spring.size = new Vector2(spring.size.x, IdleHeight - x_t);
                last_x = x_t;
                t += Time.fixedDeltaTime;
                yield return waitFixedDeltaTime;
            } while (isSwinging);
        }

        private IEnumerator UnderDampedSwing(float startAmplitude, float velocity)
        {
            float addedmass = loadedweight == null ? 0 : loadedweight.mass;
            float overalMass = addedmass + springHeadMass;
            var delta = 4 * springConstant * overalMass - dampingFactor * dampingFactor;
            delta = Mathf.Sqrt(delta) / (2 * overalMass);
            var A = startAmplitude;
            var B = (velocity + (A * dampingFactor) / (2 * overalMass)) / delta;
            var expFactor = -dampingFactor / (2 * overalMass);
            var t = 0.0f;
            var last_x = 0.0f;
            do
            {
                var x_t = Mathf.Exp(expFactor * t) * (A * Mathf.Cos(delta * t) + B * Mathf.Sin(delta * t));
                var v = Mathf.Abs(x_t - last_x) / Time.fixedDeltaTime;
                if (Mathf.Abs(x_t) < swingThreshold && Mathf.Abs(v) < swingThreshold)
                {
                    x_t = 0;
                    isSwinging = false;
                }
                spring.size = new Vector2(spring.size.x, IdleHeight - x_t);
                last_x = x_t;
                t += Time.fixedDeltaTime;
                yield return waitFixedDeltaTime;
            } while (isSwinging);
        }

        public float GetEnergy(float compressMeasure)
        {
            return compressMeasure * compressMeasure * springConstant / 2.0f;
        }

        public float GetAmplitude(float energy)
        {
            return Mathf.Sqrt(2 * energy / springConstant);
        }
    }
}