/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that draws a line on the map.
/// </summary>
[Serializable]
public class OnlineMapsDrawingLine : OnlineMapsDrawingElement
{
    /// <summary>
    /// Color of the line.
    /// </summary>
    public Color color = Color.black;

    /// <summary>
    /// List of points of the line. Geographic coordinates.
    /// </summary>
    public List<Vector2> points;

    /// <summary>
    /// Texture of line.\n
    /// Uses only in tileset.
    /// </summary>
    public Texture2D texture;

    /// <summary>
    /// Weight of the line.
    /// </summary>
    public float weight = 1;

    /// <summary>
    /// Creates a new line.
    /// </summary>
    public OnlineMapsDrawingLine()
    {
        points = new List<Vector2>();
    }

    /// <summary>
    /// Creates a new line.
    /// </summary>
    /// <param name="points">List of points of the line. Geographic coordinates.</param>
    public OnlineMapsDrawingLine(List<Vector2> points):this()
    {
        this.points = points;
    }

    /// <summary>
    /// Creates a new line.
    /// </summary>
    /// <param name="points">List of points of the line. Geographic coordinates.</param>
    /// <param name="color">Color of the line.</param>
    public OnlineMapsDrawingLine(List<Vector2> points, Color color):this(points)
    {
        this.color = color;
    }

    /// <summary>
    /// Creates a new line.
    /// </summary>
    /// <param name="points">List of points of the line. Geographic coordinates.</param>
    /// <param name="color">Color of the line.</param>
    /// <param name="weight">Weight of the line.</param>
    public OnlineMapsDrawingLine(List<Vector2> points, Color color, float weight) : this(points, color)
    {
        this.weight = weight;
    }

    public override void Draw(Color[] buffer, OnlineMapsVector2i bufferPosition, int bufferWidth, int bufferHeight, int zoom)
    {
        if (!visible) return;

        DrawLineToBuffer(buffer, bufferPosition, bufferWidth, bufferHeight, zoom, points, color, weight, false);
    }

    public override void DrawOnTileset(OnlineMapsTileSetControl control)
    {
        base.DrawOnTileset(control);

        if (!visible)
        {
            active = false;
            return;
        }

        InitMesh(control, "Line", color, default(Color), texture);

        List<Vector3> verticles;
        List<Vector3> normals;
        List<int> triangles;
        List<Vector2> uv;
        InitLineMesh(points, control, out verticles, out normals, out triangles, out uv, weight);

        mesh.Clear();
        mesh.vertices = verticles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();
        mesh.SetTriangles(triangles.ToArray(), 0);
    }
}