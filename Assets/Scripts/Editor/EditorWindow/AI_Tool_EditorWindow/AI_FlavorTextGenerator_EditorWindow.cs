using OpenAI_API.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI_FlavorTextGenerator_EditorWindow : EditorWindow
{
    public static bool temperatureInfoBubbleHovered = false;
    public static CardInfo currentCardInfo = null;

    public static void FlavorTextGenerationField()
    {
        ChatGenerationField();
        ModelCheckButton();
        AI_TemperatureSlider();
        CardInfoField();
        TweakFlavorTextPromptField();
        FlavorTextStyleField();
        RemoveAllFlavorTextsButton();
        SelectFlavorTextField();
        AllFlavorTextsField();
    }

    private static void ChatGenerationField()
    {
        GUILayout.BeginHorizontal();

        bool _generateTextButton = GUILayout.Button("Generate text");
        AI_ModelSelector_Editor.ChatModelSelection();

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
        float temperature = AI_FlavorTextGenerator.Temperature;

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

    private static void TemperatureInfoBubble()
    {
        Rect _infoRec = GUILayoutUtility.GetRect(20, 30);
        GUIStyle _style = new GUIStyle(GUI.skin.button);
        _style.padding = new RectOffset(1, 1, 1, 1); // Adjust padding to make the button smaller
        _style.fixedWidth = 20; // Set a fixed width for the button
        _style.fixedHeight = 30; // Set a fixed height for the button
        _style.normal.background = AI_Authentication_EditorWindow.RevealPassWordIcon;
        _style.active.background = AI_Authentication_EditorWindow.RevealPassWordIcon; ;

        bool _imageButton = GUI.Button(_infoRec, AI_Authentication_EditorWindow.RevealPassWordIcon, _style);
        temperatureInfoBubbleHovered = _infoRec.Contains(Event.current.mousePosition);

        if (temperatureInfoBubbleHovered)
        {

            Rect tooltipRect = new Rect(_infoRec.x, _infoRec.yMax, _infoRec.width * 20, 100);
            EditorGUI.LabelField(tooltipRect, new GUIContent("Higher values = \n more random, \n lower values" +
                " =\n more deterministic.", "This is the tooltip part"));

        }
    }

    private static void CardInfoField()
    {
        //Card info to collect to create prompt
        GUIHelpers_Editor.SpaceV(10);
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
        GUIHelpers_Editor.SpaceV(10);
    }
    private static void TweakFlavorTextPromptField()
    {
        GUIHelpers_Editor.SpaceV(5);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Tweak the flavor text style. Example : 'Strong creature', or 'Bloody vampire spell, or 'Vibrant medecinal jungle plant'", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUIHelpers_Editor.SpaceV(5);

    }
    private static void FlavorTextStyleField()
    {
        GUILayout.BeginHorizontal();
        string flavorTextStyle = AI_FlavorTextGenerator.FlavorTextStyle;
        flavorTextStyle = GUILayout.TextField(flavorTextStyle);
        GUILayout.EndHorizontal();

    }
    private static void RemoveAllFlavorTextsButton()
    {
        GUIHelpers_Editor.SpaceV(1);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove all flavor texts", GUILayout.MaxWidth(250), GUILayout.MaxHeight(30)))
            AI_FlavorTextGenerator.RemoveAllFlavorTexts();
        GUILayout.EndHorizontal();
        GUIHelpers_Editor.SpaceV(1);
    }
    private static void AllFlavorTextsField()
    {
        List<string> allFlavorTexts = AI_FlavorTextGenerator.AllFlavortexts;
        for (int i = 1; i < allFlavorTexts.Count; i++)
        {
            GUIHelpers_Editor.SpaceV(1);
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextArea(allFlavorTexts[i]);
            GUILayout.EndHorizontal();
            GUIHelpers_Editor.SpaceV(1);
        }
    }
    private static void SelectFlavorTextField()
    {
        GUIHelpers_Editor.SpaceV(5);
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
        GUIHelpers_Editor.SpaceV(5);
    }
    
}
