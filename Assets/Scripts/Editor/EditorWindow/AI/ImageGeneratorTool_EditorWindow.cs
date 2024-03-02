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


public class ImageGeneratorTool_EditorWindow : EditorWindow
{
    OpenAIAPI openAIAPI = null;

    ImageGenerationRequest imageGeneration = null;
    //IImageGenerationEndpoint iImageGenerationEndpoint = null;
    ImageGenerationEndpoint imageGenerationEndpoint = null;
    ImageResult imageResult = null;
    GameObject goImageTarget = null;
    public string userInputPrompt = "";
    public string generatedImageURL = "";

    public bool OpenAIAPISet = false;

    [MenuItem("Tools/AI Image Generator")]
    public static void ShowWindow()
    {
        GetWindow<ImageGeneratorTool_EditorWindow>("AI Image Generator").Show();

    }

    private void OnGuiInit()
    {
        // Instantiate the 
        openAIAPI = new OpenAIAPI();
        // Instantiate the ImageGenerationEndpoint using the OpenAIAPI instance
        imageGenerationEndpoint = openAIAPI.ImageGenerations;

    }

    private void OnGUI()
    {//
        OpenAIAPI _openAIAPI;

        if (!OpenAIAPISet)
        {
            _openAIAPI = new OpenAIAPI("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"); // use env vars
            OpenAIAPISet = true;
   

        // Instantiate the ImageGenerationEndpoint using the OpenAIAPI instance
        GUILayout.BeginHorizontal();
        GUILayout.Label("Prompt");
        userInputPrompt = GUILayout.TextField(userInputPrompt, 200);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Debug prompt string and auth"))
        {

            Debug.Log($"user input prompt is = {userInputPrompt}");
            Debug.Log($"API version is  = {_openAIAPI.ApiVersion}");
            Debug.Log($"API version is  = {_openAIAPI.Models.RetrieveModelDetailsAsync("0").Result}");
        }
        GUILayout.EndHorizontal();

        // Prefab to apply image on 
        GUILayout.BeginHorizontal();
        goImageTarget = (GameObject)EditorGUILayout.ObjectField("GameObject to place Image on", goImageTarget, typeof(GameObject), true);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        bool _generateImageButton = GUILayout.Button("Generate image");
        if (_generateImageButton)
        {
            string _url;
            _url = _openAIAPI.ImageGenerations.CreateImageAsync(userInputPrompt).Result.ToString();
            Debug.Log($"image url is {_url}");
        }
        GUILayout.EndHorizontal();

        }

    }
}
