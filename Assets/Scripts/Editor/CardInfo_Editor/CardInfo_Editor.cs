using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.TerrainTools;
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
        base.OnInspectorGUI();
    }
}

//EditorUtility.SetDirty(cardInfo);
//EditorGUILayout.Space();
//EditorApplication.QueuePlayerLoopUpdate();
//Repaint();
