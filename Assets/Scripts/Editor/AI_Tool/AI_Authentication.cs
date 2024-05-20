using NUnit.Framework.Internal;
using OpenAI_API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class AI_Authentication
{
    public static OpenAIAPI openAIAPI = null;
    public static OpenAIAPI OpenAIAPI
    {
        get { return openAIAPI; }
        set { openAIAPI = value; }
    }
    public static async Task<OpenAIAPI> Authenticate(string _apiKey, EditorWindow _editorWindow)
    {
        if (openAIAPI == null)
        {

            openAIAPI = new OpenAIAPI(_apiKey);

            try
            {
                bool isValidKey = await openAIAPI.Auth.ValidateAPIKey();
                if (isValidKey)
                {
                    Debug.Log("Authentication to OpenAI successful");
                    //tabs = 0;
                    _editorWindow.Repaint();
                    return openAIAPI;
                }
                else
                {
                    Debug.Log("Invalid API key provided");
                    FailedAuthentication_EditorWindow.ShowWindow();

                    AI_Card_Tool_EditorWindow.Tabs = 1;
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to authenticate to OpenAI: " + ex.Message);
                FailedAuthentication_EditorWindow.ShowWindow();
                return null;
            }
        }
        return null;
    }
    public static async void AuthenticateCall()
    {
        EditorWindow _toolWindow =  EditorWindow.GetWindow<AI_Card_Tool_EditorWindow>();
        await Authenticate(API_OpenAI_Authentication.GetApiKey(), _toolWindow);
    }

    public static async void AuthenticateCall(string _tempKey)
    {
        EditorWindow _toolWindow = EditorWindow.GetWindow<AI_Card_Tool_EditorWindow>();
        await Authenticate(_tempKey, _toolWindow);
    }
}
