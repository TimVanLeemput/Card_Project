using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Images;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public class AI_Card_Tool_EditorWindow : EditorWindow
{
    [SerializeField] public static OpenAIAPI openAIAPI = null;

    //[SerializeField] GameObject goImageTarget = null; // TO DELETE
    [SerializeField] Material material = null;
    [SerializeField] Shader shader = null;

    [SerializeField] CardInfo cardInfo = null;
    [SerializeField] CardCreator cardCreator = null;

    //Not-used
    //ImageGenerationRequest imageGeneration = null;
    //ImageGenerationEndpoint imageGenerationEndpoint = null;
    //ImageResult imageResult = null;
    //RawImage rawImage = null;

    #region Booleans
    public bool urlHasBeenGenerated = false;
    public bool openImageInBrowser = false; // TO DELETE
    public bool canRevealPassword = false;

    public bool temperatureInfoBubbleHovered = false;
    #endregion


    #region Events
    public event Action<Texture2D> onTextureLoadedFromURL = null; // TO DELETE
    public event Action<Sprite> onSpriteGenerated = null;
    public event Action<string> onPasswordEntered = null;
    public event Action<bool> onRevealPasswordButtonClicked = null;
    // Chat events
    public event Action<string> onFlavourTextGenerated = null;
    // Action delegate to be able to call it inside the CardInfo SO
    public Action<string> onFlavorTextSelected = null;
    public event Action<Texture2D,string, string> OnAIMatAndTexturePathsCreated = null; //AI Generated texture, texturePath,matPath
    #endregion

    #region EditorWindow_UI
    //strings
    public string userInputPrompt = ""; // TO DELETE
    public string generatedImageURL = ""; // TO DELETE
    public string manualURL = "";
    public string API_KEY = "";
    public string tempKey = "";
    //2D icons
    private Texture2D revealPassWordIcon = null;
    private Texture2D mouseCursorQuestion = null;
    private Texture2D white_bg_icon = null;

    //Tabs
    public int tabs = 3;
    string[] tabSelection = new string[] { "Image Generation", "Credentials", "Flavor Text Generation" };
    #endregion

    #region AI_Properties
    // AI Properties
    [UnityEngine.Range(0.4f, 1.6f)] public float temperature = 0.8f;
    public string flavorTextStyle = "";
    public string currentFlavorText = "";
    List<string> allFlavortexts = null;

    public Conversation conversation = null; // TO DO check if this is actually needed. The
                                             // accessor for sure is used in the AI_ModelSelector_EditorWindow

    #endregion

    #region AI_Generated
    //private Sprite generatedSprite = null;
    #endregion

    // Accessors
    public static OpenAIAPI OpenAIAPI { get { return openAIAPI; } }
    public float Temperature => temperature;

    public event Action<Conversation> OnConversationStarted = null;
    public Conversation Conversation
    {
        get { return conversation; }
        set { conversation = value; }
    }

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
        onPasswordEntered += Authenticate;
        AI_ImageGenerator_EditorWindow.onTextureLoadedFromURL += CreateAIMaterialsFolders;
        onFlavourTextGenerated += SetFlavorText;
        onFlavourTextGenerated += AddFlavorTextToList;
        onFlavourTextGenerated += AddTextToFile;
        onFlavorTextSelected += SetCardInfoFlavorText;

        OnConversationStarted += SetConversation;
        OnAIMatAndTexturePathsCreated += SetGameObjectMaterial ;
        //OnConversationSet += GenerateTextFlavor;

    }


    private void SetConversation(Conversation _conversation)
    {
        conversation = _conversation;
        Debug.Log("Conversation has been set, possible to edit Model from now on");
        //OnConversationSet(Task<conversation>);
    }

    private void OnGUI()
    {
        tabs = GUILayout.Toolbar(tabs, tabSelection);
        GUIHelpers_Editor.SpaceV();
        //Generating cardinfo folder
        if (cardInfo)
        {
            TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);
        }

        switch (tabs)
        {
            case 0:
                ImageGeneratorTab();
                break;
            case 1:
                CredentialsTab();
                break;
            case 2:
                FlavorTextGeneratorTab();
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
        mouseCursorQuestion = Resources.Load<Texture2D>("mouse_cursor_question_small");
        mouseCursorQuestion = Resources.Load<Texture2D>("square_bg");
    }
    private void ImageGeneratorTab()
    {
        Authenticate(API_OpenAI_Authentication.GetApiKey());
        AI_ImageGenerator_EditorWindow.ImageGeneratorField();
        
    }
    private void CredentialsTab()
    {
        Authenticate(API_OpenAI_Authentication.GetApiKey());
        APIKeyField();
    }
    private void FlavorTextGeneratorTab()
    {
        Authenticate(API_OpenAI_Authentication.GetApiKey());
        AI_ModelSelector_EditorWindow.SelectChatModelField();
        AI_ModelSelector_Editor.Init(this);
        ChatTestField();
        ModelCheckButton();
        AI_TemperatureSlider();
        CardInfoField();
        TweakFlavorTextPromptField();
        FlavorTextStyleField();
        RemoveAllFlavorTextsButton();
        SelectFlavorTextField();
        AllFlavorTextsField();
    }
    // Currently not in use 
    private async void Authenticate()
    {

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
        if (openAIAPI != null) return;
        openAIAPI = new OpenAIAPI(_apiKey);

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

    //private void CreateDefaultFolders()
    //{
    //    if (!AssetDatabase.IsValidFolder("Assets/Materials"))
    //    {
    //        AssetDatabase.CreateFolder("Assets", "Materials");
    //    }

    //    if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats"))
    //    {
    //        AssetDatabase.CreateFolder("Assets/Materials", "AI_Mats");
    //    }

    //    if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats/Textures"))
    //    {
    //        AssetDatabase.CreateFolder("Assets/Materials/AI_Mats", "Textures");
    //    }

    //    if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats/Materials"))
    //    {
    //        AssetDatabase.CreateFolder("Assets/Materials/AI_Mats", "Materials");
    //    }
    //}
    /// <summary>
    /// This method generates 2 paths : one for the texture and for the material
    /// Could possible be transformed in a method with two out strings : 
    /// void func(Texture2D _tex, out string _texPath, out string _matPath)
    /// </summary>
    /// <param name="_texture"> The AI generated texture</param>
    
    private void CreateAIMaterialsFolders(Texture2D _AI_GeneratedTexture)
    {
        DefaultAIFolderCreator_Editor.CreateDefaultAIMatsFolders(); // Ensures the creation of the default root folders for materials, textures and AI_Mats
        Texture2D _generatedTexture = _AI_GeneratedTexture;
        string _userInputNoSpace = $"{AI_ImageGenerator_EditorWindow.userInputPrompt.Replace(" ", "_")}";  // Replacing spaces with underscores
        _userInputNoSpace = _userInputNoSpace.Replace("\"", "");  // Replacing double quotes with nothing
        _userInputNoSpace = _userInputNoSpace.Replace("\'", ""); // Replacing single quotes with nothing
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

        string _newTexturePath = $"{_fullPathTextures}/{AI_ImageGenerator_EditorWindow.userInputPrompt}_Downloaded_2D_Texture.asset";

        string _newMatPath = $"{_fullPathMaterials}/{AI_ImageGenerator_EditorWindow.userInputPrompt}_Downloaded_Material.mat";
        _newTexturePath = AssetDatabase.GenerateUniqueAssetPath(_newTexturePath);
        _newMatPath = AssetDatabase.GenerateUniqueAssetPath(_newMatPath);
        OnAIMatAndTexturePathsCreated?.Invoke(_generatedTexture,_newTexturePath, _newMatPath);
    }
    /// <summary>
    /// This method  
    /// </summary>
    /// <param name="_texture"></param>
    /// <param name="_texturePath"></param>
    /// <param name="_matPath"></param>
    private void SetGameObjectMaterial(Texture2D _texture, string _texturePath, string _matPath)
    {
        AssetDatabase.CreateAsset(_texture, _texturePath);   // Creates 2DTextureFile

        Material _newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        AssetDatabase.CreateAsset(_newMaterial, _matPath);
        AI_ImageGenerator_EditorWindow.goImageTarget.GetComponent<MeshRenderer>().material = _newMaterial;
        AI_ImageGenerator_EditorWindow.goImageTarget.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = _texture;
        material = AI_ImageGenerator_EditorWindow.goImageTarget.GetComponent<MeshRenderer>().sharedMaterial;
        material.mainTexture = _texture;

        AssetDatabase.SaveAssets();
    }
    /// <summary>
    /// Call this method to test the chat
    /// </summary>
    private void ChatTestField()
    {
        GUILayout.BeginHorizontal();
        bool _generateTextButton = GUILayout.Button("Generate text");
        AI_ModelSelector_Editor.ChatModelSelection();
        //Debug.Log($"Current conversation model: {conversation?.Model?.ModelID}");
        if (conversation == null)
            StartChat();
        if (_generateTextButton)
        {
            Debug.Log("Generate text button clicked");
            _ = GenerateTextFlavor(conversation);
        }
        GUILayout.EndHorizontal();
    }
    private void ModelCheckButton()
    {
        GUILayout.BeginHorizontal();

        bool _checkModelButton = GUILayout.Button("Check Model");
        if (_checkModelButton)
            CheckModel();
        GUILayout.EndHorizontal();
    }

    private void CheckModel()
    {
        Debug.Log($"current model is {conversation.Model.ModelID}");
    }

    private void StartChat()
    {
        if (openAIAPI == null)
        {
            Authenticate(API_OpenAI_Authentication.GetApiKey());
            Debug.Log("Called authenticate from separate script");
            //tabs = 1;
            return;
        }
        Conversation _chat = openAIAPI.Chat.CreateConversation();
        conversation = _chat;
        //Debug.Log($"Started new chat. Conversation model: {conversation?.Model?.ModelID}");
        //if(conversation == null)
        //OnConversationStarted?.Invoke(_chat);
    }

    private async Task GenerateTextFlavor(Conversation _chat)
    {
        CardInfo _cardInfo = new CardInfo();
        _chat.Model = conversation.Model;
        ChatRequest _chatRequest = new ChatRequest();
        _chatRequest.Temperature = temperature;
        float _tempTemp = (float)_chatRequest.Temperature.Value;
        Debug.Log($"Chat generating answer with temperature of {_tempTemp/2}");

        // Replace the card name, type, resource type and flavor text type with variables
        // Adapt this string to setup the chat assistant responses. 
        // Following is the base model in case you erased it and lost it.
        #region BaseChatSetup
        //"Set up the model to generate " +
        //"Magic: The Gathering-style card flavor text prompts. Include parameters for card name," +
        //" type (creature/instant/ritual), associated resource type (air, fire, darkness, light, water, earth)," +
        //" and the type of flavor text (creature/spell/landscape). Ensure the responses capture the essence of the card's theme and characteristics," +
        //" incorporating details such as creature subtype (human/humanoid/animal/living/plant), action for spells, or description for landscapes. Do not limit yourself" +
        //"to only these types of flavor texts. Give the result within quotes and NO other information, " +
        //"NO flavor in the text and NOT just name of the card/type "

        #endregion
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
        GUILayout.FlexibleSpace();
        GUILayout.Label("Model Temperature");
        TemperatureInfoBubble();

        EditorGUI.BeginChangeCheck();

        float _tempTemperature = temperature / 2;
        _tempTemperature = (float)Math.Round(_tempTemperature, 1);
        _tempTemperature = EditorGUILayout.Slider(_tempTemperature, 0, 1, GUILayout.MaxWidth(250));
        if (EditorGUI.EndChangeCheck())
        {
            temperature = _tempTemperature * 2;
            temperature = (float)Math.Round(temperature, 1);
        }
        GUILayout.EndHorizontal();
    }
    
    private void TemperatureInfoBubble()
    {
        Rect _infoRec = GUILayoutUtility.GetRect(20, 30);
        GUIStyle _style = new GUIStyle(GUI.skin.button);
        _style.padding = new RectOffset(1, 1, 1, 1); // Adjust padding to make the button smaller
        _style.fixedWidth = 20; // Set a fixed width for the button
        _style.fixedHeight = 30; // Set a fixed height for the button
        _style.normal.background = revealPassWordIcon;
        _style.active.background = revealPassWordIcon;

        bool _imageButton = GUI.Button(_infoRec, revealPassWordIcon, _style);
        temperatureInfoBubbleHovered = _infoRec.Contains(Event.current.mousePosition);

        if (temperatureInfoBubbleHovered)
        {

            Rect tooltipRect = new Rect(_infoRec.x, _infoRec.yMax, _infoRec.width * 20, 100);
            EditorGUI.LabelField(tooltipRect, new GUIContent("Higher values = \n more random, \n lower values" +
                " =\n more deterministic.", "This is the tooltip part"));

        }
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
    private void CardInfoField()
    {
        //Card info to collect to create prompt
        GUIHelpers_Editor.SpaceV(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Card Scriptable Object to collect for flavor text prompt");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        cardInfo = (CardInfo)EditorGUILayout.ObjectField("", cardInfo, typeof(CardInfo), true, GUILayout.MaxWidth(150));
        if (EditorGUI.EndChangeCheck())
        {
            TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);
            Debug.Log($"current card info title = {cardInfo.CardTitle}");
        }
        GUILayout.EndHorizontal();
        GUIHelpers_Editor.SpaceV(10);
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
        GUIHelpers_Editor.SpaceV(1);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove all flavor texts", GUILayout.MaxWidth(250), GUILayout.MaxHeight(30)))
            RemoveAllFlavorTexts();
        GUILayout.EndHorizontal();
        GUIHelpers_Editor.SpaceV(1);
    }
    private void TweakFlavorTextPromptField()
    {
        GUIHelpers_Editor.SpaceV(5);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Tweak the flavor text style. Example : 'Strong creature', or 'Bloody vampire spell, or 'Vibrant medecinal jungle plant'", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUIHelpers_Editor.SpaceV(5);

    }
    private void AllFlavorTextsField()
    {
        for (int i = 1; i < allFlavortexts.Count; i++)
        {
            GUIHelpers_Editor.SpaceV(1);
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextArea(allFlavortexts[i]);
            GUILayout.EndHorizontal();
            GUIHelpers_Editor.SpaceV(1);
        }
    }
    private void SelectFlavorTextField()
    {
        GUIHelpers_Editor.SpaceV(5);
        GUILayout.BeginHorizontal();

        if (allFlavortexts.Count <= 0)
        {
            allFlavortexts.Add("Select a flavor text");
            GUILayout.EndHorizontal();
            return;
        }
        int _currentOptionIndex = 0;
        int _size = allFlavortexts.Count;
        EditorGUI.BeginChangeCheck();

        _currentOptionIndex = EditorGUILayout.Popup("",
            _currentOptionIndex, allFlavortexts.ToArray());
        string _selectedFlavorText = allFlavortexts[_currentOptionIndex];
        // if currentOptionIndex = 0 -> Invoke()? 
        // on click on first option -> invoke ?
        if (EditorGUI.EndChangeCheck())
        {
            onFlavorTextSelected?.Invoke(_selectedFlavorText);
        }

        GUILayout.EndHorizontal();
        GUIHelpers_Editor.SpaceV(5);
    }
    void SetFlavorText(string _flavorText)
    {
        currentFlavorText = _flavorText;
    }
    private void SetCardInfoFlavorText(string _selectedFlavorText)
    {
        cardInfo.SetCardFlavorText(_selectedFlavorText);
    }
    private void AddTextToFile(string _textToAdd)
    {
        //await AsyncCreateFolderCall();
        TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);

        TextFileCreator_Editor.AddToTextFile($"{_textToAdd}", $"{cardInfo.CardTitle}");
    }
}
