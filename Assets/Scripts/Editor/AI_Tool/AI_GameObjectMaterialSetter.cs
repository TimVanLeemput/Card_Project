using UnityEditor;
using UnityEngine;

public class AI_GameObjectMaterialSetter 
{
    [SerializeField] static Material material = null;

    public static void SetGameObjectMaterial(Texture2D _texture, string _texturePath, string _matPath)
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
