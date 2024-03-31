using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GUIHelpers_Editor
{
    public static void SpaceV(float _value = 20)
    {
        GUILayout.BeginVertical();
        GUILayout.Space(_value);
        GUILayout.EndVertical();
    }
    public static void SpaceH(float _value = 20)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(_value);
        GUILayout.EndHorizontal();
    }
}
