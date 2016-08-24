/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/DrawingAPI_Example")]
    public class DrawingAPI_Example : MonoBehaviour
    {
        private void Start()
        {
            OnlineMaps api = OnlineMaps.instance;

            List<Vector2> line = new List<Vector2>
            {
                //Geographic coordinates
                new Vector2(3, 3),
                new Vector2(5, 3),
                new Vector2(4, 4),
                new Vector2(9.3f, 6.5f)
            };

            List<Vector2> poly = new List<Vector2>
            {
                //Geographic coordinates
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(2, 2),
                new Vector2(0, 1)
            };

            // Draw line
            api.AddDrawingElement(new OnlineMapsDrawingLine(line, Color.green, 5));

            // Draw filled transparent poly
            api.AddDrawingElement(new OnlineMapsDrawingPoly(poly, Color.red, 1, new Color(1, 1, 1, 0.5f)));

            // Draw filled rectangle
            // (position, size, borderColor, borderWeight, backgroundColor)
            api.AddDrawingElement(new OnlineMapsDrawingRect(new Vector2(2, 2), new Vector2(1, 1), Color.green, 1,
                Color.blue));
        }
    }
}