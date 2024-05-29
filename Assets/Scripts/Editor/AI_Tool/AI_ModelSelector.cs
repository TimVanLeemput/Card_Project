using OpenAI_API.Models;
using System.Collections.Generic;
using UnityEngine;


public static class AI_ModelSelector_Editor
{

    public static List<Model> allChatModels = null;
    public static AI_Card_Tool_EditorWindow tool = null;

    public static void Init()
    {
        SetTool();
        ChatModelSelection();
    }
    
    public static void SetTool()
    {
        if (tool != null) return;
        tool = AI_Card_Tool_EditorWindow.GetTool();
    }

    public static void SetAllChatModels()
    {
        allChatModels = new List<Model>();
    }

    /// <summary>
    /// Call this method to define the Chat Model of the type Conversation
    /// </summary>
    /// <param name="_conversation"></param>
    /// <param name="_selectedModel"></param>
    public static void SetChatModel(Model _model)
    {
        if (tool == null || _model == null)
        {
            Debug.Log($"FAILED TO FIND TOOL OR MODEL in {typeof(AI_ModelSelector_Editor)}");
            return;
        }
        AI_FlavorTextGenerator.Conversation.Model = _model;

        Debug.Log($"Selected model => {_model.ModelID})");

    }
    public static void ChatModelSelection()
    {
        if (tool == null) return;
        List<Model> _allModels = Model.PopulateModels();

        allChatModels = _allModels;
    }
}
