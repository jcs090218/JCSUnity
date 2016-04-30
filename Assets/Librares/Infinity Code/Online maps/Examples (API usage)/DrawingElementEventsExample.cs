/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/DrawingElementEventsExample")]
    public class DrawingElementEventsExample : MonoBehaviour
    {

        // Use this for initialization
        private void Start()
        {
            // Create a new rect element.
            OnlineMapsDrawingRect element = new OnlineMapsDrawingRect(-119.0807f, 34.58658f, 3, 3, Color.black, 1f,
                Color.blue);

            // Subscribe to events.
            element.OnClick += OnClick;
            element.OnPress += OnPress;
            element.OnRelease += OnRelease;
            element.OnDoubleClick += OnDoubleClick;
            OnlineMaps.instance.AddDrawingElement(element);

            List<Vector2> poly = new List<Vector2>
            {
                //Geographic coordinates
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(2, 2),
                new Vector2(0, 1)
            };

            // Create a new poly element.
            OnlineMapsDrawingPoly polyElement = new OnlineMapsDrawingPoly(poly, Color.red, 1f);

            // Subscribe to events.
            polyElement.OnClick += OnClick;
            polyElement.OnPress += OnPress;
            polyElement.OnRelease += OnRelease;
            polyElement.OnDoubleClick += OnDoubleClick;
            OnlineMaps.instance.AddDrawingElement(polyElement);

            // Create tooltip for poly.
            polyElement.tooltip = "Drawing Element";
        }

        private void OnDoubleClick(OnlineMapsDrawingElement onlineMapsDrawingElement)
        {
            Debug.Log("OnDoubleClick");
        }

        private void OnRelease(OnlineMapsDrawingElement onlineMapsDrawingElement)
        {
            Debug.Log("OnRelease");
        }

        private void OnPress(OnlineMapsDrawingElement onlineMapsDrawingElement)
        {
            Debug.Log("OnPress");
        }

        private void OnClick(OnlineMapsDrawingElement onlineMapsDrawingElement)
        {
            Debug.Log("OnClick");
        }
    }
}