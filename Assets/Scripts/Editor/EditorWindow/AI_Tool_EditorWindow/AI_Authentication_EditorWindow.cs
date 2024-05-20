using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class AI_Authentication_EditorWindow : EditorWindow
{
    public static bool canRevealPassword = false;

    public static string API_KEY = "";
    public static string tempKey = "";

    private static Texture2D revealPassWordIcon = null;

    public static event Action<string> onPasswordEntered = null;
    public static event Action<bool> onRevealPasswordButtonClicked = null;

    #region Accessors
    public static Texture2D RevealPassWordIcon { get; set; } = null;
    #endregion

    public static void InitAIAuthEvents()
    {
        onRevealPasswordButtonClicked += SetCanRevealPassword;

    }
    private void Init2DTextures()
    {
        // Load the eye icon texture
        revealPassWordIcon = Resources.Load<Texture2D>("reveal_password_Icon_white");
    }
    private static void SetCanRevealPassword(bool _value)
    {
        canRevealPassword = _value;
    }
    public static void APIKeyField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("OpenAI API Key :");
        EditorGUI.BeginChangeCheck();
        if (!canRevealPassword)
        {
            tempKey = GUILayout.PasswordField(API_KEY, '*');
        }
        else tempKey = GUILayout.TextField(API_KEY);

        if (EditorGUI.EndChangeCheck())
        {
            API_KEY = tempKey;
        }
        if (Event.current != null && Event.current.isKey)
        {
            if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
            {
                Debug.Log("enter pressed");
                onPasswordEntered?.Invoke(tempKey);
                Event.current.Use(); // Consume the event to prevent other actions
            }
        }
        RevealPasswordButton();
        AuthenticateButton();

        GUILayout.EndHorizontal();
    }
    private static void RevealPasswordButton()
    {
        GUILayout.BeginHorizontal();
        GUIStyle _style = new GUIStyle(GUI.skin.button);
        _style.padding = new RectOffset(1, 1, 1, 1); // Adjust padding to make the button smaller
        _style.fixedWidth = 15; // Set a fixed width for the button
        _style.fixedHeight = 20; // Set a fixed height for the button
        _style.normal.background = revealPassWordIcon;
        _style.active.background = revealPassWordIcon;

        bool _revealPasswordButton = GUILayout.Button(revealPassWordIcon, _style);
        if (_revealPasswordButton)
        {
            onRevealPasswordButtonClicked?.Invoke(!canRevealPassword);
        }
        GUILayout.EndHorizontal();
    }

    private static void AuthenticateButton()
    {
        GUILayout.BeginHorizontal();
        bool _loginButton = GUILayout.Button("Login to OpenAI");
        if (_loginButton)
        {
           
        }
        GUILayout.EndHorizontal();
    }
  
}
