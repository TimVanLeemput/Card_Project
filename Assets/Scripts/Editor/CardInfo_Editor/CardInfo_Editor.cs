using MyBox;
using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CardInfo))]
public class CardInfo_Editor : Editor
{
    public CardInfo cardInfo = null;

    private const string generateCardNameButtonName = "Generate Card Name";
    private const string checkResourceColor = "Check Resource Color";

    private void OnEnable()
    {
        //EditorFieldUtility.OnButtonClick += GenerateCardName;

        cardInfo = (CardInfo)target;

        GenerateCardName();

        EditorGUI.FocusTextInControl("");


    }
    public override void OnInspectorGUI()
    {
        cardInfo = (CardInfo)target;
        if (EditorFieldUtility.ButtonField(generateCardNameButtonName))
            GenerateCardName();
        
        InjectHelpBox("cardArtSprite", "Use sprite to drag and drop an external image");
    }
    private void InjectHelpBox(string _propertyName,string _helpMessage)
    {
        GUIHelpers.InjectIterationHelpBox(serializedObject,
                    _propertyName, _helpMessage);
    }

    [ContextMenu("Generate Card Name")]
    public void GenerateCardName()
    {
        string _cardName = cardInfo.CardTitle;
        Debug.Log("Generated Card name with = " + _cardName);
    }

}


