/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

/// <summary>
/// Building material.
/// </summary>
[Serializable]
public class OnlineMapsBuildingMaterial
{
    /// <summary>
    /// Wall material.
    /// </summary>
    public Material wall;

    /// <summary>
    /// Roof material.
    /// </summary>
    public Material roof;

    /// <summary>
    /// Wall main texture scale factor.
    /// </summary>
    public Vector2 scale = Vector2.one;
}