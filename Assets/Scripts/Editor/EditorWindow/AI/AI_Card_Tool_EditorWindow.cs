using NUnit.Framework;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Images;
using OpenAI_API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using CodiceApp;
using System.ComponentModel;


public class AI_Card_Tool_EditorWindow : EditorWindow
{
    [SerializeField] OpenAIAPI openAIAPI = null;

    [SerializeField] GameObject goImageTarget = null;
    [SerializeField] Material material = null;
    [SerializeField] Shader shader = null;
    [SerializeField] CardInfo cardInfo = null;
    [SerializeField] CardCreator cardCreator = null;

    ImageGenerationRequest imageGeneration = null;
    //IImageGenerationEndpoint iImageGenerationEndpoint = null;
    ImageGenerationEndpoint imageGenerationEndpoint = null;
    ImageResult imageResult = null;
    RawImage rawImage = null;

    public string userInputPrompt = "";
    public string generatedImageURL = "";
    public string manualURL = "";
    public string API_KEY = "";
    public string tempKey = "";

    public bool urlHasBeenGenerated = false;
    public bool openImageInBrowser = false;
    public bool canRevealPassword = false;

    //2D icons
    private Texture2D revealPassWordIcon = null;


    //Events
    public event Action<Texture2D> onTextureLoadedFromURL = null;
    public event Action<string> onPasswordEntered = null;
    public event Action<bool> onRevealPasswordButtonClicked = null;
    // Chat events
    public event Action<string> onFlavourTextGenerated = null;
    // Not event to be able to call it inside the CardInfo SO
    public Action<string> onFlavorTextSelected = null;

    //Tabs
    public int tabs = 3;
    string[] tabSelection = new string[] { "Image Generation", "Credentials", "Chat" };
    // AI Properties
    [UnityEngine.Range(0.4f, 1.6f)] public float temperature = 0.8f;
    public string flavorTextStyle = "";
    public string currentFlavorText = "";
    List<string> allFlavortexts = null;


    //
    // Styles
    // Serialized Properties
    SerializedProperty serializedProperty = null;
    // SerializedObjects

    // Accessors
    public OpenAIAPI OpenAIAPI { get { return openAIAPI; } }
    public float Temperature => temperature;

    [MenuItem("Tools/AI Helper")]
    public static void ShowWindow()
    {
        //GetWindow<ImageGeneratorTool_EditorWindow>("AI Image Generator").Show();
        AI_Card_Tool_EditorWindow t = GetWindow<AI_Card_Tool_EditorWindow>(typeof(AI_Card_Tool_EditorWindow));
    }


    private void OnEnable()
    {
        Init();
        InitEvents();

        allFlavortexts = new List<string>();
        onRevealPasswordButtonClicked += SetCanRevealPassword;
        Init2DTextures();


    }

    private void Init()
    {
        bool _hasSetTemperature = false;
        if (!_hasSetTemperature)
            SetTemperature(0.5f);
        _hasSetTemperature = true;
    }

    private void InitEvents()
    {
        onTextureLoadedFromURL += SetGameObjectMaterial;
        onPasswordEntered += Authenticate;
        onFlavourTextGenerated += SetFlavorText;
        onFlavourTextGenerated += AddFlavorTextToList;
        onFlavorTextSelected += SetCardInfoFlavorText;

    }


    private void OnGUI()
    {
        tabs = GUILayout.Toolbar(tabs, tabSelection);
        SpaceV();


        switch (tabs)
        {
            case 0:
                ImageGeneratorTab();
                break;
            case 1:
                APIKeyField();
                break;
            case 2:
                ChatTestField();
                AI_TemperatureSlider();
                CardInfoField();
                TweakFlavorTextPromptField();
                FlavorTextStyleField();
                RemoveAllFlavorTextsButton();
                FlavorTextField();
                SelectFlavorTextField();
                AllFlavorTextsField();
                break;

        }

    }

    public OpenAIAPI GetAPI()
    {
        if (openAIAPI == null) return null;
        return openAIAPI;
    }
    private void Init2DTextures()
    {
        // Load the eye icon texture
        revealPassWordIcon = Resources.Load<Texture2D>("reveal_password_Icon_white");
    }

