using OpenAI_API.Images;
using OpenAI_API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AI_ImageGenerator_EditorWindow : EditorWindow
{
    // GameObjects
    public static GameObject goImageTarget = null;

    //Strings
    public static string userInputPrompt = "";
    //public static string generatedImageURL = ""; // to delete

    // Booleans
    //private static bool openImageInBrowser; // to delete

    //Events
    //public static event Action<Texture2D> onTextureLoadedFromURL = null;// to delete


    #region Accessors
    public static string UserInputPrompt
    {
        get { return userInputPrompt; }
        set { userInputPrompt = value; }
    }
    //to delete
    //public string GeneratedImageUrl
    //{
    //    get { return generatedImageURL; }
    //    set { generatedImageURL = value; }
    //}
    #endregion

    public static void ImageGeneratorField()
    {
        ImagePromptField();
        GameObjectToApplyImageOnField();
        GeneratedURLField();
        GenerateImageButtonField();
        ApplyImageToGameObjectButtonField();
    }

    public static void ImagePromptField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Prompt");
        userInputPrompt = GUILayout.TextField(userInputPrompt, 200);
        GUILayout.EndHorizontal();
    }

    private static void GameObjectToApplyImageOnField()
    {
        GUILayout.BeginHorizontal();
        goImageTarget = (GameObject)EditorGUILayout.ObjectField("GameObject to place Image on",
                                                                 goImageTarget, typeof(GameObject), true);
        GUILayout.EndHorizontal();
    }

    private static void GeneratedURLField()
    {
        GUILayout.BeginHorizontal();
        OpenInBrowserButtonField();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUIHelpers_Editor.SpaceV(5);
        GUIStyle _labelStyle = new GUIStyle(EditorStyles.helpBox);

        GUIContent labelContent = new GUIContent("URL to Image", "URL generated by the prompt");
        Vector2 labelSize = _labelStyle.CalcSize(labelContent);

        float windowWidth = EditorGUIUtility.currentViewWidth;
        float remainingSpace = windowWidth - labelSize.x;

        GUILayout.Space(remainingSpace / 2);
        GUILayout.Label(labelContent, _labelStyle);
        GUILayout.Space(remainingSpace / 2);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (AI_ImageGenerator.GeneratedImageURL != null)
            GUILayout.TextArea(AI_ImageGenerator.GeneratedImageURL, 200);

        if (AI_ImageGenerator.GeneratedImageURL == null)
            GUILayout.TextArea("", 200);

        GUILayout.EndHorizontal();
        GUIHelpers_Editor.SpaceV(5);
    }

    private static void OpenInBrowserButtonField()
    {
        GUILayout.BeginHorizontal();
        bool _browserButton = GUILayout.Button("Open in browser");
        if (_browserButton)
            Application.OpenURL(AI_ImageGenerator.GeneratedImageURL);
        bool _editURL = GUILayout.Button("Reset URL");
        if (_editURL)
        {
            AI_ImageGenerator.GeneratedImageURL = null;
            Debug.Log("reset url");
        }
        GUILayout.EndHorizontal();
    }
    private static void GenerateImageButtonField()
    {
        GUILayout.BeginHorizontal();
        bool _generateImageButton = GUILayout.Button("Generate image");
        if (_generateImageButton)
        {
            if (userInputPrompt == string.Empty)
            {
                Debug.LogError("Please enter a valid prompt");
                GUILayout.EndHorizontal();
            }
            AI_ImageGenerator.CreateImageURL();
        }
        GUILayout.EndHorizontal();
    }
    //to delete
    //public static async void CreateImageURL()
    //{
    //    // TO DO
    //    // Auth check to double check or remove entirely
    //    #region Auth check and tab movement
    //    //if (openAIAPI == null)
    //    //{
    //    //    Debug.Log("Failed authentication, please login");
    //    //    FailedAuthentication_EditorWindow.ShowWindow();
    //    //    tabs = 1;
    //    //    return;
    //    //}
    //    //bool _checkAuth = await openAIAPI.Auth.ValidateAPIKey();
    //    //if (_checkAuth)
    //    //{ tabs = 0; }
    //    //else tabs = 1;
    //    #endregion
    //    try
    //    {
    //        Task<ImageResult> _result = AI_Authentication.OpenAIAPI.ImageGenerations.CreateImageAsync(userInputPrompt);  // This is using the prompt
    //        await _result; // Wait for the task to complete

    //        if (_result == null)
    //        {
    //            Debug.Log("Null result");
    //            return;
    //        }

    //        while (!_result.IsCompleted)
    //        {
    //            Debug.Log("Generating AI image");
    //            await Task.Delay(8000);
    //        }
    //        if (_result.IsCompleted)
    //        {

    //            ImageResult result = _result.Result;
    //            string _imageUrl = result.ToString();
    //            if (_imageUrl == null) return;
    //            Debug.Log($"Image URL: {_imageUrl}");
    //            generatedImageURL = _imageUrl;

    //            if (openImageInBrowser)  //Optional opening 
    //                Application.OpenURL(generatedImageURL);
    //            GetURLTexture();
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log($"Failed async call to generate AI Image{e.Message}");
    //    }
    //}
    //to delete
    //public static async void GetURLTexture()
    //{
    //    if (generatedImageURL == null) return;
    //    try
    //    {
    //        UnityWebRequest _webRequest = UnityWebRequestTexture.GetTexture(generatedImageURL);
    //        _webRequest.SendWebRequest();

    //        while (!_webRequest.isDone)
    //        {
    //            await Task.Delay(8000);
    //        }

    //        if (_webRequest.result == UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log("Download successful");
    //            var _texture = DownloadHandlerTexture.GetContent(_webRequest);
    //            onTextureLoadedFromURL?.Invoke(_texture);
    //        }
    //        else
    //        {
    //            Debug.Log($"Failed to load web request: {_webRequest.error}");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log($"Error loading website image: {e.Message}");
    //    }
    //}
    private static void ApplyImageToGameObjectButtonField()
    {
        GUILayout.BeginHorizontal();
        bool _applyButton = GUILayout.Button("Apply image");
        if (_applyButton)
        {
            AI_ImageGenerator.GetURLTexture();
        }
        GUILayout.EndHorizontal();
    }
    // FIX URL NOT UPDATING
}
