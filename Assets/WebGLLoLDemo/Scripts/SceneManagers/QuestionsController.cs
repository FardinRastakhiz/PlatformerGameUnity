using System.Collections;
using LoLSDK;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionsController : MonoBehaviour
{
    public GameObject imageTarget;
    public Button backButton;
    public Button nextButton;
    public Button prevButton;
    public Text questionCount;

    private int currentQuestionIdx = 0;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SetImage(null));
        this.renderQuestion(currentQuestionIdx);
        nextButton.onClick.AddListener(this.nextOnClick);
        prevButton.onClick.AddListener(this.prevOnClick);
    }

    // Private

    private void nextOnClick()
    {
        int nextIdx = currentQuestionIdx + 1;
        currentQuestionIdx = (nextIdx < SharedState.QuestionList.questions.Length)
            ? nextIdx
            : 0;

        this.renderQuestion(currentQuestionIdx);
    }

    private void updatePagination(int num, int total)
    {
        questionCount.text = "Question " + num + " of " + total;
    }

    private void prevOnClick()
    {
        currentQuestionIdx = currentQuestionIdx <= 1
            ? SharedState.QuestionList.questions.Length - 1
            : currentQuestionIdx - 1;

        this.renderQuestion(currentQuestionIdx);
    }

    private void renderQuestion(int questionIdx)
    {
        MultipleChoiceQuestion q = SharedState.QuestionList.questions[questionIdx];

        foreach (Alternative alternative in q.alternatives)
        {
            int idx = System.Array.IndexOf(q.alternatives, alternative) + 1;
            SetFieldText("Answer " + (idx), idx + ") " + alternative.text);
        }

        SetFieldText("QuestionText", q.stem);

        updatePagination(questionIdx + 1, SharedState.QuestionList.questions.Length);

        backButton.onClick.AddListener(OnClickBack);

        StartCoroutine(SetImage(q.imageURL));
    }

    private IEnumerator SetImage(string url)
    {
        Transform thumb = imageTarget.transform;
        Image img = thumb.GetComponent<Image>();

        // Clear image and early return, if none specified
        if (string.IsNullOrEmpty(url))
        {
            img.sprite = null;
            img.enabled = false;
            yield return null;
        }
        else
        {
            using (var www = UnityWebRequestTexture.GetTexture(url))
            {
                // Wait for download to complete
                yield return www.SendWebRequest();

                imageTarget.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                var temp = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;
                img.enabled = true;
            }
        }
    }

    private void SetFieldText(string objectName, string message)
    {
        Text messageText = GameObject.Find(objectName).GetComponent<Text>();
        messageText.text = message;
    }

    private void SetFieldFont(string objectName, string message)
    {
        // Text messageText =
        // messageText.font = (Font)Resources.Load("Fonts/FontName");;
    }

    private void OnClickBack()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

// 1. Layers 2-5 match up, one-to-one, with the ages of Layers 6-9. The layers to the right of the fault were uplifted, causing Layer 1 to erode away on the right side.\
// 圖層2-5與層6-9之間的一對一匹配。故障右側的層次被提升，導致第1層在右側消失
// 1.レイヤー2-5は、レイヤー6-9の年齢と1対1でマッチします。障害の右側にあるレイヤーが浮き上がり、レイヤー1が右側に崩壊しました。
