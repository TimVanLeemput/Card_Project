using NUnit.Framework.Internal;
using OpenAI_API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class AI_Authentication_Editor
{
    public static OpenAIAPI openAIAPI = null;
    public static OpenAIAPI OpenAIAPI
    {
        get { return openAIAPI; }
        set { openAIAPI = value; }
    }
    public static async Task<OpenAIAPI> Authenticate(string _apiKey, EditorWindow _editorWindow)
    {
        if (openAIAPI != null) return null;
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
}
