/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/RoteteCameraByGPSExample")]
    public class RoteteCameraByGPSExample : MonoBehaviour
    {
        private void Start()
        {
            OnlineMapsLocationService.instance.OnCompassChanged += OnCompassChanged;
        }

        private void OnCompassChanged(float f)
        {
            OnlineMapsTileSetControl.instance.cameraRotation.y = f * 360;
        }
    }
}