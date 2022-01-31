using System.Collections;
using System.Collections.Generic;
using System.IO;
using LoLSDK;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LoL.Examples.Cooking
{
    [System.Serializable]
    public class CookingData
    {
        public int coins = 200;
        // use _ and not camelCasing for easy porting to server if needed.
        public int cost_of_pan = 70;
        public int num_of_pans;
        // You can use types not supported by unity serialization
        public Dictionary<int, string> food_in_pan = new Dictionary<int, string>();
        // Also nested types of other serialized objects.
        public List<FoodData> food;
    }

    [System.Serializable]
    public class FoodData
    {
        public string name;
        public int level;
        public float cooking_temp;
        public string image_key;
        public bool available = true;
    }

    public class ExampleCookingGame : MonoBehaviour
    {
        #region Mock Game Fields
#pragma warning disable 0649
        [SerializeField] Button panPrefab, foodPrefab, purchasePanButton, pantryButton, continueButton, newGameButton;
        [SerializeField] TextMeshProUGUI purchasePanText, coinText, feedbackText, newGameText, continueText, pantryText;
        [SerializeField] Transform panHolder;
        [SerializeField] Sprite steak, onion, broccoli;
        // Full game state data for my "cooking" game.
        // Default values set in editor.
        [SerializeField, Header("Initial State Data")] CookingData cookingData;
#pragma warning restore 0649

        Dictionary<string, Button> _food = new Dictionary<string, Button>();
        Button _selectedFood;
        bool _init;
        WaitForSeconds _feedbackTimer = new WaitForSeconds(2);
        Coroutine _feedbackMethod;
        JSONNode _langNode;
        string _langCode = "en";

        #endregion Mock Game Fields

        void Start()
        {
            purchasePanButton.onClick.AddListener(AddPan);
            pantryButton.onClick.AddListener(AddFoodToPantry);

            // Create the WebGL (or mock) object
            // This will all change in SDK V6 to be simplified and streamlined.
#if UNITY_EDITOR
            ILOLSDK sdk = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
			ILOLSDK sdk = new LoLSDK.WebGL();
#elif UNITY_IOS || UNITY_ANDROID
            ILOLSDK sdk = null; // TODO COMING SOON IN V6
#endif

            LOLSDK.Init(sdk, "com.legends-of-learning.unity.sdk.v5.1.example-cooking-game");

            // Register event handlers
            LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler(StartGame);
            LOLSDK.Instance.GameStateChanged += new GameStateChangedHandler(gameState => Debug.Log(gameState));
            LOLSDK.Instance.QuestionsReceived += new QuestionListReceivedHandler(questionList => Debug.Log(questionList));
            LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(LanguageUpdate);

            // Used for player feedback. Not required by SDK.
            LOLSDK.Instance.SaveResultReceived += OnSaveResult;

            // Call GameIsReady before calling LoadState or using the helper method.
            LOLSDK.Instance.GameIsReady();

#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(this);
            LoadMockData();
#endif

            // Helper method to hide and show the state buttons as needed.
            // Will call LoadState<T> for you.
            Helper.StateButtonInitialize<CookingData>(newGameButton, continueButton, OnLoad);
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif
            LOLSDK.Instance.SaveResultReceived -= OnSaveResult;
        }

        void Save()
        {
            LOLSDK.Instance.SaveState(cookingData);
        }

        void OnSaveResult(bool success)
        {
            if (!success)
            {
                Debug.LogWarning("Saving not successful");
                return;
            }

            if (_feedbackMethod != null)
                StopCoroutine(_feedbackMethod);
            // ...Auto Saving Complete
            _feedbackMethod = StartCoroutine(_Feedback(GetText("autoSave")));
        }

        void StartGame(string startGameJSON)
        {
            if (string.IsNullOrEmpty(startGameJSON))
                return;

            JSONNode startGamePayload = JSON.Parse(startGameJSON);
            // Capture the language code from the start payload. Use this to switch fonts
            _langCode = startGamePayload["languageCode"];
        }

        void LanguageUpdate(string langJSON)
        {
            if (string.IsNullOrEmpty(langJSON))
                return;

            _langNode = JSON.Parse(langJSON);

            TextDisplayUpdate();
        }

        string GetText(string key)
        {
            string value = _langNode?[key];
            return value ?? "--missing--";
        }

        // This could be done in a component with a listener attached to an lang change
        // instead of coupling all the text to a controller class.
        void TextDisplayUpdate()
        {
            pantryText.text = GetText("pantry");
            newGameText.text = GetText("newGame");
            continueText.text = GetText("continue");
            // "Purchase Pan  <color=#F9DD3B>{0}</color>"
            purchasePanText.text = string.Format(GetText("purchasePan"), cookingData.cost_of_pan);
        }

        /// <summary>
        /// This is the setting of your initial state when the scene loads.
        /// The state can be set from your default editor settings or from the
        /// users saved data after a valid save is called.
        /// </summary>
        /// <param name="loadedCookingData"></param>
        void OnLoad(CookingData loadedCookingData)
        {
            // Overrides serialized state data or continues with editor serialized values.
            if (loadedCookingData != null)
                cookingData = loadedCookingData;

            for (int i = 0; i < cookingData.num_of_pans; ++i)
            {
                CreatePan();
            }

            foreach (var food in cookingData.food)
            {
                CreateFood(food);
            }

            foreach (var kvp in cookingData.food_in_pan)
            {
                _selectedFood = _food[kvp.Value];
                AssignFood(panHolder.GetChild(kvp.Key));
            }

            coinText.text = cookingData.coins.ToString();

            // Initially set the text displays since the lang node should be populated.
            TextDisplayUpdate();

            // I use an init flag so I can call the same Set methods during initial load and during gameplay.
            // You don't have to follow this pattern, you can have init methods and gameplay methods separated.
            _init = true;
        }

        #region Mock Game Methods
        void AddPan()
        {
            if (_init)
            {
                cookingData.num_of_pans++;
                cookingData.coins -= cookingData.cost_of_pan;
                Save();
            }

            CreatePan();
            coinText.text = cookingData.coins.ToString();
        }

        void AddFoodToPantry()
        {
            if (_selectedFood == null)
                return;

            if (_init)
            {
                // Reset the food pan link.
                cookingData.food_in_pan.Remove(_selectedFood.transform.parent.GetSiblingIndex());
                Save();
            }

            _selectedFood.transform.SetParent(pantryButton.transform, false);
        }

        void CreatePan()
        {
            var pan = Instantiate(panPrefab, panHolder);
            pan.onClick.AddListener(() => AssignFood(pan.transform));
            pan.gameObject.SetActive(true);

            purchasePanButton.interactable = cookingData.coins >= cookingData.cost_of_pan && cookingData.num_of_pans < 4;
        }

        void AssignFood(Transform pan)
        {
            if (_selectedFood == null || pan.childCount > 0)
                return;

            _selectedFood.transform.SetParent(pan, false);
            // Account for offset.
            ((RectTransform)_selectedFood.transform).anchoredPosition = new Vector2(100, -160);

            if (_init)
            {
                cookingData.food_in_pan[pan.GetSiblingIndex()] = _selectedFood.name;
                Save();
            }
        }

        void CreateFood(FoodData foodData)
        {
            var food = Instantiate(foodPrefab, foodPrefab.transform.parent);
            food.name = foodData.name;
            food.interactable = foodData.available;
            food.onClick.AddListener(() => _selectedFood = food);
            food.GetComponent<Image>().sprite = GetFoodSprite(foodData.image_key);
            _food[foodData.name] = food;
            food.gameObject.SetActive(true);
        }

        // This would actually use addressables, just doing this as a quick, baked in example.
        Sprite GetFoodSprite(string image_key)
        {
            switch (image_key)
            {
                case "steak":
                    return steak;
                case "onion":
                    return onion;
                default:
                    return broccoli;
            }
        }

        IEnumerator _Feedback(string text)
        {
            feedbackText.text = text;
            yield return _feedbackTimer;
            feedbackText.text = string.Empty;
            _feedbackMethod = null;
        }
        #endregion Mock Game Methods

#if UNITY_EDITOR
        // Loading Mock Gameframe data
        // This will all be changed and streamlined in SDK V6
        private void LoadMockData()
        {
            // Load Dev Language File from StreamingAssets

            string startDataFilePath = Path.Combine(Application.streamingAssetsPath, "startGame.json");

            if (File.Exists(startDataFilePath))
            {
                string startDataAsJSON = File.ReadAllText(startDataFilePath);
                StartGame(startDataAsJSON);
            }

            // Load Dev Language File from StreamingAssets
            string langFilePath = Path.Combine(Application.streamingAssetsPath, "language.json");
            if (File.Exists(langFilePath))
            {
                string langDataAsJson = File.ReadAllText(langFilePath);
                var lang = JSON.Parse(langDataAsJson)[_langCode];
                LanguageUpdate(lang.ToString());
            }
        }
#endif

    }
}
