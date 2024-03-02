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


public class ImageGeneratorTool_EditorWindow : EditorWindow
{
    OpenAIAPI openAIAPI = null;

    ImageGenerationRequest imageGeneration = null;
    //IImageGenerationEndpoint iImageGenerationEndpoint = null;
    ImageGenerationEndpoint imageGenerationEndpoint = null;
    ImageResult imageResult = null;
    GameObject goImageTarget = null;
    Material material = null;
    RawImage rawImage = null;
    public string userInputPrompt = "";
    public string generatedImageURL = "";
    public bool urlHasBeenGenerated = false;

    public bool OpenAIAPISet = false;

    public UnityEvent onURLgenerated = null;

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

    }

    private void AuthenticateButton()
    {
        GUILayout.BeginHorizontal();
        bool _loginButton = GUILayout.Button("Login to OpenAI");
        if (_loginButton)
        {
            Authenticate();
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
        { material = goImageTarget.GetComponent<MeshRenderer>().sharedMaterial; }
        GUILayout.EndHorizontal();
    }

    private void RawImageTest()
    {
        // GameObject to apply image on 
        GUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        goImageTarget = (GameObject)EditorGUILayout.ObjectField("RAWIMAGE", goImageTarget, typeof(GameObject), true);
        if (EditorGUI.EndChangeCheck())
        { rawImage = goImageTarget.GetComponent<RawImage>(); }
        GUILayout.EndHorizontal();
    }
    private void GenerateImageButton()
    {

        GUILayout.BeginHorizontal();
        bool _generateImageButton = GUILayout.Button("Generate image");
        if (_generateImageButton)
        {
            //generatedImageURL = "https://oaidalleapiprodscus.blob.core.windows.net/private/org-O3yZZz930FbEkusOa3wvE4Dz/user-0p8tN5py9OatcO60UhF8lK7B/img-YPPAivQtaZNBo3UFjz7szk8D.png?st=2024-03-02T21%3A15%3A15Z&se=2024-03-02T23%3A15%3A15Z&sp=r&sv=2021-08-06&sr=b&rscd=inline&rsct=image/png&skoid=6aaadede-4fb3-4698-a8f6-684d7786b067&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2024-03-02T18%3A24%3A19Z&ske=2024-03-03T18%3A24%3A19Z&sks=b&skv=2021-08-06&sig=wyEb5i64eXpwUlNgUhUhf8zy/GmNHVQQHgQUPY/Zqcg%3D";
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
           // generatedImageURL = "https://oaidalleapiprodscus.blob.core.windows.net/private/org-O3yZZz930FbEkusOa3wvE4Dz/user-0p8tN5py9OatcO60UhF8lK7B/img-jWdfKgUP1o7c5sLrLc0o3wT5.png?st=2024-03-02T21%3A07%3A29Z&se=2024-03-02T23%3A07%";
            GetURLTexture();

            // TO DO 
            // Grab image from URL
            // Apply to GameObject
            // Save GameObject as Prefab



        }
        GUILayout.EndHorizontal();
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
                material.mainTexture = _texture;
                //rawImage.texture = _texture;
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
       
            GUILayout.TextArea(generatedImageURL, 200);
        
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
                Application.OpenURL(generatedImageURL);
                onURLgenerated?.Invoke();


            }
            //Application.OpenURL(generatedImageURL);   // open on generation
            return;
        }

        catch (Exception e)
        {
            Debug.Log($"Failed async call to generate AI Image{e.Message}");

        }
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

}
