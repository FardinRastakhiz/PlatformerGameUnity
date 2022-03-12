using UnityEngine;
using SimpleJSON;
using TMPro;

namespace ThePotentialJump
{
    public class TextPresenter : MonoBehaviour
    {
        [SerializeField] protected string id;
        [SerializeField] protected TextMeshProUGUI textComponent;
        private void Awake()
        {
            if (textComponent == null) textComponent = GetComponent<TextMeshProUGUI>();
            if (textComponent == null) Debug.LogError("The object doesn't have text mesh pro!");
        }
        public virtual void AlterLanguage(JSONNode langNode)
        {
            textComponent.text = langNode?[id];
        }
    }

}