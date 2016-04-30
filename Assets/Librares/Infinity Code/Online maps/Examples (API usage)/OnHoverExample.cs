/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/OnHoverExample")]
    public class OnHoverExample : MonoBehaviour
    {
        private OnlineMapsMarkerBase hoverMarker;

        private void Start()
        {
            OnlineMaps api = OnlineMaps.instance;
            OnlineMapsMarker marker = api.AddMarker(new Vector2(), "Marker");
            marker.OnRollOver += OnRollOver;
            marker.OnRollOut += OnRollOut;
            api.position = new Vector2();
        }

        private void OnRollOut(OnlineMapsMarkerBase marker)
        {
            hoverMarker = null;
        }

        private void OnRollOver(OnlineMapsMarkerBase marker)
        {
            hoverMarker = marker;
        }

        private void Update()
        {
            if (hoverMarker != null) Debug.Log(hoverMarker.label);
        }
    }
}