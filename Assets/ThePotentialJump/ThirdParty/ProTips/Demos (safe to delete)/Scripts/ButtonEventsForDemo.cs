using TMPro;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

namespace ModelShark
{
    public class ButtonEventsForDemo : MonoBehaviour
    {
        public TextMeshProUGUI renderModeText;
        public Canvas canvas;
        public RectTransform rectToTilt;
        public ParticleSystem particles;
        public Image imageForScreenSpaceCamera;

        private readonly LoadSceneParameters lsp = new LoadSceneParameters { loadSceneMode = LoadSceneMode.Single, localPhysicsMode = LocalPhysicsMode.None };

        public void Start()
        {
            Time.timeScale = 1f; // Unpause time if it was paused in another scene.
            if (TooltipManager.Instance != null)
                canvas = TooltipManager.Instance.GuiCanvas;

            if (renderModeText != null)
            {
                switch (canvas.renderMode)
                {
                    case RenderMode.ScreenSpaceOverlay:
                        renderModeText.text = "Screen Space Overlay";
                        break;
                    case RenderMode.ScreenSpaceCamera:
                        renderModeText.text = "Screen Space Camera";
                        break;
                }
            }
        }

        public void BackToDemo()
        {
#if UNITY_EDITOR
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/ProTips/Demos (safe to delete)/2. ProTips Demo.unity", lsp);
#else
            SceneManager.LoadSceneAsync("Assets/ProTips/Demos (safe to delete)/2. ProTips Demo.unity", lsp);
#endif
        }

        public void GoToTemplates()
        {
#if UNITY_EDITOR
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/ProTips/Demos (safe to delete)/4. Tooltip Style Showcase.unity", lsp);
#else
            SceneManager.LoadSceneAsync("Assets/ProTips/Demos (safe to delete)/4. Tooltip Style Showcase.unity", lsp);
#endif
        }

        public void GoTo3DWorldObjectDemo()
        {
#if UNITY_EDITOR
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/ProTips/Demos (safe to delete)/3. 3D World Object Demo.unity", lsp);
#else
            SceneManager.LoadSceneAsync("Assets/ProTips/Demos (safe to delete)/3. 3D World Object Demo.unity", lsp);
#endif
        }

        /// <summary>Pauses time for the current scene by setting Time.timeScale = 0.</summary>
        public void PauseTime(TextMeshProUGUI displayText)
        {
            displayText.text = displayText.text == "Pause Time" ? "Unpause Time" : "Pause Time";

            Time.timeScale = Time.timeScale < 1f ? 1f : 0f;
        }

        public void ChangeCanvasRenderMode()
        {
            switch (canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    // Switch to ScreenSpaceCamera mode
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = TooltipManager.Instance.guiCamera;
                    renderModeText.text = "Screen Space Camera";
                    rectToTilt.localEulerAngles = new Vector3(0, 350, 0);
                    rectToTilt.anchoredPosition = new Vector2(70, 0);
                    if (imageForScreenSpaceCamera != null)
                        imageForScreenSpaceCamera.enabled = true;
                    break;
                case RenderMode.ScreenSpaceCamera:
                    // Switch to ScreenSpaceOverlay mode
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    renderModeText.text = "Screen Space Overlay";
                    rectToTilt.rotation = Quaternion.identity;
                    rectToTilt.anchoredPosition = new Vector2(0, 0);
                    if (imageForScreenSpaceCamera != null)
                        imageForScreenSpaceCamera.enabled = false;
                    break;
            }
            TooltipManager.Instance.ResetTooltipRotation();
        }

        public void ToggleParticles(TextMeshProUGUI displayText)
        {
            if (particles.gameObject.activeSelf)
            {
                particles.gameObject.SetActive(false);
                displayText.text = "Start Particles";
            }
            else
            {
                particles.gameObject.SetActive(true);
                displayText.text = "Stop Particles";
            }
        }
    }
}