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
    //[SerializeField] static CardInfo  cardInfo = null; // to delete
    [SerializeField] Material material = null;
    static AI_Card_Tool_EditorWindow  toolInstance = null;
    #region Booleans
    //public bool canRevealPassword = false; // to delete
    //public bool temperatureInfoBubbleHovered = false; to delete
    #endregion


    #region Events
    //public event Action<string> onPasswordEntered = null; to delete
    //public event Action<bool> onRevealPasswordButtonClicked = null; to delete
    // Chat events
    //public event Action<string> onFlavourTextGenerated = null; to delete
    // Action delegate to be able to call it inside the CardInfo SO
    //public Action<string> onFlavorTextSelected = null; to delete
    #endregion

    #region EditorWindow_UI
    //strings
    public string manualURL = "";
    //public string API_KEY = ""; To delete
    //public string tempKey = ""; to delete
    //2D icons
    //private Texture2D revealPassWordIcon = null; to delete

    //Tabs
    public static int tabs = 3;
    string[] tabSelection = new string[] { "Image Generation", "Credentials", "Flavor Text Generation" };
    #endregion

    #region AI_Properties
    // AI Properties
    //[UnityEngine.Range(0.4f, 1.6f)] public float temperature = 0.8f; // to delete
    //public string flavorTextStyle = ""; to delete
    //public string currentFlavorText = ""; to delete
    //List<string> allFlavortexts = null; to delete 
    //to delete
    //public Conversation conversation = null; // TO DO check if this is actually needed. The
                                             // accessor for sure is used in the AI_ModelSelector_EditorWindow

    #endregion

    #region AI_Generated
    //private Sprite generatedSprite = null;
    #endregion

    // Accessors
    //public float Temperature => temperature; // to delete

    //public event Action<Conversation> OnConversationStarted = null;
    #region Accessors
    public static int Tabs
    {
        get { return tabs; }
        set { tabs = value; }
    }
    public static OpenAIAPI OpenAIAPI
    {
        get { return openAIAPI; }
        set { openAIAPI = value; }
    }

    //to delete
    //public static CardInfo CardInfo
    //{
    //    get { return cardInfo; }
    //    set { cardInfo = value; }
    //}
   //to delete
    //public Conversation Conversation
    //{
    //    get { return conversation; }
    //    set { conversation = value; }
    //}
    #endregion

    [MenuItem("Tools/AI Helper")]
    public static void ShowWindow()
    {
        //GetWindow<ImageGeneratorTool_EditorWindow>("AI Image Generator").Show();
        AI_Card_Tool_EditorWindow t = GetWindow<AI_Card_Tool_EditorWindow>(typeof(AI_Card_Tool_EditorWindow));
        if (toolInstance != null) return;
        toolInstance = t;
    }
    private void OnEnable()
    {
        Init();
        //allFlavortexts = new List<string>(); to delete
        AI_Authentication_EditorWindow.InitAIAuthEvents();
        //to delete
        //AI_Authentication_EditorWindow.onRevealPasswordButtonClicked += SetCanRevealPassword;
        //Init2DTextures(); to delete
    }
    private void Init()
    {
        //InitTemperature(); to delete
        InitEvents();
        AI_FlavorTextGenerator.Init();

    }
    //to delete
    //private void InitTemperature()
    //{
    //    bool _hasSetTemperature = false;
    //    if (!_hasSetTemperature)
    //        SetTemperature(0.5f);
    //    _hasSetTemperature = true;
    //}
    private void InitEvents()
    {
        InitAuthEvents();
        InitFlavorTextEvents();
        AI_ImageGenerator_EditorWindow.onTextureLoadedFromURL += AIFolderCreator_Editor.CreateAIMaterialsFolders;
        AIFolderCreator_Editor.OnAIMatAndTexturePathsCreated += SetGameObjectMaterial;
    }
    private void InitFlavorTextEvents()
    {
        //to delete
        //onFlavourTextGenerated += SetFlavorText;
        //onFlavourTextGenerated += AddFlavorTextToList;
        //onFlavourTextGenerated += AddTextToFile;
        //onFlavorTextSelected += SetCardInfoFlavorText;
    }
    private void InitAuthEvents()
    {
        AI_Authentication_EditorWindow.onPasswordEntered += AI_Authentication.AuthenticateCall;
    }

    public static AI_Card_Tool_EditorWindow GetTool()
    {
        if(toolInstance != null) return toolInstance;
        else return null;
    }
    //to delete
    //private void Init2DTextures()
    //{
    //    // Load the eye icon texture
    //    revealPassWordIcon = Resources.Load<Texture2D>("reveal_password_Icon_white");
    //}
    private void OnGUI()
    {
        tabs = GUILayout.Toolbar(tabs, tabSelection);
        GUIHelpers_Editor.SpaceV();
        //to delete and replace 
        //if (cardInfo)
        //{
        //    TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);
        //}

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
    private void ImageGeneratorTab()
    {
        AI_Authentication.AuthenticateCall();
        //Authenticate(API_OpenAI_Authentication.GetApiKey());
        AI_ImageGenerator_EditorWindow.ImageGeneratorField();
    }
    private void CredentialsTab()
    {
        AI_Authentication.AuthenticateCall();
        AI_Authentication_EditorWindow.APIKeyField();
    }
    private void FlavorTextGeneratorTab()
    {
        AI_ModelSelector_Editor.Init(this); // TO DO might need to be replaced after SelectChatModelField
        AI_Authentication.AuthenticateCall();
        //Authenticate(API_OpenAI_Authentication.GetApiKey());
        AI_ModelSelector_EditorWindow.SelectChatModelField();
        AI_FlavorTextGenerator_EditorWindow.FlavorTextGenerationField();
        //ChatGenerationField(); to delete
        //ModelCheckButton();
        //AI_TemperatureSlider();
        //CardInfoField();
        //TweakFlavorTextPromptField();
        //FlavorTextStyleField();
        //RemoveAllFlavorTextsButton();
        //SelectFlavorTextField();
        //AllFlavorTextsField();
    }
    //to delete
    //public async void AuthenticateCall()
    //{
    //    await AI_Authentication_Editor.Authenticate(API_OpenAI_Authentication.GetApiKey(), this);
    //}

    //public async void AuthenticateCall(string _tempKey)
    //{
    //    await AI_Authentication_Editor.Authenticate(_tempKey, this);
    //}

    //to delete
    //private void APIKeyField()
    //{
    //    GUILayout.BeginHorizontal();
    //    GUILayout.Label("OpenAI API Key :");
    //    EditorGUI.BeginChangeCheck();
    //    if (!canRevealPassword)
    //    {
    //        tempKey = GUILayout.PasswordField(API_KEY, '*');
    //    }
    //    else tempKey = GUILayout.TextField(API_KEY);

    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        API_KEY = tempKey;
    //    }
    //    if (Event.current != null && Event.current.isKey)
    //    {
    //        if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
    //        {
    //            Debug.Log("enter pressed");
    //            onPasswordEntered?.Invoke(tempKey);
    //            Event.current.Use(); // Consume the event to prevent other actions
    //        }
    //    }
    //    RevealPasswordButton();
    //    AuthenticateButton();

    //    GUILayout.EndHorizontal();
    //}

    //to delete
    //private void RevealPasswordButton()
    //{
    //    GUILayout.BeginHorizontal();
    //    GUIStyle _style = new GUIStyle(GUI.skin.button);
    //    _style.padding = new RectOffset(1, 1, 1, 1); // Adjust padding to make the button smaller
    //    _style.fixedWidth = 15; // Set a fixed width for the button
    //    _style.fixedHeight = 20; // Set a fixed height for the button
    //    _style.normal.background = revealPassWordIcon;
    //    _style.active.background = revealPassWordIcon;

    //    bool _revealPasswordButton = GUILayout.Button(revealPassWordIcon, _style);
    //    if (_revealPasswordButton)
    //    {
    //        onRevealPasswordButtonClicked?.Invoke(!canRevealPassword);
    //    }
    //    GUILayout.EndHorizontal();
    //}
    //to delete
    //private void SetCanRevealPassword(bool _value)
    //{
    //    canRevealPassword = _value;
    //}
    // to delete
    //private void AuthenticateButton()
    //{
    //    GUILayout.BeginHorizontal();
    //    bool _loginButton = GUILayout.Button("Login to OpenAI");
    //    if (_loginButton)
    //    {
    //        AuthenticateCall(tempKey);
    //    }
    //    GUILayout.EndHorizontal();
    //}

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
    //private void ChatGenerationField()
    //{
    //    GUILayout.BeginHorizontal();
    //    bool _generateTextButton = GUILayout.Button("Generate text");
    //    AI_ModelSelector_Editor.ChatModelSelection();
    //    if (conversation == null)
    //        StartChat();
    //    if (_generateTextButton)
    //    {
    //        Debug.Log("Generate text button clicked");
    //        _ = GenerateTextFlavor(conversation);
    //    }
    //    GUILayout.EndHorizontal();
    //}

    //to delete
    //private void ModelCheckButton()
    //{
    //    GUILayout.BeginHorizontal();

    //    bool _checkModelButton = GUILayout.Button("Check Model");
    //    if (_checkModelButton)
    //        CheckModel();
    //    GUILayout.EndHorizontal();
    //}

    //to delete
    //private void CheckModel()
    //{
    //    //Debug.Log($"current model is {conversation.Model.ModelID}");
    //}

    //to delete
    //private void StartChat()
    //{
    //    AI_Authentication.AuthenticateCall();
    //    Conversation _chat = AI_Authentication.OpenAIAPI.Chat.CreateConversation();
    //    conversation = _chat;
    //    //Replaced invoke method with direct set of conversation.
    //    //Event was causing issues.
    //    //OnConversationStarted?.Invoke(_chat);
    //}


    //to delete
    //private async Task GenerateTextFlavor(Conversation _chat)
    //{
    //    CardInfo _cardInfo = CreateInstance<CardInfo>();
    //    _chat.Model = conversation.Model;
    //    ChatRequest _chatRequest = new ChatRequest();
    //    _chatRequest.Temperature = temperature;
    //    float _tempTemp = (float)_chatRequest.Temperature.Value;
    //    Debug.Log($"Chat generating answer with temperature of {_tempTemp / 2}");

    //    // Replace the card name, type, resource type and flavor text type with variables
    //    // Adapt this string to setup the chat assistant responses. 
    //    // Following is the base model in case you erased it and lost it.
    //    #region BaseChatSetup
    //    //"Set up the model to generate " +
    //    //"Magic: The Gathering-style card flavor text prompts. Include parameters for card name," +
    //    //" type (creature/instant/ritual), associated resource type (air, fire, darkness, light, water, earth)," +
    //    //" and the type of flavor text (creature/spell/landscape). Ensure the responses capture the essence of the card's theme and characteristics," +
    //    //" incorporating details such as creature subtype (human/humanoid/animal/living/plant), action for spells, or description for landscapes. Do not limit yourself" +
    //    //"to only these types of flavor texts. Give the result within quotes and NO other information, " +
    //    //"NO flavor in the text and NOT just name of the card/type "

    //    #endregion
    //    _chat.AppendSystemMessage("Set up the model to generate " +
    //        "Magic: The Gathering-style card flavor text prompts. Include parameters for card name," +
    //        " type (creature/instant/ritual), associated resource type (air, fire, darkness, light, water, earth)," +
    //        " and the type of flavor text (creature/spell/landscape). Ensure the responses capture the essence of the card's theme and characteristics," +
    //        " incorporating details such as creature subtype (human/humanoid/animal/living/plant), action for spells, or description for landscapes. Do not limit yourself" +
    //        "to only these types of flavor texts. Give the result within quotes and NO other information, " +
    //        "NO flavor in the text and NOT just name of the card/type ");

    //    if (!cardInfo)
    //    {
    //        string _cardNameTest = "TEST";    // return string from cardInfo.cardTitle 
    //        string _cardTypeTest = "TEST"; // same for card type description 
    //        string _cardResourceTypeTest = "TEST";
    //        string _flavorTextTypeTest = "TEST";
    //        _chat.AppendUserInput($"Flavor text failed, defaulting to : {_cardNameTest},{_cardTypeTest},{_cardResourceTypeTest},{_flavorTextTypeTest}");
    //        return;
    //    }

    //string _cardName = cardInfo.CardTitle;    // return string from cardInfo.cardTitle 
    //string _cardType = cardInfo.CardTypeRef.ToString(); // same for card type description 
    //string _cardResourceType = cardInfo.ResourceTypeRef.ToString();
    //string _flavorTextType = flavorTextStyle;

    //_chat.AppendUserInput($"Here are the flavor text variables : {_cardName},{_cardType},{_cardResourceType},{_flavorTextType}");
    //    string _response = await _chat.GetResponseFromChatbot();
    //bool _responseValid = _chat.GetResponseFromChatbot().IsCompleted;
    //Debug.Log(_response);
    //    onFlavourTextGenerated?.Invoke(_response);
    //}
    //to delete
    //private void AI_TemperatureSlider()
    //{
    //    GUILayout.BeginHorizontal();
    //    GUILayout.FlexibleSpace();
    //    GUILayout.Label("Model Temperature");
    //    TemperatureInfoBubble();

    //    EditorGUI.BeginChangeCheck();

    //    float _tempTemperature = temperature / 2;
    //    _tempTemperature = (float)Math.Round(_tempTemperature, 1);
    //    _tempTemperature = EditorGUILayout.Slider(_tempTemperature, 0, 1, GUILayout.MaxWidth(250));
    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        temperature = _tempTemperature * 2;
    //        temperature = (float)Math.Round(temperature, 1);
    //    }
    //    GUILayout.EndHorizontal();
    //}

    //to delete
    //private void TemperatureInfoBubble()
    //{
    //    Rect _infoRec = GUILayoutUtility.GetRect(20, 30);
    //    GUIStyle _style = new GUIStyle(GUI.skin.button);
    //    _style.padding = new RectOffset(1, 1, 1, 1); // Adjust padding to make the button smaller
    //    _style.fixedWidth = 20; // Set a fixed width for the button
    //    _style.fixedHeight = 30; // Set a fixed height for the button
    //    _style.normal.background = AI_Authentication_EditorWindow.RevealPassWordIcon;
    //    _style.active.background = AI_Authentication_EditorWindow.RevealPassWordIcon; ;

    //    bool _imageButton = GUI.Button(_infoRec, AI_Authentication_EditorWindow.RevealPassWordIcon, _style);
    //    temperatureInfoBubbleHovered = _infoRec.Contains(Event.current.mousePosition);

    //    if (temperatureInfoBubbleHovered)
    //    {

    //        Rect tooltipRect = new Rect(_infoRec.x, _infoRec.yMax, _infoRec.width * 20, 100);
    //        EditorGUI.LabelField(tooltipRect, new GUIContent("Higher values = \n more random, \n lower values" +
    //            " =\n more deterministic.", "This is the tooltip part"));

    //    }
    //}
    //to delete
    //private void SetTemperature(float _temperature)
    //{
    //    temperature = Math.Clamp(temperature, 0, 2);
    //}

    //to delete
    //private void FlavorTextStyleField()
    //{
    //    GUILayout.BeginHorizontal();

    //    flavorTextStyle = GUILayout.TextField(flavorTextStyle);
    //    GUILayout.EndHorizontal();

    //}
    //to delete
    //private void CardInfoField()
    //{
    //    //Card info to collect to create prompt
    //    GUIHelpers_Editor.SpaceV(10);
    //    GUILayout.BeginHorizontal();
    //    GUILayout.Label("Card Scriptable Object to collect for flavor text prompt");
    //    GUILayout.EndHorizontal();
    //    GUILayout.BeginHorizontal();
    //    EditorGUI.BeginChangeCheck();
    //    cardInfo = (CardInfo)EditorGUILayout.ObjectField("", cardInfo, typeof(CardInfo), true, GUILayout.MaxWidth(150));
    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);
    //        Debug.Log($"current card info title = {cardInfo.CardTitle}");
    //    }
    //    GUILayout.EndHorizontal();
    //    GUIHelpers_Editor.SpaceV(10);
    //}
    //to delete
    //private void AddFlavorTextToList(string _flavorText)
    //{
    //    allFlavortexts.Add(_flavorText);

    //}
    //private void RemoveAllFlavorTexts()
    //{
    //    allFlavortexts.Clear();
    //}
    //private void RemoveAllFlavorTextsButton()
    //{
    //    GUIHelpers_Editor.SpaceV(1);
    //    GUILayout.BeginHorizontal();
    //    if (GUILayout.Button("Remove all flavor texts", GUILayout.MaxWidth(250), GUILayout.MaxHeight(30)))
    //        RemoveAllFlavorTexts();
    //    GUILayout.EndHorizontal();
    //    GUIHelpers_Editor.SpaceV(1);
    //}
    //to delete
    //private void TweakFlavorTextPromptField()
    //{
    //    GUIHelpers_Editor.SpaceV(5);
    //    GUILayout.BeginHorizontal();

    //    GUILayout.Label("Tweak the flavor text style. Example : 'Strong creature', or 'Bloody vampire spell, or 'Vibrant medecinal jungle plant'", EditorStyles.boldLabel);
    //    GUILayout.EndHorizontal();
    //    GUIHelpers_Editor.SpaceV(5);

    //}
    //to delete
    //private void AllFlavorTextsField()
    //{
    //    for (int i = 1; i < allFlavortexts.Count; i++)
    //    {
    //        GUIHelpers_Editor.SpaceV(1);
    //        GUILayout.BeginHorizontal();
    //        EditorGUILayout.TextArea(allFlavortexts[i]);
    //        GUILayout.EndHorizontal();
    //        GUIHelpers_Editor.SpaceV(1);
    //    }
    //}
    // to delete
    //private void SelectFlavorTextField()
    //{
    //    GUIHelpers_Editor.SpaceV(5);
    //    GUILayout.BeginHorizontal();

    //    if (allFlavortexts.Count <= 0)
    //    {
    //        allFlavortexts.Add("Select a flavor text");
    //        GUILayout.EndHorizontal();
    //        return;
    //    }
    //    int _currentOptionIndex = 0;
    //    int _size = allFlavortexts.Count;
    //    EditorGUI.BeginChangeCheck();

    //    _currentOptionIndex = EditorGUILayout.Popup("",
    //        _currentOptionIndex, allFlavortexts.ToArray());
    //    string _selectedFlavorText = allFlavortexts[_currentOptionIndex];

    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        onFlavorTextSelected?.Invoke(_selectedFlavorText);
    //    }

    //    GUILayout.EndHorizontal();
    //    GUIHelpers_Editor.SpaceV(5);
    //}
    //void SetFlavorText(string _flavorText)
    //{
    //    currentFlavorText = _flavorText;
    //}
    //to delete
    //private void SetCardInfoFlavorText(string _selectedFlavorText)
    //{
    //    cardInfo.SetCardFlavorText(_selectedFlavorText);
    //}
    //to delete
    //private void AddTextToFile(string _textToAdd)
    //{
    //    TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);
    //    TextFileCreator_Editor.AddToTextFile($"{_textToAdd}", $"{cardInfo.CardTitle}");
    //}
}
