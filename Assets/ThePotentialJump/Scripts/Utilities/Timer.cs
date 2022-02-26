using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        text.text = "0.00 s";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = (Mathf.Round(Time.timeSinceLevelLoad * 100.0f) / 100.0f) + " s";
    }
}
