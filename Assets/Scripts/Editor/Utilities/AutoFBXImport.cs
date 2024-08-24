using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoFBXImport : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        // Check if the imported asset is an FBX model
        EnableReadWriteOnImport(true);
    }

    private void EnableReadWriteOnImport(bool _value)
    {
        if (assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
        {
            ModelImporter modelImporter = (ModelImporter)assetImporter;
            modelImporter.isReadable = _value;
            modelImporter.optimizeMeshPolygons = true;
        }
    }
}
