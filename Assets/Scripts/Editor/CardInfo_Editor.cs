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
        

    }
    public override void OnInspectorGUI()
    {
        cardInfo = (CardInfo)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Card Name"))
        { 
            cardInfo.GenerateCardName();
            //EditorUtility.SetDirty(cardInfo);
            //EditorGUILayout.Space();
            //EditorApplication.QueuePlayerLoopUpdate();
            //Repaint();
            EditorGUI.FocusTextInControl("");

        }

        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}
