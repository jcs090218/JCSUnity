#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;

// Dave Hampson 
// http://wiki.unity3d.com/index.php?title=FramesPerSecond
public class FPSDisplay : MonoBehaviour
{

    private int m_fontSize = 4;     // Change the Font Size

    private float deltaTime = 0.0f;


    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }


    private void OnGUI()
    {


        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * m_fontSize / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
    

}
#endif
