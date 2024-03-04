using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class OpenAI_Login_Pop_Up_EditorWindow : EditorWindow
{
    public static void ShowWindow()
    {
        GetWindow<OpenAI_Login_Pop_Up_EditorWindow>("OpenAI Login").Show();
    }
}