    private void ImageGeneratorTab()
    {
        //AuthenticateButton();
        ImagePromptField();
        GameObjectToApplyImageOn();
        //RawImageTest();
        GeneratedURLField();
        GenerateImageButton();
        ApplyImageToGameObjectButton();
        //SaveMaterialButton();
    }
    private async void Authenticate()
    {
        // openAIAPI = new OpenAIAPI("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");
        openAIAPI = new OpenAIAPI(API_OpenAI_Authentication.OPENAI_API_KEY);
        Debug.Log("Authenticate called");

        try
        {
            bool isValidKey = await openAIAPI.Auth.ValidateAPIKey();
            if (isValidKey)
            {
                Debug.Log("Authentication to OpenAI successful");
                tabs = 0;
            }
            else
            {
                Debug.LogError("Invalid API key provided");
                FailedAuthentication_EditorWindow.ShowWindow();
                // OpenAI_Login_Pop_Up_EditorWindow.ShowWindow();
                tabs = 1;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to authenticate to OpenAI: " + ex.Message);
            FailedAuthentication_EditorWindow.ShowWindow();

        }
    }
    private async void Authenticate(string _apiKey)
    {
        openAIAPI = new OpenAIAPI(_apiKey);
        Debug.Log("Authenticate with event called");

        try
        {
            bool isValidKey = await openAIAPI.Auth.ValidateAPIKey();
            if (isValidKey)
            {
                Debug.Log("Authentication to OpenAI successful");
                tabs = 0;
                Repaint();
            }
            else
            {
                Debug.Log("Invalid API key provided");
                // OpenAI_Login_Pop_Up_EditorWindow.ShowWindow();
                tabs = 1;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to authenticate to OpenAI: " + ex.Message);
        }
    }

    private void APIKeyField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("OpenAI API Key :");
        //API_KEY = GUILayout.TextField(API_KEY, 400);
        EditorGUI.BeginChangeCheck();
        if (!canRevealPassword)
        {
            tempKey = GUILayout.PasswordField(API_KEY, '*');
        }
        else tempKey = GUILayout.TextField(API_KEY);

        if (EditorGUI.EndChangeCheck())
        {
            API_KEY = tempKey;
        }
        if (Event.current != null && Event.current.isKey)
        {
            if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
            {
                onPasswordEntered?.Invoke(tempKey);
                Event.current.Use(); // Consume the event to prevent other actions
            }
        }
        RevealPasswordButton();
        AuthenticateButton();

        GUILayout.EndHorizontal();
    }


    private void RevealPasswordButton()
    {
        GUILayout.BeginHorizontal();
        GUIStyle _style = new GUIStyle(GUI.skin.button);
        _style.padding = new RectOffset(1, 1, 1, 1); // Adjust padding to make the button smaller
        _style.fixedWidth = 15; // Set a fixed width for the button
        _style.fixedHeight = 20; // Set a fixed height for the button
        _style.normal.background = revealPassWordIcon;
        _style.active.background = revealPassWordIcon;

        bool _revealPasswordButton = GUILayout.Button(revealPassWordIcon, _style);
        if (_revealPasswordButton)
        {
            onRevealPasswordButtonClicked?.Invoke(!canRevealPassword);
        }
        GUILayout.EndHorizontal();
    }

    private void SetCanRevealPassword(bool _value)
    {
        canRevealPassword = _value;
    }
    private void ApplyImageToGameObjectButton()
    {
        GUILayout.BeginHorizontal();
        bool _applyButton = GUILayout.Button("Apply image");
        if (_applyButton)
        {
            GetURLTexture();
        }
        GUILayout.EndHorizontal();
    }

    private void AuthenticateButton()
    {
        GUILayout.BeginHorizontal();
        bool _loginButton = GUILayout.Button("Login to OpenAI");
        if (_loginButton)
        {
            Authenticate(tempKey);
            generatedImageURL = null;
        }
        GUILayout.EndHorizontal();
    }
    private void OnURLGeneratedField()
    {

        GUILayout.BeginHorizontal();
        GUILayout.Label("Prompt");
        userInputPrompt = GUILayout.TextField(userInputPrompt, 200);
        GUILayout.EndHorizontal();
    }

    private void ImagePromptField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Prompt");
        userInputPrompt = GUILayout.TextField(userInputPrompt, 200);
        GUILayout.EndHorizontal();
    }
    private void GameObjectToApplyImageOn()
    {
        // GameObject to apply image on 
        GUILayout.BeginHorizontal();
        goImageTarget = (GameObject)EditorGUILayout.ObjectField("GameObject to place Image on", goImageTarget, typeof(GameObject), true);
        GUILayout.EndHorizontal();
    }


    private void GenerateImageButton()
    {

        GUILayout.BeginHorizontal();
        bool _generateImageButton = GUILayout.Button("Generate image");
        if (_generateImageButton)
        {
            CreateImageURL();
        }
        GUILayout.EndHorizontal();
    }

