using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GUIHelpers
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

    /// <summary>
    /// This method calls an iterator for our connected script.
    /// It also skips any individual childs within an array, 
    /// which would otherwise be shown twice on top of showing a
    /// "Size" variable for this array.
    /// </summary>
    /// <param name="_propertyName"> Variable name for anchoring the help message</param>
    /// <param name="_helpMessage"> Help message content to display in inspector</param>
    public static void InjectIterationHelpBox(SerializedObject _SO,string _propertyName, string _helpMessage)
    {
        SerializedProperty _iterator = _SO.GetIterator();
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

        _SO.ApplyModifiedProperties();
    }
}
