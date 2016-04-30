/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsDemos
{
    [ExecuteInEditMode]
    public class AircraftWarning:MonoBehaviour
    {
        private void OnGUI()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.padding = new RectOffset(10, 10, 5, 5);
            GUILayout.Label("Online Maps - is a mapping solution and does not contain the physics of flight.\nIt is just an example of how to make the flight emulation.", style);
        }
    }
}
