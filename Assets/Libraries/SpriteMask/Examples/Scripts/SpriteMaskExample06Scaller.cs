using UnityEngine;
using System.Collections;

public class SpriteMaskExample06Scaller : MonoBehaviour
{

    public Transform target;

    void Update ()
    {
        float delta = 0.2f * Mathf.Sin (Time.time);

        target.localScale = new Vector3 (1f - delta, 1f + delta, 0f);

    }
}
