using SimpleJSON;
using ThePotentialJump;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class ToleranceEnergyLabel : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI textComponent;
        [SerializeField] protected string id;
        [SerializeField] private BindPosition bindPosition;
        [SerializeField] private Vector3 offset;
        [SerializeField] private FadeInOutCanvasGroup fadeInOut;


        public void SetTarget(Transform targetTransform, float energy)
        {
            fadeInOut.FadeIn();
            transform.position = targetTransform.position + offset;
            textComponent.text = $"{SharedState.LanguageDefs?[id]} = {energy.ToString("F1")} J";
            bindPosition.StartBinding(targetTransform);
        }

        public void StopTarget()
        {
            fadeInOut?.FadeOut();
            bindPosition?.StopBinding();
        }
    }
}
