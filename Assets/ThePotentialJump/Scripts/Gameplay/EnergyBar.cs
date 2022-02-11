using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.CharacterController;
using ThePotentialJump.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Gameplay
{

    public class EnergyBar : MonoBehaviour
    {
        [Header("Energy bar assets")]
        [SerializeField] private SpriteRenderer sliderFill;
        [SerializeField] private SpriteRenderer sliderFrame;
        [SerializeField] private Slider topViewSlider;
        [SerializeField] private TextMeshProUGUI topViewSliderText;

        [Space]
        [Header("Camera")]
        [SerializeField] private Camera cam;

        private Transform sliderTransform;
        [SerializeField] private float frameSize = 1.45f;

        private void Awake()
        {
            sliderTransform = sliderFill.transform;
            topViewSliderText = topViewSlider.transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        private Coroutine updateSliderCoroutine;
        private void Start()
        {
            BeginUpdating();
            PlatformerCharacterMovement.Instance.HoldedEnergyChanged += OnEnergyChanged;
        }

        [SerializeField]
        private float fillProporsion = 0;
        private float energyAmount = 0;


        private void OnEnable()
        {
            topViewSlider.gameObject.SetActive(true);
            PlatformerCharacterMovement.Instance.HoldedEnergyChanged += OnEnergyChanged;
        }

        private void OnDisable()
        {
            PlatformerCharacterMovement.Instance.HoldedEnergyChanged -= OnEnergyChanged;
            topViewSlider.gameObject.SetActive(false);
        }

        private void OnEnergyChanged(object o, HoldEnergyEventArgs e)
        {
            fillProporsion =  e.Value / PlatformerCharacterMovement.Instance.MaxEnergy;
            energyAmount = e.Value;
            FillEnergy();
            SetFrameTransform();
        }

        private void BeginUpdating()
        {
            StopUpdating();
            updateSliderCoroutine = StartCoroutine(UpdateSlider());
        }

        private void StopUpdating()
        {
            if (updateSliderCoroutine != null)
                StopCoroutine(updateSliderCoroutine);
        }

        private IEnumerator UpdateSlider()
        {
            while (true)
            {
                FillEnergy();
                SetFrameTransform();
                yield return null;
            }
        }

        private float lastFillProporsion = 0;
        private void FillEnergy()
        {
            if (Mathf.Abs(fillProporsion - lastFillProporsion) > float.Epsilon)
            {
                lastFillProporsion = fillProporsion;
                sliderFill.size = new Vector2(sliderFill.size.x, fillProporsion * frameSize);
                topViewSlider.value = fillProporsion;
                topViewSliderText.text = (int)(energyAmount) + " J";
            }
        }

        [SerializeField]
        private float baseDistance = 40.0f;
        [SerializeField]
        private float baseScale = 150.0f;
        [SerializeField]
        private float targetSpriteWidth = 45.0f;

        private float lastDistance = 0.0f;
        private void SetFrameTransform()
        {
            var distance = sliderFrame.transform.position.z - cam.transform.position.z;
            if (Mathf.Abs(distance - lastDistance) > float.Epsilon)
            {
                lastDistance = distance;
                var scale = (distance / baseDistance) * baseScale;
                sliderFrame.transform.localScale = new Vector3(scale, scale, 1);
                sliderFrame.transform.localPosition = new Vector3(sliderFrame.size.x * scale + targetSpriteWidth, 0.0f, 0.0f) * 0.5f;
            }
        }
    }

}