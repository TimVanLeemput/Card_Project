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
    

    public bool urlHasBeenGenerated = false;
    public bool openImageInBrowser = false;


    // Styles

    [MenuItem("Tools/AI Image Generator")]
    public static void ShowWindow()
    {
        GetWindow<ImageGeneratorTool_EditorWindow>("AI Image Generator").Show();

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
        SaveMaterialButton();

    }
    private void Authenticate()
    {

        openAIAPI = new OpenAIAPI("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");
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
        EditorGUI.BeginChangeCheck();
        goImageTarget = (GameObject)EditorGUILayout.ObjectField("GameObject to place Image on", goImageTarget, typeof(GameObject), true);
        if (EditorGUI.EndChangeCheck())
        {

            //material = goImageTarget.GetComponent<MeshRenderer>().sharedMaterial;
            //Debug.Log($"material is = > {material}");
        }
        GUILayout.EndHorizontal();
    }


    private void GenerateImageButton()
    {

        GUILayout.BeginHorizontal();
        bool _generateImageButton = GUILayout.Button("Generate image");
        if (_generateImageButton)
        {
            CreateImageURL();
            //string _url = "https://tinyurl.com/y92xdw8z";
            //string _url1 = "https://tinyurl.com/y92xdw8z";
            //float _rand = UnityEngine.Random.Range(1, 3);
            //switch (_rand)
            //{
            //    case 1:
            //        generatedImageURL = _url;
            //        break;
            //    case 2:
            //        generatedImageURL = _url1;
            //        break;
            //}
            //GetURLTexture();

            // TO DO 
            // Grab image from URL
            // Apply to GameObject
            // Save GameObject as Prefab



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

        Space();
        GUILayout.BeginHorizontal();

        GUILayout.Label("generated URL");
        OpenInBrowserButton();
        if (generatedImageURL != null)
            GUILayout.TextArea(generatedImageURL, 200);

        GUILayout.TextArea("", 200);
        GUILayout.EndHorizontal();
        Space();
    }

    private void OpenInBrowserButton()
    {
        bool _browserButton = GUILayout.Button("Open in browser");
        if (_browserButton)
            Application.OpenURL(generatedImageURL);
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

            // Access the result after awaiting the task


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
            //Application.OpenURL(generatedImageURL);   // open on generation
            return;
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

            //Wait until the web request is complete
            while (!_webRequest.isDone)
            {
                await Task.Delay(8000); // Delay to avoid busy waiting

            }

            // Check if the web request was successful
            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Download successful");
                var _texture = DownloadHandlerTexture.GetContent(_webRequest);               
                string path = "Assets/Materials/AI_Mats/2DTextures/Downloaded2DTexture.asset";
                string _newMatPath = "Assets/Materials/AI_Mats/Materials/GameObjectMaterialDynamic.mat";
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                _newMatPath = AssetDatabase.GenerateUniqueAssetPath(_newMatPath);
                // Save the material as an asset
                AssetDatabase.CreateAsset(_texture, path);   // Creates 2DTextureFile

                string _currentGOMaterialPath = AssetDatabase.GetAssetPath(goImageTarget.GetComponent<MeshRenderer>().sharedMaterial);
                Debug.Log($" current game object material path is  = {_currentGOMaterialPath}");

                //Material _newGoImageMat = goImageTarget.GetComponent<MeshRenderer>().sharedMaterial;   // looks useless
                //_newGoImageMat.mainTexture = _texture;
                Material _newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

                AssetDatabase.CreateAsset(_newMaterial, _newMatPath);
                goImageTarget.GetComponent<MeshRenderer>().material = _newMaterial;
                goImageTarget.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = _texture;
                material = goImageTarget.GetComponent<MeshRenderer>().sharedMaterial;
                material.mainTexture = _texture;

                AssetDatabase.SaveAssets();
                // Save
                //SaveMaterial();
                //rawImage.texture = _texture;
                //
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
