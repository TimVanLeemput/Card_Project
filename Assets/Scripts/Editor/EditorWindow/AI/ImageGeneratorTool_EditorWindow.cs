using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OpenAI_API.Images;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Security.Policy;
using OpenAI_API;
using Codice.Client.Common;
using System;
using static System.Net.WebRequestMethods;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Events;
using UnityEngine.Rendering;
using NUnit.Framework;


public class ImageGeneratorTool_EditorWindow : EditorWindow
{
    OpenAIAPI openAIAPI = null;

    ImageGenerationRequest imageGeneration = null;
    //IImageGenerationEndpoint iImageGenerationEndpoint = null;
    ImageGenerationEndpoint imageGenerationEndpoint = null;
    ImageResult imageResult = null;
    [SerializeField] GameObject goImageTarget = null;
    [SerializeField] Material material = null;
    [SerializeField] Shader shader = null;
    RawImage rawImage = null;
    public string userInputPrompt = "";
    public string generatedImageURL = "";
    public string manualURL = "";


    public bool urlHasBeenGenerated = false;
    public bool openImageInBrowser = false;

    //Events
    public event Action<Texture2D> onTextureLoadedFromURL = null;


    // Styles

    [MenuItem("Tools/AI Image Generator")]
    public static void ShowWindow()
    {
        GetWindow<ImageGeneratorTool_EditorWindow>("AI Image Generator").Show();

    }

    private void OnEnable()
    {
        onTextureLoadedFromURL += SetGameObjectMaterial;
    }

    private void OnGUI()
    {

        AuthenticateButton();
        ImagePromptField();
        GameObjectToApplyImageOn();
        //RawImageTest();
        GeneratedURLField();
        GenerateImageButton();
        ApplyImageToGameObjectButton();
        //SaveMaterialButton();

    }
    private async void Authenticate()
    {

        openAIAPI = new OpenAIAPI("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");
        //while (openAIAPI.Auth == null)
        //{
        //}
        if (openAIAPI.Auth != null)
        {

            Debug.Log("Authentication to OpenAI successfull");
            Debug.Log($"user input is {userInputPrompt}");

        }
    }

    private void ApplyImageToGameObjectButton()
    {
        GUILayout.BeginHorizontal();
        bool _applyButton = GUILayout.Button("Apply image");
        if (_applyButton)
        {
            GetURLTexture();
        }
        GUILayout.EndHorizontal();
    }

    private void AuthenticateButton()
    {
        GUILayout.BeginHorizontal();
        bool _loginButton = GUILayout.Button("Login to OpenAI");
        if (_loginButton)
        {
            Authenticate();
            generatedImageURL = null;
        }
        GUILayout.EndHorizontal();
    }
    private void OnURLGeneratedField()
    {

        GUILayout.BeginHorizontal();
        GUILayout.Label("Prompt");
        userInputPrompt = GUILayout.TextField(userInputPrompt, 200);
        GUILayout.EndHorizontal();
    }

