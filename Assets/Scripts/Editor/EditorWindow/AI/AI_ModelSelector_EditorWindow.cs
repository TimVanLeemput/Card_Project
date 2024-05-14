using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class AI_ModelSelector_EditorWindow
{

    public static List<Model> allChatModels = new List<Model>();
    public static AI_Card_Tool_EditorWindow tool = null;

    public static void SetTool(AI_Card_Tool_EditorWindow _tool)
    {
        tool = _tool;
    }
    /// <summary>
    /// Call this method to define the Chat Model of the type Conversation
    /// </summary>
    /// <param name="_conversation"></param>
    /// <param name="_selectedModel"></param>
    public static void ChatModelSelection()
    {
        if (tool == null) return;
        List<Model> _allModels = Model.PopulateModels();

        //if(_allModels != null)
        Debug.Log($"First in array of all models is => {_allModels[0].ModelID}");


        Model _currentModel = tool.Conversation.Model;
        Debug.Log($"Current model is => {_currentModel.ModelID}");

    }

    //static List<Model> SetModels()
    //{
    //    return Model.PopulateModels();
    //}
}
