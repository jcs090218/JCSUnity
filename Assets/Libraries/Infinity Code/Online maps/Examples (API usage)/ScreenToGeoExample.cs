/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/ScreenToGeoExample")]
    public class ScreenToGeoExample : MonoBehaviour
    {
        private void Update()
        {
            // Screen coordinate of the cursor.
            Vector3 mousePosition = Input.mousePosition;

            // Converts the screen coordinates to geographic.
            Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);

            // Showing geographical coordinates in the console.
            Debug.Log(mouseGeoLocation);
        }
    }
}