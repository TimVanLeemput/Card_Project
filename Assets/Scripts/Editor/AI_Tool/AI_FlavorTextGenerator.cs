using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AI_FlavorTextGenerator
{
    [SerializeField] static CardInfo cardInfo = null;

    public static Conversation conversation = null;

    [Range(0.4f, 1.6f)] public static float temperature = 0.8f;
    public static string flavorTextStyle = "";
    public static string currentFlavorText = "";
    static List<string> allFlavortexts = null;

    #region Events
    public static event Action<string> onFlavourTextGenerated = null;
    public static Action<string> onFlavorTextSelected = null;


    #endregion
    #region Accessors
    public static float Temperature
    {
        get { return temperature; }
        set { temperature = value; }
    }
    public static Conversation Conversation
    {
        get { return conversation; }
        set { conversation = value; }
    }
    public static CardInfo CardInfo
    {
        get { return cardInfo; }
        set { cardInfo = value; }
    }
    public static string FlavorTextStyle
    {

        get { return flavorTextStyle; }
        set { flavorTextStyle = value; }
    }
    public static string CurrentFlavorText
    {
        get { return flavorTextStyle; }
        set { flavorTextStyle = value; }

    }
    public static List<string> AllFlavortexts
    {
        get { return allFlavortexts; }
        set { allFlavortexts = value; }
    }


    #endregion
    public static void Init()
    {
        InitEvents();
        InitCardInfo();
        InitTemperature();
        InitAllFlavorTexts();
    }

    private static void InitAllFlavorTexts()
    {
        allFlavortexts = new List<string>();
    }

    private static void InitCardInfo()
    {
        if (cardInfo)
        {
            TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);
        }
    }
    private static void InitTemperature()
    {
        bool _hasSetTemperature = false;
        if (!_hasSetTemperature)
            SetTemperature(0.5f);
        _hasSetTemperature = true;
    }
    private static void InitEvents()
    {
        onFlavourTextGenerated += SetFlavorText;
        onFlavourTextGenerated += AddFlavorTextToList;
        onFlavourTextGenerated += AddTextToFile;
        onFlavorTextSelected += SetCardInfoFlavorText;
    }
    private static void SetTemperature(float _temperature)
    {
        temperature = Math.Clamp(temperature, 0, 2);
    }
    static void SetFlavorText(string _flavorText)
    {
        currentFlavorText = _flavorText;
    }
    private static void SetCardInfoFlavorText(string _selectedFlavorText)
    {
        cardInfo.SetCardFlavorText(_selectedFlavorText);
    }
    private static void AddFlavorTextToList(string _flavorText)
    {
        allFlavortexts.Add(_flavorText);

    }
    public static void RemoveAllFlavorTexts()
    {
        allFlavortexts.Clear();
    }
    private static void AddTextToFile(string _textToAdd)
    {
        TextFileCreator_Editor.CreateFolder(cardInfo.CardTitle);
        TextFileCreator_Editor.AddToTextFile($"{_textToAdd}", $"{cardInfo.CardTitle}");
    }
    public static void CheckModel()
    {
        Debug.Log($"current model is {Conversation.Model.ModelID}");
    }
    public static async Task GenerateTextFlavor(Conversation _chat)
    {
        CardInfo _cardInfo = ScriptableObject.CreateInstance<CardInfo>();
        _chat.Model = conversation.Model;
        ChatRequest _chatRequest = new ChatRequest();
        _chatRequest.Temperature = temperature;
        Debug.Log($"Chat generating answer with temperature of {(float)_chatRequest.Temperature.Value}");

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

        if (!CardInfo)
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
}
