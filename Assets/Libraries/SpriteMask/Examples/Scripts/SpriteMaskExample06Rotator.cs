using UnityEngine;
using System.Collections;

public class SpriteMaskExample06Rotator : MonoBehaviour {

    public Transform[] clockwise;
    public Transform[] counterclockwise;

    void Update () {
        float delta = Time.deltaTime * 10f;

        for (int i=0;i<clockwise.Length;i++) {
            clockwise [i].Rotate (0f, 0f, delta);
        }

        for (int i=0;i<counterclockwise.Length;i++) {
            counterclockwise [i].Rotate (0f, 0f, -delta);
        }

    }
}
