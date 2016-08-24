using UnityEngine;
using System.Collections;

public class SpriteMaskExample07BloodController : MonoBehaviour
{
    public Sprite blood;
    private Camera cam;

    void Start ()
    {
        cam = Camera.main;
    }

    void Update ()
    {
        if (Input.GetMouseButtonUp (0)) {
            createBloodObject (cam.ScreenToWorldPoint (Input.mousePosition));
        }
    }

    void OnGUI() {
        GUIStyle style = GUI.skin.label;
        style.fontSize = 18;
        style.fontStyle = FontStyle.Bold;
        GUI.Label (new Rect (10, Screen.height - 30, 780, 30), "Click on trees and animals to produce blood (scene costs 3 draw calls + 1 for OnGUI).", style);

    }

    private void createBloodObject (Vector2 pos)
    {
//        print (Time.frameCount +" createBloodObject(): " + pos);

        GameObject go = new GameObject ("Blood");
        go.transform.parent = transform;
        go.transform.position = pos;

        SpriteRenderer sr = go.AddComponent <SpriteRenderer> ();
        sr.sprite = blood;
        sr.sortingOrder = 100;

        SpriteMaskExample07DestroyBlood db = go.AddComponent <SpriteMaskExample07DestroyBlood> ();
        db.sr = sr;

        SpriteMask.updateFor (go.transform);
    }
}
