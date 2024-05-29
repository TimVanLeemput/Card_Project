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
    /// <summary>
    /// This method calls an iterator for our connected script.
    /// It also skips any individual childs within an array, 
    /// which would otherwise be shown twice on top of showing a
    /// "Size" variable for this array.
    /// </summary>
    /// <param name="_propertyName"></param>
    /// <param name="_helpMessage"></param>
    private void IterationHelpBox(string _propertyName, string _helpMessage)
    {
        SerializedProperty _iterator = serializedObject.GetIterator();
        bool _arrayDisplayed = false;
        bool _propertyNameEncountered = false;

        while (_iterator.NextVisible(true))
        {
            bool _isPropertyTarget = _iterator.name == _propertyName;
            if (_iterator.isArray && !_arrayDisplayed || _isPropertyTarget && !_propertyNameEncountered)
            {
                EditorGUILayout.PropertyField(_iterator, true);
                if (_iterator.isArray) _arrayDisplayed = true;
                if (_isPropertyTarget)
                {
                    EditorGUILayout.HelpBox(_helpMessage, MessageType.None, false);
                    _propertyNameEncountered = true;
                }
                continue;
            }

            if (!_arrayDisplayed || _iterator.depth == 0)
            {
                EditorGUILayout.PropertyField(_iterator, true);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }








}


