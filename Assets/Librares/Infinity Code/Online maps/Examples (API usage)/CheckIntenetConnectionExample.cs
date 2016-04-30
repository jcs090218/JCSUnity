/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/CheckIntenetConnectionExample")]
    public class CheckIntenetConnectionExample : MonoBehaviour
    {
        private void Start()
        {
            // Begin to check your Internet connection.
            OnlineMaps.instance.CheckServerConnection(OnCheckConnectionComplete);
        }

        // When the connection test is completed, this function will be called.
        private void OnCheckConnectionComplete(bool status)
        {
            // If the test is successful, then allow the user to manipulate the map.
            OnlineMapsControlBase.instance.allowUserControl = status;

            // Showing test result in console.
            Debug.Log(status ? "Has connection" : "No connection");
        }
    }
}