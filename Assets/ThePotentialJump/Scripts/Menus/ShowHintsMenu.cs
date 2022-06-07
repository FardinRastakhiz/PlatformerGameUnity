using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Menus
{
    public class ShowHintsMenu : MonoBehaviour
    {

        [SerializeField] private Sprite showButtonIcon;
        [SerializeField] private Sprite hideButtonIcon;
        [SerializeField] private Image showHideButtonIconImage;
        [SerializeField] private TextMeshProUGUI showHideButtonText;

        [SerializeField] private FadeInOutCanvasGroup hintsMenu;

        public void Show()
        {
            hintsMenu.FadeIn();
            showHideButtonIconImage.sprite = hideButtonIcon;
            showHideButtonText.text = " Hide Hints";
        }

        public void Hide()
        {
            hintsMenu.FadeOut();
            showHideButtonIconImage.sprite = showButtonIcon;
            showHideButtonText.text = " Show Hints";
        }
    }
}
