using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnpackTool_Editor : Editor
{
    [MenuItem("Shortcuts/Unpack")]
    static void Unpack()
    {
        Unpack(PrefabUnpackMode.OutermostRoot);
    }
    [MenuItem("Shortcuts/UnpackCompletely")]
    public static void UnpackCompletely()
    {
        Unpack(PrefabUnpackMode.Completely);
    }
    static void Unpack(PrefabUnpackMode mode)
    {
        foreach (Transform transform in Selection.transforms)
        {
            RectTransform t = transform as RectTransform;

            if (PrefabUtility.GetPrefabAssetType(transform.gameObject) != PrefabAssetType.Regular)
            {
                continue;
            }
            else
            {
                PrefabUtility.UnpackPrefabInstance(transform.gameObject, mode, InteractionMode.UserAction);
            }
        }
    }
    public static void Unpack(PrefabUnpackMode mode, List<Transform> _transforms)
    {
        foreach (Transform transform in _transforms)
        {
            if (PrefabUtility.GetPrefabAssetType(transform.gameObject) != PrefabAssetType.Regular)
            {
                continue;
            }
            else
            {
                PrefabUtility.UnpackPrefabInstance(transform.gameObject, mode, InteractionMode.UserAction);
            }
        }
    }
}