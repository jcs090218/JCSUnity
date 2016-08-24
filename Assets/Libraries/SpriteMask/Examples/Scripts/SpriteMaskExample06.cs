using UnityEngine;
using System.Collections;

public class SpriteMaskExample06 : MonoBehaviour
{
    public SpriteMask mask1;
    public SpriteMask mask2;

    void OnGUI ()
    {
        GUI.color = Color.white;


        GUIStyle style = GUI.skin.button;
        style.fontStyle = FontStyle.Bold;
        if (GUI.Button (new Rect (10, 10, 150, 30), mask1.enabled ? "Outer mask OFF" : "Outer mask ON", style)) {
            mask1.enabled = !mask1.enabled;
        }
        if (GUI.Button (new Rect (10, 70, 150, 30), mask2.enabled ? "Inner mask OFF" : "Inner mask ON", style)) {
            mask2.enabled = !mask2.enabled;
        }

        style = GUI.skin.toggle;
        style.fontStyle = FontStyle.Bold;
        mask1.inverted = GUI.Toggle (new Rect (10, 40, 200, 20), mask1.inverted , "Outer inverted (1)", style);
        mask2.inverted = GUI.Toggle (new Rect (10, 100, 200, 20), mask2.inverted , "Inner inverted (1)", style);

        style = GUI.skin.label;
        style.fontStyle = FontStyle.Bold;
        GUI.Label (new Rect (10, Screen.height - 20, 600, 20), "(1) 'Inverted' is an experimental feature. It may not work properly in all cases.", style);
    }
}
