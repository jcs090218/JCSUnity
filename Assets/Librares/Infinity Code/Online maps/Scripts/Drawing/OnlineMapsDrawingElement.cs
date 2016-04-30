/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Class implements the basic functionality of drawing on the map.
/// </summary>
[Serializable]
public class OnlineMapsDrawingElement
{
    private static readonly Vector2 halfVector = new Vector2(0.5f, 0.5f);
    private static readonly Vector2 sampleV1 = new Vector2(0.25f, 0.5f);
    private static readonly Vector2 sampleV2 = new Vector2(0.75f, 0.5f);
    private static readonly Vector2 sampleV3 = new Vector2(0.5f, 0.25f);
    private static readonly Vector2 sampleV4 = new Vector2(0.5f, 0.75f);

    /// <summary>
    /// Default event caused to draw tooltip.
    /// </summary>
    public static Action<OnlineMapsDrawingElement> OnElementDrawTooltip;

    /// <summary>
    /// Events that occur when user click on the drawing element.
    /// </summary>
    public Action<OnlineMapsDrawingElement> OnClick;

    /// <summary>
    /// Events that occur when user double click on the drawing element.
    /// </summary>
    public Action<OnlineMapsDrawingElement> OnDoubleClick;

    /// <summary>
    /// Event caused to draw tooltip.
    /// </summary>
    public Action<OnlineMapsDrawingElement> OnDrawTooltip;

    /// <summary>
    /// Events that occur when user press on the drawing element.
    /// </summary>
    public Action<OnlineMapsDrawingElement> OnPress;

    /// <summary>
    /// Events that occur when user release on the drawing element.
    /// </summary>
    public Action<OnlineMapsDrawingElement> OnRelease;

    /// <summary>
    /// In this variable you can put any data that you need to work with drawing element.
    /// </summary>
    public object customData;

    /// <summary>
    /// Tooltip that is displayed when user hover on the drawing element.
    /// </summary>
    public string tooltip;

    protected Mesh mesh;
    protected GameObject gameObject;
    protected bool _visible = true;

    private float bestElevationYScale;
    protected double tlx;
    protected double tly;
    protected double brx;
    protected double bry;

    protected static OnlineMaps api
    {
        get { return OnlineMaps.instance; }
    }

