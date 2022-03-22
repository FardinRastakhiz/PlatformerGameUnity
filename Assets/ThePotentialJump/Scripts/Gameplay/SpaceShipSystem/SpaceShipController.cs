using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private string accelerationId;
    [SerializeField] private TextMeshProUGUI accelerationViewer;

    private WaitForSeconds waitForFixedUpdate;
    private float energyRate = 0;
    private void Awake()
    {
        waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
    }

    private void Start()
    {
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
        while (true)
        {
            spaceShipRigidBody.AddForce(Vector2.up * energyRate);
            heightViewer.text = $"{SharedState.LanguageDefs?[heightId]}  = {Mathf.Round(transform.position.y * 100) / 100.0f} m";
            speedViewer.text = $"{SharedState.LanguageDefs?[speedId]} = {Mathf.Round(spaceShipRigidBody.velocity.y * 100) / 100.0f} m/s";
            accelerationViewer.text = $"{SharedState.LanguageDefs?[accelerationId]} = {Mathf.Round((energyRate / spaceShipRigidBody.mass) * 100) / 100.0f} m/s^2";
            yield return waitForFixedUpdate;
        }
    }

    private void UpdateUIs(float energyValue)
    {
        energySlider.value = energyValue;
        energyInputField.text = $"{energyValue}";
    }
}
