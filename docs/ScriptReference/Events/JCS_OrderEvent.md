# JCS_OrderEvent

Execute an operation in order with the interval of time.

## Variables

| Name           | Description              |
|:---------------|:-------------------------|
| IntervalTime   | Time for each execution. |
| mTimeType      | Type of the delta time.  |
| unityExecution | Unity execution event.   |

## Functions

| Name       | Description                              |
|:-----------|:-----------------------------------------|
| StartEvent | Start a new event with interval of time. |
| DoneEvent  | Terminate the current event loop.        |

## Example

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;

/// <summary>
/// Test the component `JCS_OrderEvent`.
/// </summary>
[RequireComponent(typeof(JCS_OrderEvent))]
public class FT_OrderEvent_Test 
    : MonoBehaviour 
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
```
