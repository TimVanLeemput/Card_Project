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
        GUIHelpers.InjectIterationHelpBox(serializedObject,
            "cardArtSprite", "Use sprite to drag and drop an external image");
    }
}


