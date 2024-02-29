using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ComponentUtils
{
    public static T GetComponentInChildren<T>(this GameObject obj, bool includeInactive = false, bool excludeParent = false) where T : Component
    {
        var components = obj.GetComponentsInChildren<T>(includeInactive);

        if (!excludeParent)
            return components.FirstOrDefault();

        return components.FirstOrDefault(childComponent =>
            childComponent.transform != obj.transform);
    }
}

