using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketsController : MonoBehaviour
{
    [SerializeField] private float basePower = 0.4f;
    [SerializeField] private ParticleSystem[] rocketParticles;


    public void TurnOnRockets()
    {
        for (int i = 0; i < rocketParticles.Length; i++)
            rocketParticles[i].Play();
        UpdateRocketPower(0);
    }

    public void UpdateRocketPower(float power)
    {
        for (int i = 0; i < rocketParticles.Length; i++)
        {
            var main = rocketParticles[i].main;
            main.startLifetime = basePower + power;
            Debug.Log($"main.startLifetime: {main.startLifetime}");
        }
    }
}
