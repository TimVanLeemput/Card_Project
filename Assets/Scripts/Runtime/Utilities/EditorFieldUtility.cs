using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class EditorFieldUtility
{
    private static Action onButtonClick = null;

    #region Accesors
    public static Action OnButtonClick
    {
        get { return onButtonClick; }
        set { onButtonClick = value; }
    }
    #endregion
    public static bool ButtonField(string _buttonName)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_buttonName))
        {
            EditorGUI.FocusTextInControl("");
            onButtonClick?.Invoke();
            return true;
        }
        GUILayout.EndHorizontal();
        return false;
    }
}
