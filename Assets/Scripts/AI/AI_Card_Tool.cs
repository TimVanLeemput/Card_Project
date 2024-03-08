using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OpenAI_API.Images;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using OpenAI_API;
using System;




public class AI_Card_Tool : MonoBehaviour
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

    private void Start()
    {
        Init();
    }

    private void Init() 
    {
        openAIAPI = new OpenAIAPI("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");
        Debug.Log($"auth is {openAIAPI.Auth.ToString()}");
        Debug.Log($"user input is {userInputPrompt}");
        Invoke(nameof(CreateImageURL), 3);
    }
    public async void CreateImageURL()
    {
        try
        {
            Task<ImageResult> _result = openAIAPI.ImageGenerations.CreateImageAsync(userInputPrompt);
            await _result; // Wait for the task to complete

            if (_result == null)
            {
                Debug.Log("Null result");
                return;
            }

            // Access the result after awaiting the task
            ImageResult result = _result.Result;

            if (result == null || result.Data == null || string.IsNullOrEmpty(result.ToString()))
            {
                Debug.Log("Invalid image result or image URL");
                return;
            }

            string imageUrl = result.ToString();
            Debug.Log($"Image URL: {imageUrl}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error creating image URL: {e.Message}");
        }
    }


    //public async void CreateImageURL()
    //{

    //    Task<ImageResult> _result = openAIAPI.ImageGenerations.CreateImageAsync(userInputPrompt);
    //   await _result;

    //    if (_result == null)
    //    { 
    //        Debug.Log("null result");
    //        return;
    //    }
    //    Debug.Log($" Task Image result = {_result}");
    //    int _id =_result.Id;
    //    Debug.Log($" Task Image id = {_id}");


    //    string _url = _result.ToString();
    //    if (_url == null)
    //    {
    //        Debug.Log("_url is null");
    //        return;
    //    }
    //    Debug.Log($" image url = {_url}");
    //}

    void Update()
    {

    }
}
