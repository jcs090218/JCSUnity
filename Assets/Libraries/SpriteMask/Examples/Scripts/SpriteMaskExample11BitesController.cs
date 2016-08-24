using UnityEngine;
using System.Collections;

public class SpriteMaskExample11BitesController : MonoBehaviour {

    public Sprite circle;
    private Camera cam;
    
    void Start ()
    {
        cam = Camera.main;
    }
    
    void Update ()
    {
        if (Input.GetMouseButtonUp (0)) {
            createBiteObject (cam.ScreenToWorldPoint (Input.mousePosition));
        }
    }
    
    void OnGUI() {
        GUIStyle style = GUI.skin.label;
        style.fontSize = 18;
        style.fontStyle = FontStyle.Bold;
        GUI.Label (new Rect (10, Screen.height - 30, 780, 30), "Click on the tree to eat :).", style);
        
    }
    
    private void createBiteObject (Vector2 pos)
    {
        // Create bite object which will take pare in inverted masking
        GameObject go = new GameObject ("Bite");
        go.transform.parent = transform;
        go.transform.position = pos;
        go.AddComponent<SpriteMaskingPart>();
        
        SpriteRenderer sr = go.AddComponent <SpriteRenderer> ();
        sr.sprite = circle;
        sr.sortingOrder = 1;

        // Find SpriteMask in parents od go.transform and update it
        SpriteMask.updateFor(go.transform);
    }
}
