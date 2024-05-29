using OpenAI_API;
using UnityEditor;
using UnityEngine;



public class AI_Card_Tool_EditorWindow : EditorWindow
{
    [SerializeField] public static OpenAIAPI openAIAPI = null;
    [SerializeField] Material material = null;
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
        //GetWindow<ImageGeneratorTool_EditorWindow>("AI Image Generator").Show();
        AI_Card_Tool_EditorWindow t = GetWindow<AI_Card_Tool_EditorWindow>(typeof(AI_Card_Tool_EditorWindow));
        if (toolInstance != null) return;
        toolInstance = t;
    }
    private void OnEnable()
    {
        Init();
        AI_Authentication_EditorWindow.InitAIAuthEvents();
    }
    private void Init()
    {
        InitEvents();
        AI_ModelSelector_Editor.Init();
        AI_FlavorTextGenerator.Init();


    }
   
    private void InitEvents()
    {
        InitAuthEvents();
        AI_ImageGenerator.onTextureLoadedFromURL += AIFolderCreator_Editor.CreateAIMaterialsFolders;
        AIFolderCreator_Editor.OnAIMatAndTexturePathsCreated += SetGameObjectMaterial;
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
        GUIHelpers_Editor.SpaceV();

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
   
    /// <summary>
    /// This method  
    /// </summary>
    /// <param name="_texture"></param>
    /// <param name="_texturePath"></param>
    /// <param name="_matPath"></param>
    private void SetGameObjectMaterial(Texture2D _texture, string _texturePath, string _matPath)
    {
        AssetDatabase.CreateAsset(_texture, _texturePath);   // Creates 2DTextureFile

        Material _newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        AssetDatabase.CreateAsset(_newMaterial, _matPath);
        AI_ImageGenerator_EditorWindow.goImageTarget.GetComponent<MeshRenderer>().material = _newMaterial;
        AI_ImageGenerator_EditorWindow.goImageTarget.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = _texture;
        material = AI_ImageGenerator_EditorWindow.goImageTarget.GetComponent<MeshRenderer>().sharedMaterial;
        material.mainTexture = _texture;

        AssetDatabase.SaveAssets();
    }
}
