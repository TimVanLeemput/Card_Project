using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardInfo))]
public class CardInfo_Editor : Editor
{
    public CardInfo cardInfo = null;
    private void OnEnable()
    {
        cardInfo = (CardInfo)target;

        cardInfo.GenerateCardName();

        EditorGUI.FocusTextInControl("");

    }
    public override void OnInspectorGUI()
    {
        cardInfo = (CardInfo)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Regenerate Card Name"))
        {
            cardInfo.GenerateCardName();

            EditorGUI.FocusTextInControl("");

        }

        GUILayout.EndHorizontal();
        //base.OnInspectorGUI();
        IterationHelpBox("cardArtSprite", "Use sprite to drag and drop an external image");
    }

    private void IterationHelpBox(string _propertyName, string _helpMessage)
    {
        SerializedProperty _iterator = serializedObject.GetIterator();
        bool arrayDisplayed = false;
        bool propertyNameEncountered = false;

        while (_iterator.NextVisible(true))
        {
            if (_iterator.isArray && !arrayDisplayed)
            {
                EditorGUILayout.PropertyField(_iterator, true);
                arrayDisplayed = true;
                continue;
            }

            // Check if the property name is encountered
            if (_iterator.name == _propertyName && !propertyNameEncountered)
            {
                EditorGUILayout.PropertyField(_iterator, true);
                EditorGUILayout.HelpBox(_helpMessage, MessageType.None, false);
                propertyNameEncountered = true;
                continue;
            }

            // Skip displaying individual elements of the array
            if (arrayDisplayed && _iterator.depth > 0)
            {
                continue;
            }

            EditorGUILayout.PropertyField(_iterator, true);
        }

        serializedObject.ApplyModifiedProperties();
    }




}