    protected virtual bool active
    {
        get
        {

            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }

    /// <summary>
    /// Center point of the drawing element.
    /// </summary>
    public virtual Vector2 center
    {
        get { return Vector2.zero; }
    }

    /// <summary>
    /// Gets or sets the visibility of the drawing element.
    /// </summary>
    public virtual bool visible
    {
        get { return _visible; }
        set
        {
            _visible = value;
            OnlineMaps.instance.needRedraw = true;
        }
    }

    protected OnlineMapsDrawingElement()
    {
        
    }

    /// <summary>
    /// Draw element on the map.
    /// </summary>
    /// <param name="buffer">Backbuffer</param>
    /// <param name="bufferPosition">Backbuffer position</param>
    /// <param name="bufferWidth">Backbuffer width</param>
    /// <param name="bufferHeight">Backbuffer height</param>
    /// <param name="zoom">Zoom of the map</param>
    public virtual void Draw(Color[] buffer, OnlineMapsVector2i bufferPosition, int bufferWidth, int bufferHeight, int zoom)
    {
        
    }

    protected void DrawActivePoints(OnlineMapsTileSetControl control, ref List<Vector2> activePoints, ref List<Vector3> verticles, ref List<Vector3> normals, ref List<int> triangles, ref List<Vector2> uv, float weight)
    {
        if (activePoints.Count < 2)
        {
            activePoints.Clear();
            return;
        }
        List<Vector3> side1 = new List<Vector3>();
        List<Vector3> side2 = new List<Vector3>();

        for (int i = 0; i < activePoints.Count; i++)
        {
            Vector3 p = new Vector3(-activePoints[i].x, 0, activePoints[i].y);
            if (i == 0)
            {
                float a = OnlineMapsUtils.Angle2DRad(p, new Vector3(-activePoints[i + 1].x, 0, activePoints[i + 1].y), 90);
                Vector3 off = new Vector3(Mathf.Cos(a) * weight, 0, Mathf.Sin(a) * weight);
                Vector3 s1 = p + off;
                Vector3 s2 = p - off;
                s1.y = control.GetElevationValue(s1.x, s1.z, bestElevationYScale, tlx, tly, brx, bry);
                s2.y = control.GetElevationValue(s2.x, s2.z, bestElevationYScale, tlx, tly, brx, bry);
                side1.Add(s1);
                side2.Add(s2);
            }
            else if (i == activePoints.Count - 1)
            {
                float a = OnlineMapsUtils.Angle2DRad(new Vector3(-activePoints[i - 1].x, 0, activePoints[i - 1].y), p, 90);
                Vector3 off = new Vector3(Mathf.Cos(a) * weight, 0, Mathf.Sin(a) * weight);
                Vector3 s1 = p + off;
                Vector3 s2 = p - off;
                s1.y = control.GetElevationValue(s1.x, s1.z, bestElevationYScale, tlx, tly, brx, bry);
                s2.y = control.GetElevationValue(s2.x, s2.z, bestElevationYScale, tlx, tly, brx, bry);
                side1.Add(s1);
                side2.Add(s2);
            }
            else
            {
                Vector3 p1 = new Vector3(-activePoints[i - 1].x, 0, activePoints[i - 1].y);
                Vector3 p2 = new Vector3(-activePoints[i + 1].x, 0, activePoints[i + 1].y);
                float a1 = OnlineMapsUtils.Angle2DRad(p1, p, 90);
                float a2 = OnlineMapsUtils.Angle2DRad(p, p2, 90);
                Vector3 off1 = new Vector3(Mathf.Cos(a1) * weight, 0, Mathf.Sin(a1) * weight);
                Vector3 off2 = new Vector3(Mathf.Cos(a2) * weight, 0, Mathf.Sin(a2) * weight);
                Vector3 p21 = p + off1;
                Vector3 p22 = p - off1;
                Vector3 p31 = p + off2;
                Vector3 p32 = p - off2;
                int state1, state2;
                Vector2 is1 = OnlineMapsUtils.GetIntersectionPointOfTwoLines(p1 + off1, p21, p31, p2 + off2, out state1);
                Vector2 is2 = OnlineMapsUtils.GetIntersectionPointOfTwoLines(p1 - off1, p22, p32, p2 - off2, out state2);
                if (state1 == 1) side1.Add(new Vector3(is1.x, control.GetElevationValue(is1.x, is1.y, bestElevationYScale, tlx, tly, brx, bry), is1.y));
                if (state2 == 1) side2.Add(new Vector3(is2.x, control.GetElevationValue(is2.x, is2.y, bestElevationYScale, tlx, tly, brx, bry), is2.y));
            }
        }

        for (int i = 0; i < Mathf.Min(side1.Count, side2.Count) - 1; i++)
        {
            int ti = verticles.Count;

            verticles.Add(side1[i]);
            verticles.Add(side1[i + 1]);
            verticles.Add(side2[i + 1]);
            verticles.Add(side2[i]);

            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);

            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 1));
            uv.Add(new Vector2(1, 0));

