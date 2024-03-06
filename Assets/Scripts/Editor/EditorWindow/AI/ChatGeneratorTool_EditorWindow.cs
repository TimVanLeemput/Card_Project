using NUnit.Framework.Interfaces;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum ChatModel
{
    None,
    Ada,
    AdaEmbedding,
    Babbage,
    Curie,
    Davinci,
    CushmanCode,
    DavinciCode,
    ChatGPTTurbo,
    TextModerationLatest

}
public class ChatGeneratorTool_EditorWindow
{
    OpenAIAPI openAIAPI = null;

    [SerializeField] Model model = null;
    [SerializeField] ChatModel chatModel = ChatModel.None;

    public event Action<Conversation> OnChatSet = null;

    private void GetAPI()
    {
        ImageGeneratorTool_EditorWindow _imageGenerator = ImageGeneratorTool_EditorWindow.GetWindow<ImageGeneratorTool_EditorWindow>();
        if (openAIAPI == null) return;
        openAIAPI = _imageGenerator.OpenAIAPI;
        Debug.Log($"api is = {openAIAPI}");
    }

    // all functions must have a API in the params? 

    public static void SetModel(Model _model)
    {

    }


    public async static void StartChat(OpenAIAPI _api, ChatModel _model)
    {
        Conversation _chat = _api.Chat.CreateConversation();
        switch (_model)
        {
            case ChatModel.Ada:
                _chat.Model = Model.AdaText;
                break;
            case ChatModel.AdaEmbedding:
                _chat.Model = Model.AdaTextEmbedding;
                break;
            case ChatModel.Babbage:
                _chat.Model = Model.BabbageText;
                break;
            case ChatModel.Curie:
                _chat.Model = Model.CurieText;
                break;
            case ChatModel.Davinci:
                _chat.Model = Model.DavinciText;
                break;
            case ChatModel.CushmanCode:
                _chat.Model = Model.CushmanCode;
                break;
            case ChatModel.DavinciCode:
                _chat.Model = Model.DavinciCode;
                break;
            case ChatModel.ChatGPTTurbo:
                _chat.Model = Model.ChatGPTTurbo;
                break;
            case ChatModel.TextModerationLatest:
                _chat.Model = Model.TextModerationLatest;
                break;
            default:
                Debug.LogError("Unsupported chat model");
                break;
        }
        

        /// replace the card name, type, resource type and flavor text type with variables
        _chat.AppendSystemMessage("Set up the model to generate " +
            "Magic: The Gathering-style card flavor text prompts. Include parameters for card name," +
            " type (creature/instant/ritual), associated resource type (air, fire, darkness, light, water, earth)," +
            " and the type of flavor text (creature/spell/landscape). Ensure the responses capture the essence of the card's theme and characteristics," +
            " incorporating details such as creature subtype (human/humanoid/animal/living/plant), action for spells, or description for landscapes." );
        string _response = await _chat.GetResponseFromChatbot();
        Debug.Log(_response);

    }


    private void InvokeChat(Conversation _chat)
    {
        OnChatSet?.Invoke(_chat);
    }


}
