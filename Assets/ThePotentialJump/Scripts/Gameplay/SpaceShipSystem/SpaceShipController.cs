using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Gameplay
{
    public class SpaceShipController : MonoBehaviour
    {
        [SerializeField] RocketsController rocketsController;
        [SerializeField] Rigidbody2D spaceShipRigidBody;
        [SerializeField] Slider energySlider;
        [SerializeField] TMP_InputField energyInputField;
        [Space]

        [SerializeField] private string heightId;
        [SerializeField] private TextMeshProUGUI heightViewer;
        [SerializeField] private string speedId;
        [SerializeField] private TextMeshProUGUI speedViewer;
        [SerializeField] private string energyFlowId;
        [SerializeField] private TextMeshProUGUI accelerationViewer;

        private WaitForSeconds waitForFixedUpdate;
        private float energyRate = 0;
        private void Awake()
        {
            waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
        }

        private void Start()
        {
            spaceShipRigidBody.drag = 0;
            StartCoroutine(UpdateForce());
        }
        public void TurnOnSpaceShip()
        {
            rocketsController.TurnOnRockets();
        }

        public void EnergyRateChanged(Slider energySlider)
        {
            if (Mathf.Abs(energySlider.value - energyRate) > float.Epsilon * 3)
            {
                energyRate = energySlider.value;
                UpdateUIs(energyRate);
                rocketsController.UpdateRocketPower((energyRate / energySlider.maxValue) * 2.0f);
            }
        }
        public void EnergyRateChanged(TMP_InputField energyInputField)
        {
            var value = float.Parse(energyInputField.text);
            if (Mathf.Abs(value - energyRate) > float.Epsilon * 3)
            {
                energyRate = value;
                UpdateUIs(energyRate);
                rocketsController.UpdateRocketPower((energyRate / energySlider.maxValue) * 2.0f);
            }
        }

        IEnumerator UpdateForce()
        {
            float heightInMeter = 0.0f;
            float speedInMeter = 0.0f;
            float accelerationInMeter = 0.0f;
            float heightInFoot = 0.0f;
            float speedInFoot = 0.0f;
            float accelerationInFoot = 0.0f;
            while (true)
            {
                spaceShipRigidBody.AddForce(Vector2.up * energyRate);

                heightInMeter = transform.position.y;
                speedInMeter = spaceShipRigidBody.velocity.y;
                accelerationInMeter = energyRate / spaceShipRigidBody.mass;
                heightInFoot = heightInMeter * 3.28f;
                speedInFoot = speedInMeter * 3.28f;
                accelerationInFoot = accelerationInMeter * 3.28f;
                heightViewer.text = $"{SharedState.LanguageDefs?[heightId]}  = {heightInMeter.ToString("F1")} m or {heightInFoot.ToString("F1")} ft";
                speedViewer.text = $"{SharedState.LanguageDefs?[speedId]} = {speedInMeter.ToString("F1")} m/s or {speedInFoot.ToString("F1")} ft/s";
                accelerationViewer.text = $"{SharedState.LanguageDefs?[energyFlowId]} = {energyRate} J"; // {accelerationInMeter.ToString("F1")} m/s^2 or {accelerationInFoot.ToString("F1")} ft/s^2";
                yield return waitForFixedUpdate;
            }
        }

        private void UpdateUIs(float energyValue)
        {
            energySlider.value = energyValue;
            energyInputField.text = $"{energyValue}";
        }
    }
}