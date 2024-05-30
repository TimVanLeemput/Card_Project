using OpenAI_API;
using UnityEditor;
using UnityEngine;



public class AI_Card_Tool_EditorWindow : EditorWindow
{
    [SerializeField] public static OpenAIAPI openAIAPI = null;
    static AI_Card_Tool_EditorWindow  toolInstance = null;
    #region Booleans
    #endregion


    #region Events
  
    #endregion

    #region EditorWindow_UI
    public string manualURL = "";

    //Tabs
    public static int tabs = 3;
    string[] tabSelection = new string[] { "Image Generation", "Credentials", "Flavor Text Generation" };
    #endregion



    #region AI_Generated
    #endregion

    #region Accessors
    public static int Tabs
    {
        get { return tabs; }
        set { tabs = value; }
    }
    public static OpenAIAPI OpenAIAPI
    {
        get { return openAIAPI; }
        set { openAIAPI = value; }
    }

    #endregion

    [MenuItem("Tools/AI Helper")]
    public static void ShowWindow()
    {
        AI_Card_Tool_EditorWindow t = GetWindow<AI_Card_Tool_EditorWindow>(typeof(AI_Card_Tool_EditorWindow));
        if (toolInstance != null) return;
        toolInstance = t;
    }
    private void OnEnable()
    {
        Init();
        AI_Authentication_EditorWindow.InitAIAuthEvents();
        AI_ModelSelector_EditorWindow.Init();
    }
    private void Init()
    {
        InitEvents();
        AI_ModelSelector.Init();
        AI_FlavorTextGenerator.Init();
        AI_GameObjectMaterialSetter.Init();

    }
   
    private void InitEvents()
    {
        InitAuthEvents();
        AI_ImageGenerator.onTextureLoadedFromURL += AIFolderCreator_Editor.CreateAIMaterialsFolders;
        AIFolderCreator_Editor.OnAIMatAndTexturePathsCreated += AI_GameObjectMaterialSetter.SaveGameObjectMaterial;
    }
 
    private void InitAuthEvents()
    {
        AI_Authentication_EditorWindow.onPasswordEntered += AI_Authentication.AuthenticateCall;
    }

    public static AI_Card_Tool_EditorWindow GetTool()
    {
        if(toolInstance != null) return toolInstance;
        else return null;
    }

    private void OnGUI()
    {
        tabs = GUILayout.Toolbar(tabs, tabSelection);
        GUIHelpers.SpaceV();

        switch (tabs)
        {
            case 0:
                ImageGeneratorTab();
                break;
            case 1:
                CredentialsTab();
                break;
            case 2:
                FlavorTextGeneratorTab();
                break;
        }
    }
    private void ImageGeneratorTab()
    {
        AI_Authentication.AuthenticateCall();
        AI_ImageGenerator_EditorWindow.ImageGeneratorField();
    }
    private void CredentialsTab()
    {
        AI_Authentication.AuthenticateCall();
        AI_Authentication_EditorWindow.APIKeyField();
    }
    private void FlavorTextGeneratorTab()
    {
        AI_Authentication.AuthenticateCall();
        AI_FlavorTextGenerator_EditorWindow.FlavorTextGenerationField();

    }
}
