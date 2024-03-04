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
using Unity.VisualScripting.YamlDotNet.Core.Tokens;


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
    public string API_KEY = "";
    public string tempKey = "";

    public bool urlHasBeenGenerated = false;
    public bool openImageInBrowser = false;
    public bool canRevealPassword = false;

    //2D icons
    private Texture2D revealPassWordIcon = null;


    //Events
    public event Action<Texture2D> onTextureLoadedFromURL = null;
    public event Action<string> onPasswordEntered = null;
    public event Action<bool> onRevealPasswordButtonClicked = null;
    
    //Tabs
    public int tabs = 3;
    string[] tabSelection = new string[] { "Image Generation", "Credentials", "Chat" };


    // Styles

    [MenuItem("Tools/AI Helper")]
    public static void ShowWindow()
    {
        //GetWindow<ImageGeneratorTool_EditorWindow>("AI Image Generator").Show();
        ImageGeneratorTool_EditorWindow t = GetWindow<ImageGeneratorTool_EditorWindow>(typeof(ImageGeneratorTool_EditorWindow));
    }


    private void OnEnable()
    {
        onTextureLoadedFromURL += SetGameObjectMaterial;
        onPasswordEntered += Authenticate;
        onRevealPasswordButtonClicked += SetCanRevealPassword;
        Init2DTextures();

    }


    private void OnGUI()
    {
        tabs = GUILayout.Toolbar(tabs, tabSelection);
        Space();


        switch (tabs)
        {
            case 0:
                ImageGeneratorTab();
                break;
            case 1:
                APIKeyField();                
                break;
            case 2:
                ImagePromptField();
                break;

        }

    }
    private void Init2DTextures()
    {
        // Load the eye icon texture
        revealPassWordIcon = Resources.Load<Texture2D>("reveal_password_Icon_white"); 
    }

    private void ImageGeneratorTab()
    {
        //AuthenticateButton();
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
        // openAIAPI = new OpenAIAPI("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");
        openAIAPI = new OpenAIAPI(API_OpenAI_Authentication.OPENAI_API_KEY);
        Debug.Log("Authenticate called");

        try
        {
            bool isValidKey = await openAIAPI.Auth.ValidateAPIKey();
            if (isValidKey)
            {
                Debug.Log("Authentication to OpenAI successful");
                tabs = 0;
            }
            else
            {
                Debug.LogError("Invalid API key provided");
                // OpenAI_Login_Pop_Up_EditorWindow.ShowWindow();
                tabs = 1;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to authenticate to OpenAI: " + ex.Message);
        }
    }
    private async void Authenticate(string _apiKey)
    {
        openAIAPI = new OpenAIAPI(_apiKey);
        Debug.Log("Authenticate with event called");

        try
        {
            bool isValidKey = await openAIAPI.Auth.ValidateAPIKey();
            if (isValidKey)
            {
                Debug.Log("Authentication to OpenAI successful");
                tabs = 0;
                Repaint();
            }
            else
            {
                Debug.LogError("Invalid API key provided");
                // OpenAI_Login_Pop_Up_EditorWindow.ShowWindow();
                tabs = 1;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to authenticate to OpenAI: " + ex.Message);
        }
    }

    private void APIKeyField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("OpenAI API Key :");
        //API_KEY = GUILayout.TextField(API_KEY, 400);
        EditorGUI.BeginChangeCheck();
        if (!canRevealPassword)
        {
            tempKey = GUILayout.PasswordField(API_KEY, '*');
            Debug.Log($"{tempKey}");
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
                onPasswordEntered?.Invoke(tempKey);
                Event.current.Use(); // Consume the event to prevent other actions
            }
        }
        RevealPasswordButton();
        AuthenticateButton();

        GUILayout.EndHorizontal();
    }
    private void RevealPasswordButton()
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

    private void SetCanRevealPassword(bool _value)
    {
        canRevealPassword = _value;
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
