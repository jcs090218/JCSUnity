/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

#if NGUI

using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/NGUICustomMarkerExample")]
    public class NGUICustomMarkerExample : MonoBehaviour
    {
        public Vector2 position;
        public Vector2 size = new Vector2(32, 32);

        public UIWidget widget
        {
            get { return GetComponent<UIWidget>(); }
        }
    }
}

#endif