    private void SpaceV(float _value = 20)
    {
        GUILayout.BeginVertical();
        GUILayout.Space(_value);
        GUILayout.EndVertical();
    }
    private void SpaceH(float _value = 20)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(_value);
        GUILayout.BeginHorizontal();
    }
    private void GeneratedURLField()
    {


        //GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        OpenInBrowserButton();
        GUILayout.EndHorizontal();


        //GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        SpaceV(5);
        GUIStyle _labelStyle = new GUIStyle(EditorStyles.helpBox);

        GUIContent labelContent = new GUIContent("URL to Image", "URL generated by the prompt");
        Vector2 labelSize = _labelStyle.CalcSize(labelContent);

        float windowWidth = EditorGUIUtility.currentViewWidth;
        float remainingSpace = windowWidth - labelSize.x;

        GUILayout.Space(remainingSpace / 2);
        GUILayout.Label(labelContent, _labelStyle);
        GUILayout.Space(remainingSpace / 2);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (generatedImageURL != null)
            GUILayout.TextArea(generatedImageURL, 200);

        if (generatedImageURL == null)
            GUILayout.TextArea("", 200);

        //GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        SpaceV(5);


    }

    private void OpenInBrowserButton()
    {
        GUILayout.BeginHorizontal();
        bool _browserButton = GUILayout.Button("Open in browser");
        if (_browserButton)
            Application.OpenURL(generatedImageURL);
        bool _editURL = GUILayout.Button("Reset URL");
        if (_editURL)
        {
            generatedImageURL = null;
            Debug.Log("reset url");
        }
        GUILayout.EndHorizontal();
    }

    public async void CreateImageURL()
    {
        if (openAIAPI == null)
        {
            Debug.Log("Failed authentication, please login");
            FailedAuthentication_EditorWindow.ShowWindow();
            tabs = 1;
            return;
        }
        bool _checkAuth = await openAIAPI.Auth.ValidateAPIKey();
        if (_checkAuth)
        { tabs = 0; }
        else tabs = 1;
        try
        {
            Task<ImageResult> _result = openAIAPI.ImageGenerations.CreateImageAsync(userInputPrompt);  // This is using the prompt
            await _result; // Wait for the task to complete

            if (_result == null)
            {
                Debug.Log("Null result");
                return;
            }

            while (!_result.IsCompleted)
            {
                await Task.Delay(8000);
            }
            if (_result.IsCompleted)
            {

                ImageResult result = _result.Result;
                string _imageUrl = result.ToString();
                if (_imageUrl == null) return;
                Debug.Log($"Image URL: {_imageUrl}");
                generatedImageURL = _imageUrl;

                if (openImageInBrowser)  //Optional opening 
                    Application.OpenURL(generatedImageURL);
                GetURLTexture();
            }

        }

        catch (Exception e)
        {
            Debug.Log($"Failed async call to generate AI Image{e.Message}");

        }
    }


    public async void GetURLTexture()
    {
        if (generatedImageURL == null) return;
        try
        {
            UnityWebRequest _webRequest = UnityWebRequestTexture.GetTexture(generatedImageURL);
            _webRequest.SendWebRequest();

            while (!_webRequest.isDone)
            {
                await Task.Delay(8000);

            }

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Download successful");
                var _texture = DownloadHandlerTexture.GetContent(_webRequest);
                onTextureLoadedFromURL?.Invoke(_texture);
            }
            else
            {
                Debug.Log($"Failed to load web request: {_webRequest.error}");
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error loading website image: {e.Message}");
        }

    }

    private void CreateDefaultFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats"))
        {
            AssetDatabase.CreateFolder("Assets/Materials", "AI_Mats");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats/Textures"))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats", "Textures");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats/Materials"))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats", "Materials");
        }



    }
    private void SetGameObjectMaterial(Texture2D _texture)
    {

        CreateDefaultFolders();
        string _userInputNoSpace = $"{userInputPrompt.Replace(" ", "_")}";  // Replacing spaces with underscores

        string _fullPathTextures = $"Assets/Materials/AI_Mats/Textures/{_userInputNoSpace}_Textures";
        if (!AssetDatabase.IsValidFolder(_fullPathTextures))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats/Textures", $"{_userInputNoSpace}_Textures"); // Doesn't allow numbers in path!
        }

        string _fullPathMaterials = $"Assets/Materials/AI_Mats/Materials/{_userInputNoSpace}_Material";
        if (!AssetDatabase.IsValidFolder(_fullPathMaterials))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats/Materials", $"{_userInputNoSpace}_Material"); // Doesn't allow numbers in path!
        }


        string _newTexturePath = $"{_fullPathTextures}/{userInputPrompt}_Downloaded_2D_Texture.asset";

        string _newMatPath = $"{_fullPathMaterials}/{userInputPrompt}_Downloaded_Material.mat";
        _newTexturePath = AssetDatabase.GenerateUniqueAssetPath(_newTexturePath);
        _newMatPath = AssetDatabase.GenerateUniqueAssetPath(_newMatPath);

        // Save the material as an asset
        AssetDatabase.CreateAsset(_texture, _newTexturePath);   // Creates 2DTextureFile

        Material _newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        AssetDatabase.CreateAsset(_newMaterial, _newMatPath);
        goImageTarget.GetComponent<MeshRenderer>().material = _newMaterial;
        goImageTarget.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = _texture;
        material = goImageTarget.GetComponent<MeshRenderer>().sharedMaterial;
        material.mainTexture = _texture;

        AssetDatabase.SaveAssets();
    }


    /// <summary>
    /// Call this method to test the chat
    /// </summary>
    private void ChatTestField()
    {

        GUILayout.BeginHorizontal();
        bool _chatTestButton = GUILayout.Button("Chat tester");
        if (_chatTestButton)
        {
            StartChat();
            //ChatGeneratorTool_EditorWindow.StartChat(openAIAPI, ChatModel.Ada);

        }
        GUILayout.EndHorizontal();
    }
    //
    private async void StartChat()
    {
        if (openAIAPI == null)
        {
            Authenticate(API_OpenAI_Authentication.OPENAI_API_KEY);
            Debug.Log("Called authenticate from separate script");
            //tabs = 1;
            return;
        }
        Conversation _chat = openAIAPI.Chat.CreateConversation();
        _chat.Model = Model.ChatGPTTurbo;
        //_chat.RequestParameters.Temperature = 2f;  //0 --> 2


        //switch (_model)
        //{
        //    case ChatModel.Ada:
        //        _chat.Model = Model.AdaText;
        //        break;
        //    case ChatModel.AdaEmbedding:
        //        _chat.Model = Model.AdaTextEmbedding;
        //        break;
        //    case ChatModel.Babbage:
        //        _chat.Model = Model.BabbageText;
        //        break;
        //    case ChatModel.Curie:
        //        _chat.Model = Model.CurieText;
        //        break;
        //    case ChatModel.Davinci:
        //        _chat.Model = Model.DavinciText;
        //        break;
        //    case ChatModel.CushmanCode:
        //        _chat.Model = Model.CushmanCode;
        //        break;
        //    case ChatModel.DavinciCode:
        //        _chat.Model = Model.DavinciCode;
        //        break;
        //    case ChatModel.ChatGPTTurbo:
        //        _chat.Model = Model.ChatGPTTurbo;
        //        break;
        //    case ChatModel.TextModerationLatest:
        //        _chat.Model = Model.TextModerationLatest;
        //        break;
        //    default:
        //        Debug.LogError("Unsupported chat model");
        //        break;
        //}

        CardInfo _cardInfo = new CardInfo();
        /// replace the card name, type, resource type and flavor text type with variables

        _chat.AppendSystemMessage("Set up the model to generate " +
            "Magic: The Gathering-style card flavor text prompts. Include parameters for card name," +
            " type (creature/instant/ritual), associated resource type (air, fire, darkness, light, water, earth)," +
            " and the type of flavor text (creature/spell/landscape). Ensure the responses capture the essence of the card's theme and characteristics," +
            " incorporating details such as creature subtype (human/humanoid/animal/living/plant), action for spells, or description for landscapes. Do not limit yourself" +
            "to only these types of flavor texts. Give the result within quotes and NO other information, " +
            "NO flavor in the text and NOT just name of the card/type ");

        if (!cardInfo)
        {
            string _cardNameTest = "TEST";    // return string from cardInfo.cardTitle 
            string _cardTypeTest = "TEST"; // same for card type description 
            string _cardResourceTypeTest = "TEST";
            string _flavorTextTypeTest = "TEST";
            _chat.AppendUserInput($"Flavor text failed, defaulting to : {_cardNameTest},{_cardTypeTest},{_cardResourceTypeTest},{_flavorTextTypeTest}");
            return;
        }

        string _cardName = cardInfo.CardTitle;    // return string from cardInfo.cardTitle 
        string _cardType = cardInfo.CardTypeRef.ToString(); // same for card type description 
        string _cardResourceType = cardInfo.ResourceTypeRef.ToString();
        string _flavorTextType = flavorTextStyle;

        _chat.AppendUserInput($"Here are the flavor text variables : {_cardName},{_cardType},{_cardResourceType},{_flavorTextType}");
        string _response = await _chat.GetResponseFromChatbot();
        bool _responseValid = _chat.GetResponseFromChatbot().IsCompleted;
        Debug.Log(_response);

        onFlavourTextGenerated?.Invoke(_response);
    }

    private void AI_TemperatureSlider()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("AI_Temperature");
        EditorGUI.BeginChangeCheck();

        float _tempTemperature = temperature / 2;
        _tempTemperature = (float)Math.Round(_tempTemperature, 1);
        _tempTemperature = EditorGUILayout.Slider(_tempTemperature, 0, 1, GUILayout.MaxWidth(250));
        if (EditorGUI.EndChangeCheck())
        {
            temperature = _tempTemperature * 2;
            temperature = (float)Math.Round(temperature, 1);

            //Debug.Log($"Chat generating answer with temperature of {temperature}/2");
        }

        GUILayout.EndHorizontal();
    }

    private void SetTemperature(float _temperature)
    {
        temperature = Math.Clamp(temperature, 0, 2);
    }
    private void FlavorTextStyleField()
    {
        GUILayout.BeginHorizontal();

        flavorTextStyle = GUILayout.TextField(flavorTextStyle);
        GUILayout.EndHorizontal();

    }

    /// <summary>
    /// Call this method to display the flavor text prompt result
    /// </summary>
    /// <param name="_flavorTextPromptResult"></param>
    /// 
    private void FlavorTextField()
    {
        //if (currentFlavorText == string.Empty || currentFlavorText == "No flavor text generated")
        //{
        //    currentFlavorText = "No flavor text generated";
        //    GUILayout.BeginHorizontal();

        //    GUILayout.TextField(currentFlavorText);
        //    GUILayout.EndHorizontal();
        //}
        //else
        //{
        //    GUILayout.BeginHorizontal();

        //    GUILayout.TextArea(currentFlavorText);
        //    GUILayout.EndHorizontal();
        //}
    }



    private void CardInfoField()
    {
        //Card info to collect to create prompt
        SpaceV(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("CardInfo to collect for flavor text prompt");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        cardInfo = (CardInfo)EditorGUILayout.ObjectField("", cardInfo, typeof(CardInfo), true, GUILayout.MaxWidth(150));
        GUILayout.EndHorizontal();
        SpaceV(10);

    }

    private void AddFlavorTextToList(string _flavorText)
    {
        allFlavortexts.Add(_flavorText);

    }
    private void RemoveAllFlavorTexts()
    {
        allFlavortexts.Clear();
    }
    private void RemoveAllFlavorTextsButton()
    {
        SpaceV(1);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove all flavor texts", GUILayout.MaxWidth(250), GUILayout.MaxHeight(30)))
            RemoveAllFlavorTexts();
        GUILayout.EndHorizontal();
        SpaceV(1);
    }
    private void TweakFlavorTextPromptField()
    {
        SpaceV(5);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Tweak the flavor text style. Example : 'Strong creature', or 'Bloody vampire spell, or 'Vibrant medecinal jungle plant'", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        SpaceV(5);

    }
    private void AllFlavorTextsField()
    {
        foreach (string _flavorText in allFlavortexts)
        {
            SpaceV(1);
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextArea(_flavorText);
            GUILayout.EndHorizontal();
            SpaceV(1);

        }
    }

    private void SelectFlavorTextField()
    {
        SpaceV(5);
        GUILayout.BeginHorizontal();
       
        if (allFlavortexts.Count <= 0)
        {
            GUILayout.EndHorizontal();
            return;
        }
        int _currentOptionIndex = 0;
        int _size = allFlavortexts.Count;
        EditorGUI.BeginChangeCheck();
        _currentOptionIndex = EditorGUILayout.Popup("Select flavor text to use:",
            _currentOptionIndex, allFlavortexts.ToArray());
        string _selectedFlavorText = allFlavortexts[_currentOptionIndex];
        Debug.Log($"this is the current selected flavor text:{_selectedFlavorText}");
        
        if(EditorGUI.EndChangeCheck())  
            onFlavorTextSelected?.Invoke(_selectedFlavorText);

        GUILayout.EndHorizontal();
        SpaceV(5);

    }


    void SetFlavorText(string _flavorText)
    {
        currentFlavorText = _flavorText;
    }

    private void SetCardInfoFlavorText(string _selectedFlavorText)
    { 
        cardInfo.SetCardFlavorText(_selectedFlavorText);
    }
    

    void SaveMaterial()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}







//private void SaveMaterialButton()
//{
//    GUILayout.BeginHorizontal();
//    bool _loginButton = GUILayout.Button("Save material");
//    if (_loginButton)
//    {
//        SaveMaterial();
//    }
//    GUILayout.EndHorizontal();
//}
