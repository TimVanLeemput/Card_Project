using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestMenuContexts
{
    // Add a new menu item that is accessed by right-clicking on an asset in the project view

    [MenuItem("Assets/Load Additive Scene")]
    [System.Obsolete]
    private static void LoadAdditiveScene()
    {
        Debug.Log("Loaded Additive Scenes");
    }
}
