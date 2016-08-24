/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

public class OnlineMapsAboutWindow:EditorWindow
{
    [MenuItem("Component/Infinity Code/Online Maps/About")]
    public static void OpenWindow()
    {
        OnlineMapsAboutWindow window = GetWindow<OnlineMapsAboutWindow>(true, "About", true);
        window.minSize = new Vector2(200, 100);
        window.maxSize = new Vector2(200, 100);
    }

    public void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
        titleStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle textStyle = new GUIStyle(EditorStyles.label);
        textStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("Online Maps", titleStyle);
        GUILayout.Label("version " + OnlineMaps.version, textStyle);
        GUILayout.Label("created Infinity Code", textStyle);
        GUILayout.Label("2013-2016", textStyle);
    }
}