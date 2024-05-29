using OpenAI_API.Images;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AI_ImageGenerator
{
    private static string generatedImageURL = "";
    private static bool openImageInBrowser;

    public static event Action<Texture2D> onTextureLoadedFromURL = null;

    public static string GeneratedImageURL
    {
        get { return generatedImageURL; }
        set { generatedImageURL = value; }
    }


    public static async void CreateImageURL()
    {
        // TO DO
        // Auth check to double check or remove entirely
        #region Auth check and tab movement
        //if (openAIAPI == null)
        //{
        //    Debug.Log("Failed authentication, please login");
        //    FailedAuthentication_EditorWindow.ShowWindow();
        //    tabs = 1;
        //    return;
        //}
        //bool _checkAuth = await openAIAPI.Auth.ValidateAPIKey();
        //if (_checkAuth)
        //{ tabs = 0; }
        //else tabs = 1;
        #endregion
        try
        {
            Task<ImageResult> _result = AI_Authentication.OpenAIAPI.ImageGenerations.CreateImageAsync(AI_ImageGenerator_EditorWindow.UserInputPrompt);  // This is using the prompt
            await _result; // Wait for the task to complete

            if (_result == null)
            {
                Debug.Log("Null result");
                return;
            }

            while (!_result.IsCompleted)
            {
                Debug.Log("Generating AI image");
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
    public static async void GetURLTexture()
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
}