            triangles.Add(ti);
            triangles.Add(ti + 1);
            triangles.Add(ti + 2);
            triangles.Add(ti);
            triangles.Add(ti + 2);
            triangles.Add(ti + 3);
        }

        activePoints.Clear();
    }

    protected void DrawLineToBuffer(Color[] buffer, OnlineMapsVector2i bufferPosition, int bufferWidth, int bufferHeight,
        int zoom, List<Vector2> points, Color color, float weight, bool closed)
    {
        if (color.a == 0) return;

        double sx, sy;
        OnlineMapsUtils.LatLongToTiled(tlx, tly, zoom, out sx, out sy);

        int maxX = 1 << zoom;

        int off = closed ? 1 : 0;

        List<Vector2> localPoints = new List<Vector2>();

        double ppx = 0;

        for (int i = 0; i < points.Count + off; i++)
        {
            int ci = i;
            if (ci >= points.Count) ci -= points.Count;
            double px, py;
            OnlineMapsUtils.LatLongToTiled(points[ci].x, points[ci].y, zoom, out px, out py);

            px -= sx;
            py -= sy;

            if (i == 0)
            {
                if (px < maxX * -0.25) px += maxX;
                else if (px > maxX * 0.75) px -= maxX;
            }
            else
            {
                double gpx = px + maxX;
                double lpx = px - maxX;

                if (Math.Abs(ppx - gpx) < Math.Abs(ppx - px)) px = gpx;
                else if (Math.Abs(ppx - lpx) < Math.Abs(ppx - px)) px = lpx;
            }

            ppx = px;

            double rx1 = px + sx;
            double ry1 = py + sy;

            Vector2 np = new Vector2((float)rx1, (float)ry1);
            localPoints.Add(np);
        }

        int w = Mathf.RoundToInt(weight);

        for (int j = 0; j < localPoints.Count - 1; j++)
        {
            Vector2 p1 = localPoints[j] - bufferPosition;
            Vector2 p2 = localPoints[j + 1] - bufferPosition;

            if (p1.x > maxX && p2.x > maxX)
            {
                p1.x -= maxX;
                p2.x -= maxX;
            }

            Vector2 from = p1 * OnlineMapsUtils.tileSize;
            Vector2 to = p2 * OnlineMapsUtils.tileSize;

            float stY = Mathf.Clamp(Mathf.Min(from.y, to.y) - w, 0, bufferHeight);
            float stX = Mathf.Clamp(Mathf.Min(from.x, to.x) - w, 0, bufferWidth);
            float endY = Mathf.Clamp(Mathf.Max(from.y, to.y) + w, 0, bufferHeight);
            float endX = Mathf.Clamp(Mathf.Max(from.x, to.x) + w, 0, bufferWidth);
            int strokeOuter2 = w * w;

            int sqrW = w * w;

            int lengthX = Mathf.RoundToInt(endX - stX);
            int lengthY = Mathf.RoundToInt(endY - stY);
            Vector2 start = new Vector2(stX, stY);

            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    Vector2 p = new Vector2(x, y) + start;
                    Vector2 center = p + halfVector;
                    float dist = (center - center.NearestPointStrict(from, to)).sqrMagnitude;

                    if (dist <= strokeOuter2)
                    {
                        Color c = Color.black;

                        Vector2[] samples = {
                            p + sampleV1,
                            p + sampleV2,
                            p + sampleV3,
                            p + sampleV4
                        };
                        int bufferIndex = (int)p.y * bufferWidth + (int)p.x;
                        Color pc = buffer[bufferIndex];
                        for (int i = 0; i < 4; i++)
                        {
                            dist = (samples[i] - samples[i].NearestPointStrict(from, to)).sqrMagnitude;
                            if (dist < sqrW) c += Color.Lerp(pc, color, color.a);
                            else c += pc;
                        }
                        c /= 4;
                        buffer[bufferIndex] = c;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draws element on a specified TilesetControl.
    /// </summary>
    /// <param name="control"></param>
    public virtual void DrawOnTileset(OnlineMapsTileSetControl control)
    {
        
    }

    protected void FillPoly(Color[] buffer, OnlineMapsVector2i bufferPosition, int bufferWidth, int bufferHeight,
        int zoom, List<Vector2> points, Color color)
    {
        if (color.a == 0) return;

        List<Vector2> bufferPoints = new List<Vector2>();
        
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (Vector2 point in points)
        {
            Vector2 bufferPoint = (OnlineMapsUtils.LatLongToTilef(point, zoom) - bufferPosition) * OnlineMapsUtils.tileSize;

            if (bufferPoint.x < minX) minX = bufferPoint.x;
            if (bufferPoint.x > maxX) maxX = bufferPoint.x;
            if (bufferPoint.y < minY) minY = bufferPoint.y;
            if (bufferPoint.y > maxY) maxY = bufferPoint.y;
            
            bufferPoints.Add(bufferPoint);
        }

        float stY = Mathf.Clamp(minY, 0, bufferHeight);
        float stX = Mathf.Clamp(minX, 0, bufferWidth);
        float endY = Mathf.Clamp(maxY, 0, bufferHeight);
        float endX = Mathf.Clamp(maxX, 0, bufferWidth);

        int lengthX = Mathf.RoundToInt(endX - stX);
        int lengthY = Mathf.RoundToInt(endY - stY);
        Vector2 start = new Vector2(stX, stY);

        Color clr = new Color(color.r, color.g, color.b, 1);

        for (int y = 0; y < lengthY; y++)
        {
            float bufferY = y + start.y;
            for (int x = 0; x < lengthX; x++)
            {
                float bufferX = x + start.x;
                if (OnlineMapsUtils.IsPointInPolygon(bufferPoints, bufferX, bufferY))
                {
                    int bufferIndex = (int)bufferY * bufferWidth + (int)bufferX;
                    if (color.a == 1) buffer[bufferIndex] = color;
                    else buffer[bufferIndex] = Color.Lerp(buffer[bufferIndex], clr, color.a);
                }
            }
        }
    }

    protected List<Vector2> GetLocalPoints(List<Vector2> points, bool closed = false)
    {
        double sx, sy;
        OnlineMapsUtils.LatLongToTiled(tlx, tly, api.buffer.apiZoom, out sx, out sy);

        int maxX = 1 << api.zoom;

        int off = closed ? 1 : 0;

        List<Vector2> localPoints = new List<Vector2>();

        double ppx = 0;

        for (int i = 0; i < points.Count + off; i++)
        {
            int ci = i;
            if (ci >= points.Count) ci -= points.Count;
            double px, py;
            OnlineMapsUtils.LatLongToTiled(points[ci].x, points[ci].y, api.buffer.apiZoom, out px, out py);

            px -= sx;
            py -= sy;

            if (i == 0)
            {
                if (px < maxX * -0.25) px += maxX;
                else if (px > maxX * 0.75) px -= maxX;
            }
            else
            {
                double gpx = px + maxX;
                double lpx = px - maxX;

                if (Math.Abs(ppx - gpx) < Math.Abs(ppx - px)) px = gpx;
                else if (Math.Abs(ppx - lpx) < Math.Abs(ppx - px)) px = lpx;
            }

            ppx = px;

            double rx1 = (px * OnlineMapsUtils.tileSize) / api.tilesetWidth * api.tilesetSize.x;
            double ry1 = (py * OnlineMapsUtils.tileSize) / api.tilesetHeight * api.tilesetSize.y;

            Vector2 np = new Vector2((float)rx1, (float)ry1);
            localPoints.Add(np);
        }
        return localPoints;
    }

    /// <summary>
    /// Determines if the drawing element at the specified coordinates.
    /// </summary>
    /// <param name="positionLatLng">
    /// Position.
    /// </param>
    /// <param name="zoom">
    /// The zoom.
    /// </param>
    /// <returns>
    /// True if the drawing element in position, false if not.
    /// </returns>
    public virtual bool HitTest(Vector2 positionLatLng, int zoom)
    {
        return false;
    }

    protected void InitLineMesh(List<Vector2> points, OnlineMapsTileSetControl control, out List<Vector3> verticles, out List<Vector3> normals, out List<int> triangles, out List<Vector2> uv, float weight, bool closed = false)
    {
        api.GetTopLeftPosition(out tlx, out tly);
        api.GetBottomRightPosition(out brx, out bry);

        if (brx < tlx) brx += 360;

        List<Vector2> localPoints = GetLocalPoints(points, closed);

        List<Vector2> activePoints = new List<Vector2>();

        Rect mapRect = new Rect(0, 0, api.tilesetSize.x, api.tilesetSize.y);
        Vector2 lastPoint = Vector2.zero;

        Vector2 rightTop = new Vector2(api.tilesetSize.x, 0);
        Vector2 rightBottom = new Vector2(api.tilesetSize.x, api.tilesetSize.y);
        Vector2 leftBottom = new Vector2(0, api.tilesetSize.y);

        bestElevationYScale = control.GetBestElevationYScale(tlx, tly, brx, bry);

        verticles = new List<Vector3>();
        normals = new List<Vector3>();
        triangles = new List<int>();
        uv = new List<Vector2>();

        for (int i = 0; i < localPoints.Count; i++)
        {
            Vector2 p = localPoints[i];

            if (lastPoint != Vector2.zero)
            {
                Vector2 crossTop, crossLeft, crossBottom, crossRight;

                bool hasCrossTop = OnlineMapsUtils.LineIntersection(lastPoint, p, Vector2.zero, rightTop, out crossTop);
                bool hasCrossBottom = OnlineMapsUtils.LineIntersection(lastPoint, p, leftBottom, rightBottom, out crossBottom);
                bool hasCrossLeft = OnlineMapsUtils.LineIntersection(lastPoint, p, Vector2.zero, leftBottom, out crossLeft);
                bool hasCrossRight = OnlineMapsUtils.LineIntersection(lastPoint, p, rightTop, rightBottom, out crossRight);

                List<Vector2> intersections = new List<Vector2>();
                if (hasCrossTop) intersections.Add(crossTop);
                if (hasCrossBottom) intersections.Add(crossBottom);
                if (hasCrossLeft) intersections.Add(crossLeft);
                if (hasCrossRight) intersections.Add(crossRight);

                if (intersections.Count == 1) activePoints.Add(intersections[0]);
                else if (intersections.Count == 2)
                {
                    int minIndex = ((lastPoint - intersections[0]).magnitude < (lastPoint - intersections[1]).magnitude)? 0: 1;
                    activePoints.Add(intersections[minIndex]);
                    activePoints.Add(intersections[1 - minIndex]);
                }
            }

            if (mapRect.Contains(p)) activePoints.Add(p);
            else if (activePoints.Count > 0) DrawActivePoints(control, ref activePoints, ref verticles, ref normals, ref triangles, ref uv, weight);

            lastPoint = p;
        }

        if (activePoints.Count > 0) DrawActivePoints(control, ref activePoints, ref verticles, ref normals, ref triangles, ref uv, weight);
    }

    protected bool InitMesh(OnlineMapsTileSetControl control, string name, Color borderColor, Color backgroundColor = default(Color), Texture borderTexture = null, Texture backgroundTexture = null)
    {
        if (mesh != null) return false;
        gameObject = new GameObject(name);
        gameObject.transform.parent = control.drawingsGameObject.transform;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        mesh = new Mesh {name = name};
        meshFilter.mesh = mesh;
        renderer.materials = new Material[backgroundColor != default(Color)?2: 1];
        Shader shader = control.drawingShader;
        renderer.materials[0] = new Material(shader);
        renderer.materials[0].shader = shader;
        renderer.materials[0].color = borderColor;
        renderer.materials[0].mainTexture = borderTexture;

        if (backgroundColor != default(Color))
        {
            renderer.materials[1] = new Material(shader);
            renderer.materials[1].shader = shader;
            renderer.materials[1].color = backgroundColor;
            renderer.materials[1].mainTexture = backgroundTexture;
        }

        return true;
    }

    /// <summary>
    /// Method thats called when drawing element remove from map.
    /// </summary>
    public void OnRemoveFromMap()
    {
        if (mesh == null) return;

        Object.DestroyImmediate(gameObject);
        mesh = null;
    }
}