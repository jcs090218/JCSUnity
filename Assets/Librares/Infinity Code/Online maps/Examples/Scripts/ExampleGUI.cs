/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsDemos
{
    [AddComponentMenu("")]
    // ReSharper disable once UnusedMember.Global
    public class ExampleGUI : MonoBehaviour
    {
        private OnlineMaps api;
        private GUIStyle rowStyle;
        private string search = "";

        // ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            api = GetComponent<OnlineMaps>();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            if (rowStyle == null)
            {
                rowStyle = new GUIStyle(GUI.skin.button);
                RectOffset margin = rowStyle.margin;
                rowStyle.margin = new RectOffset(margin.left, margin.right, 1, 1);
            }

            GUILayout.BeginArea(new Rect(5, 40, 30, 255), GUI.skin.box);

            if (GUILayout.Button("-")) api.zoom--;

            for (int i = 3; i < 21; i++)
                if (GUILayout.Button("", rowStyle, GUILayout.Height(10))) api.zoom = i;

            if (GUILayout.Button("+")) api.zoom++;

            GUILayout.EndArea();

            GUI.Box(new Rect(5, 5, Screen.width - 10, 30), "");
            GUI.Label(new Rect(10, 10, 100, 20), "Find place:");
            search = GUI.TextField(new Rect(80, 10, Screen.width - 200, 20), search);
            if (Event.current.type == EventType.KeyUp &&
                (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
                FindLocation();
            if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 20), "Search")) FindLocation();
        }

        private void FindLocation()
        {
            OnlineMapsFindLocation.Find(search).OnComplete += delegate(string s)
            {
                try
                {
                    Vector2 position = OnlineMapsFindLocation.GetCoordinatesFromResult(s);
                    if (position != Vector2.zero) OnlineMaps.instance.position = position;
                    else Debug.Log(s);
                }
                catch
                {
                    Debug.Log(s);
                }
            };
        }
    }
}