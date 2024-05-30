using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI_FlavorTextGenerator_EditorWindow : EditorWindow
{
    public static bool temperatureInfoBubbleHovered = false;
    static Vector2 scrollPos = Vector2.zero;

    private static Texture2D helpToolTipIcon = null;



    private void Load2DTextures()
    {
        helpToolTipIcon = Resources.Load<Texture2D>("_Help");

    }
    public static void FlavorTextGenerationField()
    {
        AI_ModelSelector_EditorWindow.SelectChatModelField();
        ChatGenerationField();
        ModelCheckButton();
        AI_TemperatureSlider();
        CardInfoField();
        TweakFlavorTextPromptLabel();
        FlavorTextTweakField();
        RemoveAllFlavorTextsButton();
        SelectFlavorTextField();
        AllFlavorTextsField();
    }

    private static void ChatGenerationField()
    {
        GUILayout.BeginHorizontal();

        bool _generateTextButton = GUILayout.Button("Generate text");

        Conversation _conv = AI_FlavorTextGenerator.Conversation;
        if (_conv == null) StartChat();
        if (_generateTextButton)
        {
            Debug.Log("Generate text button clicked");
            _ = AI_FlavorTextGenerator.GenerateTextFlavor(_conv);
        }

        GUILayout.EndHorizontal();
    }
    private static void StartChat()
    {
        AI_Authentication.AuthenticateCall();
        Conversation _chat = AI_Authentication.OpenAIAPI.Chat.CreateConversation();
        AI_FlavorTextGenerator.Conversation = _chat;

        //Replaced invoke method with direct set of conversation.
        //Event was causing issues.
        //OnConversationStarted?.Invoke(_chat);
    }

    private static void ModelCheckButton()
    {
        GUILayout.BeginHorizontal();

        bool _checkModelButton = GUILayout.Button("Check Model");
        if (_checkModelButton)
            AI_FlavorTextGenerator.CheckModel();
        GUILayout.EndHorizontal();
    }

    private static void AI_TemperatureSlider()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Model Temperature");

        TemperatureInfoBubble();

        EditorGUI.BeginChangeCheck();

        AI_FlavorTextGenerator.Temperature = EditorGUILayout.Slider(((float)Math.Round(AI_FlavorTextGenerator.Temperature, 1)), 0, 1, GUILayout.MaxWidth(250));
        if (EditorGUI.EndChangeCheck())
        {
            AI_FlavorTextGenerator.Temperature = (float)Math.Round(AI_FlavorTextGenerator.Temperature, 1);
        }
        GUILayout.EndHorizontal();
        ModelTemperatureHelpBox(); // move in init


    }
    private static void ModelTemperatureHelpBox()
    {
        //EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300), GUILayout.Height(40));
        //GUILayout.Label("Higher values = more random, lower values" +
        //          " = more deterministic.", EditorStyles.wordWrappedLabel);
        //EditorGUILayout.EndVertical();
        ////EditorGUILayout.HelpBox("Higher values = more random, lower values" +
        ////          " = more deterministic.", MessageType.None,false);
    }

    // Not in use anymore
    private static void TemperatureInfoBubble()
    {
        Rect _infoRec = GUILayoutUtility.GetRect(20, 30);
        GUIStyle _style = new GUIStyle(GUI.skin.button);
        _style.padding = new RectOffset(2, 2, 2, 2);

        _style.fixedWidth = 16; // Smaller width
        _style.fixedHeight = 16; // Smaller height
        GUIContent _iconContent = EditorGUIUtility.IconContent("_Help@2x");
        _style.normal.background = _iconContent.image as Texture2D; ; // Should be changed for another icon
        _style.active.background = _iconContent.image as Texture2D; ; // Should be changed for another icon
        //_style.active.background = AI_Authentication_EditorWindow.RevealPassWordIcon; ; // Should be changed for another icon
        bool _imageButton = GUI.Button(_infoRec, AI_Authentication_EditorWindow.RevealPassWordIcon, _style);
        temperatureInfoBubbleHovered = _infoRec.Contains(Event.current.mousePosition);

        if (temperatureInfoBubbleHovered)
        {

            Rect _tooltipRect = new Rect(_infoRec.x, _infoRec.y, 500, 100);

            EditorGUI.LabelField(_tooltipRect, new GUIContent("", "Higher values = more random\nlower values = more deterministic"));
        }
    }

    private static void CardInfoField()
    {
        //Card info to collect to create prompt
        GUIHelpers.SpaceV(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Card Scriptable Object to collect for flavor text prompt");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        //CardInfo cardInfo = AI_FlavorTextGenerator.CardInfo;


        AI_FlavorTextGenerator.CardInfo = (CardInfo)EditorGUILayout.ObjectField("",
            AI_FlavorTextGenerator.CardInfo, typeof(CardInfo), true, GUILayout.MaxWidth(150));
        if (EditorGUI.EndChangeCheck())
        {
            TextFileCreator_Editor.CreateFolder(AI_FlavorTextGenerator.CardInfo.CardTitle);
            Debug.Log($"current card info title = {AI_FlavorTextGenerator.CardInfo.CardTitle}");
        }
        GUILayout.EndHorizontal();
        GUIHelpers.SpaceV(10);
    }
    private static void TweakFlavorTextPromptLabel()
    {
        GUIHelpers.SpaceV(5);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Tweak the flavor text style. Example : 'Strong creature', or 'Bloody vampire spell, or 'Vibrant medecinal jungle plant'", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUIHelpers.SpaceV(5);

    }
    private static void FlavorTextTweakField()
    {
        GUILayout.BeginHorizontal();
        AI_FlavorTextGenerator.FlavorTextStyle = GUILayout.TextField(AI_FlavorTextGenerator.FlavorTextStyle);
        GUILayout.EndHorizontal();

    }
    private static void RemoveAllFlavorTextsButton()
    {
        GUIHelpers.SpaceV(1);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove all flavor texts", GUILayout.MaxWidth(250), GUILayout.MaxHeight(30)))
            AI_FlavorTextGenerator.RemoveAllFlavorTexts();
        GUILayout.EndHorizontal();
        GUIHelpers.SpaceV(1);
    }
    private static void AllFlavorTextsField()
    {
        scrollPos =
        EditorGUILayout.BeginScrollView(scrollPos);
        List<string> allFlavorTexts = AI_FlavorTextGenerator.AllFlavortexts;
        for (int i = 1; i < allFlavorTexts.Count; i++)
        {
            GUIHelpers.SpaceV(1);
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextArea(allFlavorTexts[i]);
            GUILayout.EndHorizontal();
            GUIHelpers.SpaceV(1);
        }
        EditorGUILayout.EndScrollView();
    }
    private static void SelectFlavorTextField()
    {
        GUIHelpers.SpaceV(5);
        GUILayout.BeginHorizontal();
        List<string> allFlavorTexts = AI_FlavorTextGenerator.AllFlavortexts;

        if (allFlavorTexts.Count <= 0)
        {
            allFlavorTexts.Add("Select a flavor text");
            GUILayout.EndHorizontal();
            return;
        }
        int _currentOptionIndex = 0;
        int _size = allFlavorTexts.Count;
        EditorGUI.BeginChangeCheck();

        _currentOptionIndex = EditorGUILayout.Popup("",
            _currentOptionIndex, allFlavorTexts.ToArray());
        string _selectedFlavorText = allFlavorTexts[_currentOptionIndex];

        if (EditorGUI.EndChangeCheck())
        {
            AI_FlavorTextGenerator.onFlavorTextSelected?.Invoke(_selectedFlavorText);
        }

        GUILayout.EndHorizontal();
        GUIHelpers.SpaceV(5);
    }

}
