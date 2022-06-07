using System;
using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.Inventory;
using ThePotentialJump.Utilities;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class EnergyCalculator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI formulaText;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private HeightViewer heightViewer;
        [SerializeField] private InventorySystem inventorySystem;

        private float mass = 20.0f;
        private float height = 2.0f;
        private float energy = 0.0f;

        private void Start()
        {
            UpdateText();
            heightViewer.HeightChanged += OnHeightChanged;
            inventorySystem.CellActivated += OnCellActivated;
        }

        private void OnCellActivated(object o, CellActivatedEventArgs e)
        {
            if (e.ActivatedCell == null) return;
            if (e.ActivatedCell.Content.DropItame is FreeDropWeight dropWeight)
            {
                OnWeightSelected(dropWeight.Mass);
            }
        }

        public void OnWeightSelected(float mass)
        {
            this.mass = mass;
            UpdateText();
        }

        public void OnHeightChanged(object o, HeightChangedEventArgs e)
        {
            this.height = e.Height;
            UpdateText();
        }

        private void UpdateText()
        {
            formulaText.text = $"E = mgh = ({mass.ToString("F1")} kg) x (9.81 kg/m2) x ({height.ToString("F2")} m)";
        }

        private Coroutine resultCoroutine;
        public void UpdateResultSuccess()
        {
            if (resultCoroutine != null) StopCoroutine(resultCoroutine);
            energy = mass * 9.81f * height;
            resultText.text = $"E = <color=green>{energy.ToString("F1")} J</color>";
            resultCoroutine = StartCoroutine(ResetResult());
        }
        public void UpdateResultFail()
        {
            if (resultCoroutine != null) StopCoroutine(resultCoroutine);
            energy = mass * 9.81f * height;
            resultText.text = $"E = <color=orange>{energy.ToString("F1")} J</color>";
            resultCoroutine = StartCoroutine(ResetResult());
        }

        IEnumerator ResetResult()
        {
            yield return new WaitForSeconds(10);
            resultText.text = $"E = ...";
        }
    }

}