    private void ImagePromptField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Prompt");
        userInputPrompt = GUILayout.TextField(userInputPrompt, 200);
        GUILayout.EndHorizontal();
    }
    private void GameObjectToApplyImageOn()
    {
        // GameObject to apply image on 
        GUILayout.BeginHorizontal();
        goImageTarget = (GameObject)EditorGUILayout.ObjectField("GameObject to place Image on", goImageTarget, typeof(GameObject), true);
        GUILayout.EndHorizontal();
    }


    private void GenerateImageButton()
    {

        GUILayout.BeginHorizontal();
        bool _generateImageButton = GUILayout.Button("Generate image");
        if (_generateImageButton)
        {
            CreateImageURL();
        }
        GUILayout.EndHorizontal();
    }

    private void Space(float _value = 20)
    {
        GUILayout.BeginVertical();
        GUILayout.Space(_value);
        GUILayout.EndVertical();
    }
    private void GeneratedURLField()
    {
        GUIStyle _labelStyle = new GUIStyle(EditorStyles.helpBox);
        GUILayout.Label("generated URL", _labelStyle);
        Space();
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        OpenInBrowserButton();


        GUILayout.EndHorizontal();



        if (generatedImageURL != null)
            GUILayout.TextArea(generatedImageURL, 200);

        if (generatedImageURL == null)
            GUILayout.TextArea("", 200);

        GUILayout.EndVertical();
        Space();


    }

    private void OpenInBrowserButton()
    {
        GUILayout.BeginVertical();
        bool _browserButton = GUILayout.Button("Open in browser");
        if (_browserButton)
            Application.OpenURL(generatedImageURL);
        bool _editURL = GUILayout.Button("Reset URL");
        if (_editURL)
        {
            generatedImageURL = null;
            Debug.Log("reset url");
        }
        GUILayout.EndVertical();
    }

    public async void CreateImageURL()
    {
        try
        {
            Task<ImageResult> _result = openAIAPI.ImageGenerations.CreateImageAsync(userInputPrompt);  // This is using the prompt
            await _result; // Wait for the task to complete

            if (_result == null)
            {
                Debug.Log("Null result");
                return;
            }

            while (!_result.IsCompleted)
            {
                await Task.Delay(8000);
            }
            if (_result.IsCompleted)
            {

                ImageResult result = _result.Result;
                string _imageUrl = result.ToString();
                if (_imageUrl == null) return;
                Debug.Log($"Image URL: {_imageUrl}");
                generatedImageURL = _imageUrl;

                if (openImageInBrowser)  //Optional opening 
                    Application.OpenURL(generatedImageURL);
                GetURLTexture();
            }

        }

        catch (Exception e)
        {
            Debug.Log($"Failed async call to generate AI Image{e.Message}");

        }
    }


    public async void GetURLTexture()
    {
        if (generatedImageURL == null) return;
        try
        {
            UnityWebRequest _webRequest = UnityWebRequestTexture.GetTexture(generatedImageURL);
            _webRequest.SendWebRequest();

            while (!_webRequest.isDone)
            {
                await Task.Delay(8000);

            }

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Download successful");
                var _texture = DownloadHandlerTexture.GetContent(_webRequest);
                onTextureLoadedFromURL?.Invoke(_texture);
            }
            else
            {
                Debug.Log($"Failed to load web request: {_webRequest.error}");
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error loading website image: {e.Message}");
        }

    }

    private void SetGameObjectMaterial(Texture2D _texture)
    {
        string _userInputNoSpace = $"{userInputPrompt.Replace(" ", "_")}";  // Replacing spaces with underscores

        string _fullPathTextures = $"Assets/Materials/AI_Mats/Textures/{_userInputNoSpace}_Textures";
        if (!AssetDatabase.IsValidFolder(_fullPathTextures))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats/Textures", $"{_userInputNoSpace}_Textures"); // Doesn't allow numbers in path!
        }

        string _fullPathMaterials = $"Assets/Materials/AI_Mats/Materials/{_userInputNoSpace}_Material";
        if (!AssetDatabase.IsValidFolder(_fullPathMaterials))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats/Materials", $"{_userInputNoSpace}_Material"); // Doesn't allow numbers in path!
        }


        string _newTexturePath = $"{_fullPathTextures}/{userInputPrompt}_Downloaded_2D_Texture.asset";

        string _newMatPath = $"{_fullPathMaterials}/{userInputPrompt}_Downloaded_Material.mat";
        _newTexturePath = AssetDatabase.GenerateUniqueAssetPath(_newTexturePath);
        _newMatPath = AssetDatabase.GenerateUniqueAssetPath(_newMatPath);

        // Save the material as an asset
        AssetDatabase.CreateAsset(_texture, _newTexturePath);   // Creates 2DTextureFile

        Material _newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        AssetDatabase.CreateAsset(_newMaterial, _newMatPath);
        goImageTarget.GetComponent<MeshRenderer>().material = _newMaterial;
        goImageTarget.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = _texture;
        material = goImageTarget.GetComponent<MeshRenderer>().sharedMaterial;
        material.mainTexture = _texture;

        AssetDatabase.SaveAssets();
    }

    private void SaveMaterialButton()
    {
        GUILayout.BeginHorizontal();
        bool _loginButton = GUILayout.Button("Save material");
        if (_loginButton)
        {
            SaveMaterial();
        }
        GUILayout.EndHorizontal();
    }
    private void SaveMaterial()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
