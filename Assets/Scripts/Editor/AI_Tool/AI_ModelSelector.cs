using OpenAI_API.Models;
using System.Collections.Generic;
using UnityEngine;


public static class AI_ModelSelector
{

    public static List<Model> allChatModels = null;
    public static AI_Card_Tool_EditorWindow tool = null;

    public static void Init()
    {
        SetTool();
        PopulateAllChatModels();
    }
    
    public static void SetTool()
    {
        //if (tool != null) return;
        tool = AI_Card_Tool_EditorWindow.GetTool();
    }


    public static List<string> GetAllChatModelsIDs()
    {
        if (allChatModels.Count <= 0) return null;
        List<string> _allModelsID = new List<string>();
        foreach (Model _model in allChatModels)
        {
            _allModelsID.Add(_model.ModelID);
        }
        return _allModelsID;
    }
    /// <summary>
    /// Call this method to define the Chat Model of the type Conversation
    /// </summary>
    /// <param name="_conversation"></param>
    /// <param name="_selectedModel"></param>
    public static void SetChatModel(Model _model)
    {
        if (_model == null)
        {
            Debug.Log($"FAILED TO FIND MODEL in {typeof(AI_ModelSelector)}");
            return;
        }
        AI_FlavorTextGenerator.Conversation.Model = _model;
    }
    public static void PopulateAllChatModels()
    {
        List<Model> _allModels = Model.PopulateModels();

        allChatModels = _allModels;

    }


}
