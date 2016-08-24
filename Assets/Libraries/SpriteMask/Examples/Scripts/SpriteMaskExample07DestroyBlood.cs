using UnityEngine;
using System.Collections;

public class SpriteMaskExample07DestroyBlood : MonoBehaviour
{
    private float startTime;
    public SpriteRenderer sr;
    
    void Start ()
    { 
        startTime = Time.time;
    }
    
    void Update ()
    {
        float delta = (Time.time - startTime) / 3f;
        
        if (delta > 1f) {
            enabled = false;
            Destroy (gameObject);
        } else {
            sr.color = new Color (1f, 1f, 1f, 1f - delta);
        }
    }
}

