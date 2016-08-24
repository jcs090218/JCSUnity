/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class draws a closed polygon on the map.
/// </summary>
[Serializable]
public class OnlineMapsDrawingPoly : OnlineMapsDrawingElement
{
    /// <summary>
    /// Background color of the polygon.\n
    /// Note: Not supported in tileset.
    /// </summary>
    public Color backgroundColor = new Color(1, 1, 1, 0);

    /// <summary>
    /// Border color of the polygon.
    /// </summary>
    public Color borderColor = Color.black;

    /// <summary>
    /// Border weight of the polygon.
    /// </summary>
    public float borderWeight = 1;

    /// <summary>
    /// List of points of the polygon. Geographic coordinates.
    /// </summary>
    public List<Vector2> points;

    /// <summary>
    /// Center point of the polygon.
    /// </summary>
    public override Vector2 center
    {
        get
        {
            Vector2 centerPoint = Vector2.zero;
            foreach (Vector2 point in points) centerPoint += point;
            return centerPoint / points.Count;
        }
    }

    /// <summary>
    /// Creates a new polygon.
    /// </summary>
    public OnlineMapsDrawingPoly()
    {
        points = new List<Vector2>();
    }

    /// <summary>
    /// Creates a new polygon.
    /// </summary>
    /// <param name="points">List of points of the polygon. Geographic coordinates.</param>
    public OnlineMapsDrawingPoly(List<Vector2> points):this()
    {
        this.points = points;
    }

    /// <summary>
    /// Creates a new polygon.
    /// </summary>
    /// <param name="points">List of points of the polygon. Geographic coordinates.</param>
    /// <param name="borderColor">Border color of the polygon.</param>
    public OnlineMapsDrawingPoly(List<Vector2> points, Color borderColor)
        : this(points)
    {
        this.borderColor = borderColor;
    }

    /// <summary>
    /// Creates a new polygon.
    /// </summary>
    /// <param name="points">List of points of the polygon. Geographic coordinates.</param>
    /// <param name="borderColor">Border color of the polygon.</param>
    /// <param name="borderWeight">Border weight of the polygon.</param>
    public OnlineMapsDrawingPoly(List<Vector2> points, Color borderColor, float borderWeight)
        : this(points, borderColor)
    {
        this.borderWeight = borderWeight;
    }

    /// <summary>
    /// Creates a new polygon.
    /// </summary>
    /// <param name="points">List of points of the polygon. Geographic coordinates.</param>
    /// <param name="borderColor">Border color of the polygon.</param>
    /// <param name="borderWeight">Border weight of the polygon.</param>
    /// <param name="backgroundColor">
    /// Background color of the polygon.\n
    /// Note: Not supported in tileset.
    /// </param>
    public OnlineMapsDrawingPoly(List<Vector2> points, Color borderColor, float borderWeight, Color backgroundColor)
        : this(points, borderColor, borderWeight)
    {
        this.backgroundColor = backgroundColor;
    }

    public override void Draw(Color[] buffer, OnlineMapsVector2i bufferPosition, int bufferWidth, int bufferHeight, int zoom)
    {
        if (!visible) return;

        FillPoly(buffer, bufferPosition, bufferWidth, bufferHeight, zoom, points, backgroundColor);
        DrawLineToBuffer(buffer, bufferPosition, bufferWidth, bufferHeight, zoom, points, borderColor, borderWeight, true);
    }

    public override void DrawOnTileset(OnlineMapsTileSetControl control)
    {
        base.DrawOnTileset(control);

        if (!visible)
        {
            active = false;
            return;
        }

        InitMesh(control, "Poly", borderColor, backgroundColor);

        List<Vector3> verticles;
        List<Vector3> normals;
        List<int> triangles;
        List<Vector2> uv;
        InitLineMesh(points, control, out verticles, out normals, out triangles, out uv, borderWeight, true);

        mesh.Clear();
        mesh.subMeshCount = 2;
        mesh.vertices = verticles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();
        mesh.SetTriangles(triangles.ToArray(), 0);
    }

    public override bool HitTest(Vector2 positionLatLng, int zoom)
    {
        return OnlineMapsUtils.IsPointInPolygon(points, positionLatLng.x, positionLatLng.y);
    }
}