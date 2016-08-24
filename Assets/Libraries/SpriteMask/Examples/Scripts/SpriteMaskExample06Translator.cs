using UnityEngine;
using System.Collections;

public class SpriteMaskExample06Translator : MonoBehaviour {

    public Transform target;

    public Vector2 min;
    public Vector2 max;

    private Vector2 basePosition;
    private Vector2 deltaPosition;

    void Start () {
        basePosition = target.localPosition;
        deltaPosition = max - min;
    }

    void Update () {
        float delta = Mathf.Sin (Time.time);
        Vector2 d = deltaPosition;
        d.Scale (new Vector2 (delta, delta));
        target.localPosition = basePosition + d;
    }
}
