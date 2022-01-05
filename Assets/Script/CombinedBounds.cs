using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedBounds : MonoBehaviour
{
    /// <summary>
    /// Get the combined bounds of a .
    /// </summary>
    /// <returns>Entire bounds of a multi-GameObject element</returns>
    public Bounds GetCombinedBounds()
    {
        Bounds combinedBounds = new Bounds();
        
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer render  in renderers) {
            if (render != null) combinedBounds.Encapsulate(render.bounds);
        }

        return combinedBounds;
    }
}
