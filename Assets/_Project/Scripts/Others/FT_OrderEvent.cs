/**
 * $File: FT_OrderEvent.cs $
 * $Date: 2020-06-20 21:34:02 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Test the component `JCS_OrderEvent`.
/// </summary>
[RequireComponent(typeof(JCS_OrderEvent))]
public class FT_OrderEvent : MonoBehaviour
{
    /* Variables */

    private JCS_OrderEvent mOrderEvent = null;

    [Tooltip("Text to print for testing.")]
    public string word = "Hello test!!~~";

    [Tooltip("Count to print the text.")]
    public int printCount = 5;

    [Tooltip("Interval time for each print.")]
    [Range(0.0f, 10.0f)]
    public float intervalTime = 1.0f;

    private int mCounter = 0;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        mOrderEvent = this.GetComponent<JCS_OrderEvent>();

        mOrderEvent.StartEvent(intervalTime, () => {
            Debug.Log(word);

            ++mCounter;

            // Check for termination.
            if (mCounter == printCount)
                mOrderEvent.DoneEvent();
        });
    }
}
