/*
 * Copyright (c) [2024] [Tim Van Leemput]
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * - Make sure to include this script in your Unity project and use it in accordance with the MIT License terms.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// TagManager is a Unity Editor window tool that provides advanced functionality for managing tags.
/// 
/// Features:
/// - Search, filter, and apply tags to GameObjects directly within the Unity Editor.
/// - Create and add new tags seamlessly with intuitive UI controls.
/// - Customize tag colors for easy visual identification and organization.
/// - Access the tool via Unity Editor's Tools menu under 'Tag Manager'.
/// - Ideal for developers and designers needing efficient tag management for large-scale projects.
/// </summary>
public class TagManager : EditorWindow
{
    private string newTagName = "";
    private Vector2 scrollPositionTagList;
    private Vector2 scrollPositionGameObjectsWithFilteredTag;

    private string searchQuery = "";
    private List<string> filteredTags = new List<string>();
    public List<GameObject> allGameObjectsWithSpecificTag = null;
    private bool displayGameObjectsWithSpecificTagField = false;

    private float dividerPosition = 0.5f; // Initial position of the divider (0.5 = center)
    private bool isDraggingDivider = false;
    private float dividerHeight = 5f; // Height of the divider bar

    private string tagToFind = "";

    public event Action<string> OnFindGameObjectInSceneButtonPressed = null;
    private enum TagColor
    {
        LIGHT_RED,
        LIGHT_GREEN,
        LIGHT_BLUE,
        LIGHT_YELLOW,
        LIGHT_CYAN,
        LIGHT_MAGENTA,
        LIGHT_ORANGE,
        LIGHT_LIME,
        LIGHT_PURPLE,
        LIGHT_TEAL
    }

    private Color[] tagColors = {
        new Color(0.8f, 0.6f, 0.6f),   // LIGHT_RED
        new Color(0.6f, 0.8f, 0.6f),   // LIGHT_GREEN
        new Color(0.6f, 0.6f, 0.8f),   // LIGHT_BLUE
        new Color(0.8f, 0.8f, 0.6f),   // LIGHT_YELLOW
        new Color(0.6f, 0.8f, 0.8f),   // LIGHT_CYAN
        new Color(0.8f, 0.6f, 0.8f),   // LIGHT_MAGENTA
        new Color(0.9f, 0.7f, 0.5f),   // LIGHT_ORANGE
        new Color(0.7f, 0.9f, 0.5f),   // LIGHT_LIME
        new Color(0.7f, 0.5f, 0.9f),   // LIGHT_PURPLE
        new Color(0.5f, 0.9f, 0.7f)    // LIGHT_TEAL
    };

    [MenuItem("Tools/Tag Manager Tool")]
    public static void ShowWindow()
    {
        GetWindow<TagManager>("Tag Manager");
    }

    private void OnEnable()
    {
        displayGameObjectsWithSpecificTagField = false;
        OnFindGameObjectInSceneButtonPressed += SetTagForGameObjectToFind;
    }

    private void SetTagForGameObjectToFind(string _tag)
    {
        tagToFind = _tag;
    }

    private void OnGUI()
    {
        SearchTagField();
        DrawResizableScrollView();

    }
    /// <summary>
    /// Split view when showing the List of GameObjects 
    /// with a matching tag
    /// </summary>
    private void DrawResizableScrollView()
    {
        EditorGUILayout.BeginVertical();

        // Top ScrollView
        EditorGUILayout.BeginVertical(GUILayout.Height(position.height * dividerPosition));
        TopScrollViewField();
        EditorGUILayout.EndVertical();

        // Divider
        DividerMouseDragField();

        // Bottom ScrollView
        EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
        BottomScrollViewField();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }

    private void BottomScrollViewField()
    {
        AllGameObjectsWithTagListField();
        HideGameObjectWithFilteredTagListButtonField();
        CreateNewTagField();
    }

    private void TopScrollViewField()
    {
        TagsScrollViewField();
    }

    private void DividerMouseDragField()
    {
        Rect resizeRect = GUILayoutUtility.GetRect(position.width, dividerHeight);
        EditorGUI.DrawRect(resizeRect, Color.black * 0.5f);

        // Handle divider dragging
        EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeVertical);
        if (Event.current.type == EventType.MouseDown && resizeRect.Contains(Event.current.mousePosition))
        {
            isDraggingDivider = true;
        }
        if (isDraggingDivider)
        {
            if (position.height <= 450)
            {
                dividerPosition = Mathf.Clamp(Event.current.mousePosition.y / position.height - 0.15f, 0.1f, 0.9f); // 0.15f => Dynamic Offset for window resizing
            }
            else
            {
                dividerPosition = Mathf.Clamp(Event.current.mousePosition.y / position.height - 0.05f, 0.1f, 0.9f); // 0.05f => Dynamic Offset for window resizing
            }
            Repaint();
        }
        if (Event.current.type == EventType.MouseUp)
        {
            isDraggingDivider = false;
        }
    }

    private void UntaggedButtonField()
    {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Untagged", GUILayout.Width(80), GUILayout.Height(30)))
        {
            ChangeTagOfSelectedObject("Untagged");
        }
        GUILayout.FlexibleSpace();
    }

    private void SearchTagField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Search tag:", EditorStyles.boldLabel, GUILayout.Width(80));
        searchQuery = EditorGUILayout.TextField(searchQuery);
        UntaggedButtonField();
        GUILayout.EndHorizontal();
    }
    private void TagsScrollViewField()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        scrollPositionTagList = EditorGUILayout.BeginScrollView(scrollPositionTagList);

        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

        // Filter tags based on search query
        filteredTags = FilterTags(tags, searchQuery);
        filteredTags.Sort();

        foreach (string tag in filteredTags)
        {
            DisplayTagEntry(tag);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private List<string> FilterTags(string[] tags, string searchQuery)
    {
        List<string> filtered = new List<string>();

        foreach (string tag in tags)
        {
            if (string.IsNullOrEmpty(searchQuery) || tag.ToLower().Contains(searchQuery.ToLower()))
            {
                filtered.Add(tag);
            }
        }
        return filtered;
    }
    /// <summary>
    /// This method is called to display one tag
    /// as a Button field
    /// </summary>
    /// <param name="tag"></param>
    private void DisplayTagEntry(string tag)
    {
        EditorGUILayout.BeginHorizontal();

        // Display tag name with custom color as button
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        //int colorIndex = System.Array.IndexOf(UnityEditorInternal.InternalEditorUtility.tags, tag) % tagColors.Length;  // If we want to filter colors by actual TagManager indexation
        int colorIndex = filteredTags.IndexOf(tag) % tagColors.Length; // Filtering colors per filteredTag list
        buttonStyle.normal.textColor = tagColors[colorIndex];
        buttonStyle.hover.textColor = tagColors[colorIndex]; // Change text color on hover (optional)
        buttonStyle.active.textColor = tagColors[colorIndex]; // Change text color on click (optional)

        // Turn into if statement to add a functionality on click
        GUILayout.Button(tag, buttonStyle, GUILayout.ExpandWidth(true));

        //Find in scene button
        if (GUILayout.Button("Find", GUILayout.Width(60)))
        {
            OnFindGameObjectInSceneButtonPressed?.Invoke(tag);
            allGameObjectsWithSpecificTag = GetListOfGameObjectsWithSpecificTag(tagToFind);
            if (allGameObjectsWithSpecificTag.Count > 0) displayGameObjectsWithSpecificTagField = true;
        }
        // Apply button
        if (GUILayout.Button("Apply", GUILayout.Width(60)))
        {
            ChangeTagOfSelectedObject(tag);
        }

        // Copy button
        if (GUILayout.Button("Copy", GUILayout.Width(50)))
        {
            CopyToClipboard(tag);
        }

        if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"), GUILayout.Width(20)))
        {
            RemoveTagFromTagList(tag);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void RemoveTagFromTagList(string _tagToRemove)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        if (TagExists(_tagToRemove))
        {
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty tag = tagsProp.GetArrayElementAtIndex(i);
                if (tag.stringValue == _tagToRemove)
                {
                    tagsProp.DeleteArrayElementAtIndex(i);
                    tagManager.ApplyModifiedProperties();
                    Debug.Log("Tag '" + _tagToRemove + "' deleted.");
                    return; // Early return if tag found
                }
            }
        }
        else
        {
            Debug.LogWarning("Tag '" + _tagToRemove + "' does not exist.");
        }
    }

    private void AllGameObjectsWithTagListField()
    {
        if (!displayGameObjectsWithSpecificTagField) return;
   
        EditorGUILayout.BeginVertical(GUI.skin.box);
        scrollPositionGameObjectsWithFilteredTag = EditorGUILayout.BeginScrollView(scrollPositionGameObjectsWithFilteredTag);

        if (string.IsNullOrEmpty(tagToFind) || !TagExists(tagToFind))
        {
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            return;
        }

        allGameObjectsWithSpecificTag = GetListOfGameObjectsWithSpecificTag(tagToFind);

        if (allGameObjectsWithSpecificTag.Count > 0)
        {
            foreach (GameObject _go in allGameObjectsWithSpecificTag)
            {
                EditorGUILayout.ObjectField(_go.name, _go, typeof(GameObject), true);
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private static List<GameObject> GetListOfGameObjectsWithSpecificTag(string _tag)
    {
        List<GameObject> _allGOWithSpecificTag = GameObject.FindGameObjectsWithTag(_tag).ToList();
        return _allGOWithSpecificTag;
    }

    private void HideGameObjectWithFilteredTagListButtonField()
    {
        if (displayGameObjectsWithSpecificTagField)
        {
            if (GUILayout.Button("Hide List", GUILayout.Width(65)))
            {
                displayGameObjectsWithSpecificTagField = false;
            }
        }
    }

    private void CreateNewTagField()
    {
        GUILayout.Space(10);
        GUILayout.Label("Create New Tag", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        newTagName = EditorGUILayout.TextField(newTagName);

        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && !string.IsNullOrEmpty(newTagName))
        {
            AddTag(newTagName);
            newTagName = "";
            Repaint();
        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Tag") && !string.IsNullOrEmpty(newTagName))
        {
            AddTag(newTagName);
            newTagName = "";
        }

        GUILayout.EndHorizontal();
    }

    private void AddTag(string tag)
    {
        if (!TagExists(tag))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            int arraySize = tagsProp.arraySize;
            tagsProp.InsertArrayElementAtIndex(arraySize);
            SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(arraySize);
            newTag.stringValue = tag;
            tagManager.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogWarning("Tag already exists.");
        }
    }

    private void CopyToClipboard(string text)
    {
        TextEditor te = new TextEditor { text = text };
        te.SelectAll();
        te.Copy();
    }

    private void ChangeTagOfSelectedObject(string tag)
    {
        if (Selection.activeGameObject != null)
        {
            Undo.RecordObject(Selection.activeGameObject, "Change Tag");
            Selection.activeGameObject.tag = tag;
            EditorUtility.SetDirty(Selection.activeGameObject);
        }
        else
        {
            Debug.LogWarning("No GameObject selected.");
        }
    }


    private bool TagExists(string tag)
    {
        return UnityEditorInternal.InternalEditorUtility.tags.Contains(tag);
    }
}
