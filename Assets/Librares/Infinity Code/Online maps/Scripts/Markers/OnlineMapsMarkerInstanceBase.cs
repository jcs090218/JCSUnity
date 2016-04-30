/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Base class for instance of marker. 
/// This class is used when for each marker create a separate GameObject.
/// </summary>
[System.Serializable]
[AddComponentMenu("")]
public class OnlineMapsMarkerInstanceBase:MonoBehaviour
{
    /// <summary>
    /// Reference to marker.
    /// </summary>
    public OnlineMapsMarkerBase marker;
}