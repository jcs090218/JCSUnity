/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Helper class, which contains all the basic methods.
/// </summary>
public static class OnlineMapsUtils
{
    /// <summary>
    /// Arcseconds in meters.
    /// </summary>
    public const float angleSecond = 1 / 3600f;

    /// <summary>
    /// Maximal distance of raycast.
    /// </summary>
    public const int maxRaycastDistance = 100000;

    /// <summary>
    /// Earth radius.
    /// </summary>
    public const double R = 6371;

    /// <summary>
    /// Degrees-to-radians conversion constant.
    /// </summary>
    public const double Deg2Rad = Math.PI / 180;

    /// <summary>
    /// Bytes per megabyte.
    /// </summary>
    public const int mb = 1024 * 1024;

    /// <summary>
    /// PI * 4
    /// </summary>
    public const float pi4 = 4 * Mathf.PI;

    /// <summary>
    /// Size of the tile texture in pixels.
    /// </summary>
    public const short tileSize = 256;

    /// <summary>
    /// The second in ticks.
    /// </summary>
    public const long second = 10000000;

    /// <summary>
    /// tileSize squared, to accelerate the calculations.
    /// </summary>
    public const int sqrTileSize = tileSize * tileSize;

    /// <summary>
    /// The angle between the two points in degree.
    /// </summary>
    /// <param name="point1">Point 1</param>
    /// <param name="point2">Point 2</param>
    /// <returns>Angle in degree</returns>
    public static float Angle2D(Vector2 point1, Vector2 point2)
    {
        return Mathf.Atan2((point2.y - point1.y), (point2.x - point1.x)) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// The angle between the two points in degree.
    /// </summary>
    /// <param name="point1">Point 1</param>
    /// <param name="point2">Point 2</param>
    /// <returns>Angle in degree</returns>
    public static float Angle2D(Vector3 point1, Vector3 point2)
    {
        return Mathf.Atan2((point2.z - point1.z), (point2.x - point1.x)) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// The angle between the three points in degree.
    /// </summary>
    /// <param name="point1">Point 1</param>
    /// <param name="point2">Point 2</param>
    /// <param name="point3">Point 3</param>
    /// <param name="unsigned">Return a positive result.</param>
    /// <returns>Angle in degree</returns>
    public static float Angle2D(Vector3 point1, Vector3 point2, Vector3 point3, bool unsigned = true)
    {
        float angle1 = Angle2D(point1, point2);
        float angle2 = Angle2D(point2, point3);
        float angle = angle1 - angle2;
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 360;
        if (unsigned) angle = Mathf.Abs(angle);
        return angle;
    }

    /// <summary>
    /// The angle between the two points in radians.
    /// </summary>
    /// <param name="point1">Point 1</param>
    /// <param name="point2">Point 2</param>
    /// <param name="offset">Result offset in degrees.</param>
    /// <returns>Angle in radians</returns>
    public static float Angle2DRad(Vector3 point1, Vector3 point2, float offset = 0)
    {
        return Mathf.Atan2((point2.z - point1.z), (point2.x - point1.x)) + offset * Mathf.Deg2Rad;
    }

    public static void ApplyColorArray(ref Color[] result, int px, int py, int width, int height, ref Color[] color, int rx, int ry)
    {
        const int s = tileSize;
        int hs = s / 2;
        for (int cy = 0; cy < height; cy++)
        {
            int scy = (cy + py) * s;
            int hcy = (cy + ry) * hs + rx;
            int scpy = scy + px;
            for (int cx = 0; cx < width; cx++) result[scpy + cx] = color[hcy + cx];
        }
    }

    public static void ApplyColorArray2(ref Color[] result, int px, int py, int width, int height, ref Color[] color)
    {
        const int s = tileSize;
        int hs = s / 2 * 2;
        for (int cy = 0; cy < height; cy++)
        {
            int scy = (cy + py) * s;
            int hcy = cy * hs;
            int scpy = scy + px;
            for (int cx = scpy, cx2 = hcy; cx < width + scpy; cx++, cx2 += 2) result[cx] = color[cx2];
        }
    }

    /// <summary>
    /// Clamps a value between a minimum double and maximum double value.
    /// </summary>
    /// <param name="n">Value</param>
    /// <param name="minValue">Minimum</param>
    /// <param name="maxValue">Maximum</param>
    /// <returns>Value between a minimum and maximum.</returns>
    public static double Clip(double n, double minValue, double maxValue)
    {
        if (n < minValue) return minValue;
        if (n > maxValue) return maxValue;
        return n;
    }

    public static Vector2 Crossing(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        if (p3.x == p4.x)
        {
            float y = p1.y + ((p2.y - p1.y) * (p3.x - p1.x)) / (p2.x - p1.x);
            if (y > Mathf.Max(p3.y, p4.y) || y < Mathf.Min(p3.y, p4.y) || y > Mathf.Max(p1.y, p2.y) || y < Mathf.Min(p1.y, p2.y)) return Vector2.zero;
            Debug.Log("Cross Vertical");
            return new Vector2(p3.x, y);
        }
        float x = p1.x + ((p2.x - p1.x) * (p3.y - p1.y)) / (p2.y - p1.y);
        if (x > Mathf.Max(p3.x, p4.x) || x < Mathf.Min(p3.x, p4.x) || x > Mathf.Max(p1.x, p2.x) || x < Mathf.Min(p1.x, p2.x)) return Vector2.zero;
        return new Vector2(x, p3.y);
    }

    /// <summary>
    /// The distance between two geographical coordinates.
    /// </summary>
    /// <param name="point1">Coordinate (X - Lng, Y - Lat)</param>
    /// <param name="point2">Coordinate (X - Lng, Y - Lat)</param>
    /// <returns>Distance (km).</returns>
    public static Vector2 DistanceBetweenPoints(Vector2 point1, Vector2 point2)
    {
        double scfY = Math.Sin(point1.y * Deg2Rad);
        double sctY = Math.Sin(point2.y * Deg2Rad);
        double ccfY = Math.Cos(point1.y * Deg2Rad);
        double cctY = Math.Cos(point2.y * Deg2Rad);
        double cX = Math.Cos((point1.x - point2.x) * Deg2Rad);
        double sizeX1 = Math.Abs(R * Math.Acos(scfY * scfY + ccfY * ccfY * cX));
        double sizeX2 = Math.Abs(R * Math.Acos(sctY * sctY + cctY * cctY * cX));
        float sizeX = (float)((sizeX1 + sizeX2) / 2.0);
        float sizeY = (float)(R * Math.Acos(scfY * sctY + ccfY * cctY));
        if (float.IsNaN(sizeY)) sizeY = 0;
        return new Vector2(sizeX, sizeY);
    }

    /// <summary>
    /// The distance between two geographical coordinates.
    /// </summary>
    /// <param name="x1">Longitude 1.</param>
    /// <param name="y1">Latitude 1.</param>
    /// <param name="x2">Longitude 2.</param>
    /// <param name="y2">Latitude 2.</param>
    /// <param name="dx">Distance longitude (km).</param>
    /// <param name="dy">Distance latitude (km).</param>
    public static void DistanceBetweenPoints(double x1, double y1, double x2, double y2, out double dx, out double dy)
    {
        double scfY = Math.Sin(y1 * Deg2Rad);
        double sctY = Math.Sin(y2 * Deg2Rad);
        double ccfY = Math.Cos(y1 * Deg2Rad);
        double cctY = Math.Cos(y2 * Deg2Rad);
        double cX = Math.Cos((x1 - x2) * Deg2Rad);
        double sizeX1 = Math.Abs(R * Math.Acos(scfY * scfY + ccfY * ccfY * cX));
        double sizeX2 = Math.Abs(R * Math.Acos(sctY * sctY + cctY * cctY * cX));
        dx = (sizeX1 + sizeX2) / 2.0;
        dy = R * Math.Acos(scfY * sctY + ccfY * cctY);
        if (double.IsNaN(dy)) dy = 0;
    }

    /// <summary>
    /// The distance between two geographical coordinates.
    /// </summary>
    /// <param name="point1">Coordinate (X - Lng, Y - Lat)</param>
    /// <param name="point2">Coordinate (X - Lng, Y - Lat)</param>
    /// <returns>Distance (km).</returns>
    public static double DistanceBetweenPointsD(Vector2 point1, Vector2 point2)
    {
        double scfY = Math.Sin(point1.y * Deg2Rad);
        double sctY = Math.Sin(point2.y * Deg2Rad);
        double ccfY = Math.Cos(point1.y * Deg2Rad);
        double cctY = Math.Cos(point2.y * Deg2Rad);
        double cX = Math.Cos((point1.x - point2.x) * Deg2Rad);
        double sizeX1 = Math.Abs(R * Math.Acos(scfY * scfY + ccfY * ccfY * cX));
        double sizeX2 = Math.Abs(R * Math.Acos(sctY * sctY + cctY * cctY * cX));
        double sizeX = ((sizeX1 + sizeX2) / 2.0);
        double sizeY = (R * Math.Acos(scfY * sctY + ccfY * cctY));
        if (double.IsNaN(sizeY)) sizeY = 0;
        return Math.Sqrt(sizeX * sizeX + sizeY * sizeY);
    }

    /// <summary>
    /// Fix geographic coordinates.
    /// </summary>
    /// <param name="v">Coordinates for fix.</param>
    /// <returns>Correct geographic coordinates.</returns>
    public static Vector2 FixAngle(this Vector2 v)
    {
        float y = v.y;
        if (y < -90) y = -90;
        else if (y > 90) y = 90;
        return new Vector2(Mathf.Repeat(v.x + 180, 360) - 180, y);
    }

    /// <summary>
    /// Flip the negative dimensions of the rect.
    /// </summary>
    /// <param name="r">Rect.</param>
    public static void FlipNegative(this Rect r)
    {
        if (r.width < 0) r.x -= (r.width *= -1);
        if (r.height < 0) r.y -= (r.height *= -1);
    }

    /// <summary>
    /// Get the center point and best zoom for the array of markers.
    /// </summary>
    /// <param name="markers">Array of markers.</param>
    /// <param name="center">Center point.</param>
    /// <param name="zoom">Best zoom.</param>
    public static void GetCenterPointAndZoom (OnlineMapsMarkerBase[] markers, out Vector2 center, out int zoom)
    {
        float minX = Single.MaxValue;
        float minY = Single.MaxValue;
        float maxX = Single.MinValue;
        float maxY = Single.MinValue;

        foreach (OnlineMapsMarkerBase marker in markers)
        {
            if (marker.position.x < minX) minX = marker.position.x;
            if (marker.position.y < minY) minY = marker.position.y;
            if (marker.position.x > maxX) maxX = marker.position.x;
            if (marker.position.y > maxY) maxY = marker.position.y;
        }

        float rx = maxX - minX;
        float ry = maxY - minY;
        center = new Vector2(rx / 2 + minX, ry / 2 + minY);

        int width = OnlineMaps.instance.width;
        int height = OnlineMaps.instance.height;

        float countX = width / (float)tileSize / 2;
        float countY = height / (float)tileSize / 2;

        bool useZoomMin = false;

        for (int z = 20; z > 4; z--)
        {
            bool success = true;

            foreach (OnlineMapsMarkerBase marker in markers)
            {
                Vector2 p = LatLongToTilef(marker.position, z);
                Vector2 bufferPosition = LatLongToTilef(center, z);
                p.x -= bufferPosition.x - countX;
                p.y -= bufferPosition.y - countY;

                if (marker is OnlineMapsMarker)
                {
                    useZoomMin = true;
                    OnlineMapsMarker m = marker as OnlineMapsMarker;
                    OnlineMapsVector2i ip = m.GetAlignedPosition(new OnlineMapsVector2i((int)(p.x * tileSize), (int)(p.y * tileSize)));
                    if (ip.x < 0 || ip.y < 0 || ip.x + m.width > width || ip.y + m.height > height)
                    {
                        success = false;
                        break;
                    }
                }
                else if (marker is OnlineMapsMarker3D)
                {
                    if (p.x < 0 || p.y < 0 || p.x > width || p.y > height)
                    {
                        success = false;
                        break;
                    }
                }
                else
                {
                    throw new Exception("Wrong marker type");
                }
            }
            if (success)
            {
                zoom = z;
                if (useZoomMin) zoom -= 1;
                return;
            }
        }

        zoom = 3;
    }

    /// <summary>
    /// Get the center point and best zoom for the array of coordinates.
    /// </summary>
    /// <param name="positions">Array of coordinates</param>
    /// <param name="center">Center coordinate</param>
    /// <param name="zoom">Best zoom</param>
    public static void GetCenterPointAndZoom(Vector2[] positions, out Vector2 center, out int zoom)
    {
        float minX = Single.MaxValue;
        float minY = Single.MaxValue;
        float maxX = Single.MinValue;
        float maxY = Single.MinValue;

        foreach (Vector2 p in positions)
        {
            if (p.x < minX) minX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.x > maxX) maxX = p.x;
            if (p.y > maxY) maxY = p.y;
        }

        float rx = maxX - minX;
        float ry = maxY - minY;
        center = new Vector2(rx / 2 + minX, ry / 2 + minY);

        int width = OnlineMaps.instance.width;
        int height = OnlineMaps.instance.height;

        float countX = width / (float)tileSize / 2;
        float countY = height / (float)tileSize / 2;

        for (int z = 20; z > 4; z--)
        {
            bool success = true;

            foreach (Vector2 pos in positions)
            {
                Vector2 p = LatLongToTilef(pos, z);
                Vector2 bufferPosition = LatLongToTilef(center, z);
                p.x -= bufferPosition.x - countX;
                p.y -= bufferPosition.y - countY;

                if (p.x < 0 || p.y < 0 || p.x > width || p.y > height)
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                zoom = z;
                return;
            }
        }

        zoom = 3;
    }

    public static Vector2 GetIntersectionPointOfTwoLines(Vector2 p11, Vector2 p12, Vector2 p21, Vector2 p22,
        out int state)
    {
        state = -2;
        Vector2 result = new Vector2();
        float m = ((p22.x - p21.x) * (p11.y - p21.y) - (p22.y - p21.y) * (p11.x - p21.x));
        float n = ((p22.y - p21.y) * (p12.x - p11.x) - (p22.x - p21.x) * (p12.y - p11.y));

        float Ua = m / n;

        if (n == 0 && m != 0) state = -1;
        else if (m == 0 && n == 0) state = 0;
        else
        {
            result.x = p11.x + Ua * (p12.x - p11.x);
            result.y = p11.y + Ua * (p12.y - p11.y);

            if (((result.x >= p11.x || result.x <= p11.x) && (result.x >= p21.x || result.x <= p21.x))
                && ((result.y >= p11.y || result.y <= p11.y) && (result.y >= p21.y || result.y <= p21.y))) state = 1;
        }
        return result;
    }

    public static Vector2 GetIntersectionPointOfTwoLines(Vector3 p11, Vector3 p12, Vector3 p21, Vector3 p22,
        out int state)
    {
        return GetIntersectionPointOfTwoLines(new Vector2(p11.x, p11.z), new Vector2(p12.x, p12.z),
            new Vector2(p21.x, p21.z), new Vector2(p22.x, p22.z), out state);
    }

    public static Vector2 GetLatLng(this OnlineMapsXML node, string subNodeName)
    {
        OnlineMapsXML subNode = node[subNodeName];
        return new Vector2(subNode.Get<float>("lng"), subNode.Get<float>("lat"));
    }

    /// <summary>
    /// Gets Webplayes safe URL.
    /// </summary>
    /// <param name="url">Original URL.</param>
    /// <returns>Webplayer safe URL.</returns>
    public static WWW GetWWW(string url)
    {
#if UNITY_WEBPLAYER || UNITY_WEBGL
        return new WWW(OnlineMaps.instance.webplayerProxyURL + url);
#else
        return new WWW(url);
#endif
    }

    public static Color HexToColor(string hex)
    {
        byte r = Byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = Byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte b = Byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

    public static bool Intersect(this Rect a, Rect b)
    {
        a.FlipNegative();
        b.FlipNegative();
        if (a.xMin >= b.xMax) return false;
        if (a.xMax <= b.xMin) return false;
        if (a.yMin >= b.yMax) return false;
        if (a.yMax <= b.yMin) return false;

        return true;
    }

    public static bool LineIntersection(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 out_intersection)
    {
        out_intersection = Vector2.zero;

        Vector2 dir1 = end1 - start1;
        Vector2 dir2 = end2 - start2;

        //считаем уравнения прямых проходящих через отрезки
        float a1 = -dir1.y;
        float b1 = +dir1.x;
        float d1 = -(a1 * start1.x + b1 * start1.y);

        float a2 = -dir2.y;
        float b2 = +dir2.x;
        float d2 = -(a2 * start2.x + b2 * start2.y);

        //подставляем концы отрезков, для выяснения в каких полуплоскотях они
        float seg1_line2_start = a2 * start1.x + b2 * start1.y + d2;
        float seg1_line2_end = a2 * end1.x + b2 * end1.y + d2;

        float seg2_line1_start = a1 * start2.x + b1 * start2.y + d1;
        float seg2_line1_end = a1 * end2.x + b1 * end2.y + d1;

        //если концы одного отрезка имеют один знак, значит он в одной полуплоскости и пересечения нет.
        if (seg1_line2_start * seg1_line2_end >= 0 || seg2_line1_start * seg2_line1_end >= 0) return false;

        float u = seg1_line2_start / (seg1_line2_start - seg1_line2_end);
        out_intersection = start1 + u * dir1;

        return true;
    }

    public static Vector2 LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        double Ax, Bx, Cx, Ay, By, Cy, d, e, f, num, offset;
        double x1lo, x1hi, y1lo, y1hi;

        Ax = p2.x - p1.x;
        Bx = p3.x - p4.x;

        if (Ax < 0)
        {
            x1lo = p2.x; 
            x1hi = p1.x;
        }
        else
        {
            x1hi = p2.x; 
            x1lo = p1.x;
        }

        if (Bx > 0)
        {
            if (x1hi < p4.x || p3.x < x1lo) return Vector2.zero;
        }
        else
        {
            if (x1hi < p3.x || p4.x < x1lo) return Vector2.zero;
        }

        Ay = p2.y - p1.y;
        By = p3.y - p4.y;

        if (Ay < 0)
        {
            y1lo = p2.y; 
            y1hi = p1.y;
        }
        else
        {
            y1hi = p2.y; 
            y1lo = p1.y;
        }

        if (By > 0)
        {
            if (y1hi < p4.y || p3.y < y1lo) return Vector2.zero;
        }
        else
        {
            if (y1hi < p3.y || p4.y < y1lo) return Vector2.zero;
        }

        Cx = p1.x - p3.x;
        Cy = p1.y - p3.y;
        d = By * Cx - Bx * Cy;
        f = Ay * Bx - Ax * By;

        if (f > 0)
        {
            if (d < 0 || d > f) return Vector2.zero;
        }
        else
        {
            if (d > 0 || d < f) return Vector2.zero;
        }

        e = Ax * Cy - Ay * Cx;

        if (f > 0)
        {
            if (e < 0 || e > f) return Vector2.zero;
        }
        else
        {
            if (e > 0 || e < f) return Vector2.zero;
        }

        if (f == 0) return Vector2.zero;

        Vector2 intersection;

        num = d * Ax;
        offset = same_sign(num, f) ? f * 0.5 : -f * 0.5;
        intersection.x = (float)(p1.x + (num + offset) / f);

        num = d * Ay;
        offset = same_sign(num, f) ? f * 0.5 : -f * 0.5;
        intersection.y = (float)(p1.y + (num + offset) / f);

        return intersection;
    }

    private static bool same_sign(float a, float b)
    {
        return ((a * b) >= 0f);
    }

    private static bool same_sign(double a, double b)
    {
        return ((a * b) >= 0f);
    }

    public static bool IsPointInPolygon(List<Vector2> poly, float x, float y)
    {
        int i, j;
        bool c = false;
        for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
        {
            if (((poly[i].y <= y && y < poly[j].y) || (poly[j].y <= y && y < poly[i].y)) && 
                x < (poly[j].x - poly[i].x) * (y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)
                c = !c;
        }
        return c;
    }

    /// <summary>
    /// Converts geographic coordinates to Mercator coordinates.
    /// </summary>
    /// <param name="x">Longitude</param>
    /// <param name="y">Latitude</param>
    /// <returns>Mercator coordinates</returns>
    public static Vector2 LatLongToMercat(float x, float y)
    {
        float sy = Mathf.Sin(y * Mathf.Deg2Rad);
        return new Vector2((x + 180) / 360, 0.5f - Mathf.Log((1 + sy) / (1 - sy)) / pi4);
    }

    /// <summary>
    /// Converts geographic coordinates to Mercator coordinates.
    /// </summary>
    /// <param name="x">Longitude</param>
    /// <param name="y">Latitude</param>
    public static void LatLongToMercat(ref float x, ref float y)
    {
        float sy = Mathf.Sin(y * Mathf.Deg2Rad);
        x = (x + 180) / 360;
        y = 0.5f - Mathf.Log((1 + sy) / (1 - sy)) / pi4;
    }

    /// <summary>
    /// Converts geographic coordinates to Mercator coordinates.
    /// </summary>
    /// <param name="x">Longitude</param>
    /// <param name="y">Latitude</param>
    public static void LatLongToMercat(ref double x, ref double y)
    {
        double sy = Math.Sin(y * Deg2Rad);
        x = (x + 180) / 360;
        y = 0.5 - Math.Log((1 + sy) / (1 - sy)) / (Math.PI * 4);
    }

    /// <summary>
    /// Converts geographic coordinates to the index of the tile.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="x">Longitude</param>
    /// <param name="y">Latitude</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Tile index</returns>
    private static OnlineMapsVector2i LatLongToTile(float x, float y, int zoom)
    {
        LatLongToMercat(ref x, ref y);
        uint mapSize = (uint) tileSize << zoom;
        int px = (int) Clip(x * mapSize + 0.5, 0, mapSize - 1);
        int py = (int) Clip(y * mapSize + 0.5, 0, mapSize - 1);
        int ix = px / tileSize;
        int iy = py / tileSize;

        return new OnlineMapsVector2i(ix, iy);
    }

    /// <summary>
    /// Converts geographic coordinates to the index of the tile.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="p">Geographic coordinates (X - Lng, Y - Lat)</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Tile index</returns>
    public static OnlineMapsVector2i LatLongToTile(Vector2 p, int zoom)
    {
        return LatLongToTile(p.x, p.y, zoom);
    }

    /// <summary>
    /// Converts geographic coordinates to the index of the tile.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="x">Longitude.</param>
    /// <param name="y">Latitude.</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Tile index</returns>
    public static OnlineMapsVector2i LatLongToTile(double x, double y, int zoom)
    {
        LatLongToMercat(ref x, ref y);
        uint mapSize = (uint)tileSize << zoom;
        int px = (int)Clip(x * mapSize + 0.5, 0, mapSize - 1);
        int py = (int)Clip(y * mapSize + 0.5, 0, mapSize - 1);
        int ix = px / tileSize;
        int iy = py / tileSize;

        return new OnlineMapsVector2i(ix, iy);
    }

    /// <summary>
    /// Converts geographic coordinates to the index of the tile.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="dx">Longitude</param>
    /// <param name="dy">Latitude</param>
    /// <param name="zoom">Zoom</param>
    /// <param name="tx">Tile X</param>
    /// <param name="ty">Tile Y</param>
    public static void LatLongToTiled(double dx, double dy, int zoom, out double tx, out double ty)
    {
        LatLongToMercat(ref dx, ref dy);
        uint mapSize = (uint)tileSize << zoom;
        double px = Clip(dx * mapSize + 0.5, 0, mapSize - 1);
        double py = Clip(dy * mapSize + 0.5, 0, mapSize - 1);
        tx = px / tileSize;
        ty = py / tileSize;
    }

    /// <summary>
    /// Converts geographic coordinates to tile coordinates.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="p">Geographic coordinates (X - Lng, Y - Lat)</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Tile coordinates</returns>
    public static Vector2 LatLongToTilef(Vector2 p, int zoom)
    {
        LatLongToMercat(ref p.x, ref p.y);
        uint mapSize = (uint) tileSize << zoom;
        float px = (float) Clip(p.x * mapSize + 0.5, 0, mapSize - 1);
        float py = (float)Clip(p.y * mapSize + 0.5, 0, mapSize - 1);
        float fx = px / tileSize;
        float fy = py / tileSize;

        return new Vector2(fx, fy);
    }

    /// <summary>
    /// Converts geographic coordinates to tile coordinates.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="p">Geographic coordinates (X - Lng, Y - Lat)</param>
    /// <param name="zoom">Zoom</param>
    /// <param name="fx">Tile X</param>
    /// <param name="fy">Tile Y</param>
    public static void LatLongToTilef(Vector2 p, int zoom, out float fx, out float fy)
    {
        LatLongToMercat(ref p.x, ref p.y);
        uint mapSize = (uint)tileSize << zoom;
        float px = (float)Clip(p.x * mapSize + 0.5, 0, mapSize - 1);
        float py = (float)Clip(p.y * mapSize + 0.5, 0, mapSize - 1);
        fx = px / tileSize;
        fy = py / tileSize;
    }

    /// <summary>
    /// Converts geographic coordinates to tile coordinates.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="x">Longitude</param>
    /// <param name="y">Latitude</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Tile coordinates</returns>
    public static Vector2 LatLongToTilef(float x, float y, int zoom)
    {
        LatLongToMercat(ref x, ref y);
        uint mapSize = (uint)tileSize << zoom;
        float px = (float)Clip(x * mapSize + 0.5, 0, mapSize - 1);
        float py = (float)Clip(y * mapSize + 0.5, 0, mapSize - 1);
        float fx = px / tileSize;
        float fy = py / tileSize;

        return new Vector2(fx, fy);
    }

    public static Vector2 NearestPointStrict(this Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        var fullDirection = lineEnd - lineStart;
        var lineDirection = fullDirection.normalized;
        var closestPoint = Vector2.Dot((point - lineStart), lineDirection) / Vector2.Dot(lineDirection, lineDirection);
        return lineStart + (Mathf.Clamp(closestPoint, 0, fullDirection.magnitude) * lineDirection);
    }

    private static double Repeat(double n, double minValue, double maxValue)
    {
        if (double.IsInfinity(n) || double.IsInfinity(minValue) || double.IsInfinity(maxValue) ||
            double.IsNaN(n) || double.IsNaN(minValue) || double.IsNaN(maxValue)) return n;

        double range = maxValue - minValue;
        while (n < minValue || n > maxValue)
        {
            if (n < minValue) n += range;
            else if (n > maxValue) n -= range;
        }
        return n;
    }

    /// <summary>
    /// Converts tile coordinates to geographic coordinates.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="x">Tile X</param>
    /// <param name="y">Tile Y</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Geographic coordinates (X - Lng, Y - Lat)</returns>
    public static Vector2 TileToLatLong(int x, int y, int zoom)
    {
        double mapSize = tileSize << zoom;
        double lx = 360 * ((Repeat(x * tileSize, 0, mapSize - 1) / mapSize) - 0.5);
        double ly = 90 -
                    360 * Math.Atan(Math.Exp(-(0.5 - (Clip(y * tileSize, 0, mapSize - 1) / mapSize)) * 2 * Math.PI)) /
                    Math.PI;
        return new Vector2((float) lx, (float) ly);
    }

    /// <summary>
    /// Converts tile coordinates to geographic coordinates.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="x">Tile X</param>
    /// <param name="y">Tile Y</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Geographic coordinates (X - Lng, Y - Lat)</returns>
    public static Vector2 TileToLatLong(float x, float y, int zoom)
    {
        double mapSize = tileSize << zoom;
        double lx = 360 * ((Repeat(x * tileSize, 0, mapSize - 1) / mapSize) - 0.5);
        double ly = 90 -
                    360 * Math.Atan(Math.Exp(-(0.5 - (Clip(y * tileSize, 0, mapSize - 1) / mapSize)) * 2 * Math.PI)) /
                    Math.PI;
        return new Vector2((float) lx, (float) ly);
    }

    /// <summary>
    /// Converts tile coordinates to geographic coordinates.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="tx">Tile X</param>
    /// <param name="ty">Tile Y</param>
    /// <param name="zoom">Zoom</param>
    /// <param name="lx">Longitude</param>
    /// <param name="ly">Latitude</param>
    public static void TileToLatLong(double tx, double ty, int zoom, out double lx, out double ly)
    {
        double mapSize = tileSize << zoom;
        lx = 360 * ((Repeat(tx * tileSize, 0, mapSize - 1) / mapSize) - 0.5);
        ly = 90 - 360 * Math.Atan(Math.Exp(-(0.5 - (Clip(ty * tileSize, 0, mapSize - 1) / mapSize)) * 2 * Math.PI)) / Math.PI;
    }

    /// <summary>
    /// Converts tile coordinates to geographic coordinates.
    /// What is the tiles, and how it works, you can read here:
    /// https://developers.google.com/maps/documentation/javascript/v2/overlays?csw=1#Google_Maps_Coordinates
    /// </summary>
    /// <param name="p">Tile coordinates</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Geographic coordinates (X - Lng, Y - Lat)</returns>
    public static Vector2 TileToLatLong(Vector2 p, int zoom)
    {
        return TileToLatLong(p.x, p.y, zoom);
    }

    /// <summary>
    /// Converts tile index to quadkey.
    /// What is the tiles and quadkey, and how it works, you can read here:
    /// http://msdn.microsoft.com/en-us/library/bb259689.aspx
    /// </summary>
    /// <param name="x">Tile X</param>
    /// <param name="y">Tile Y</param>
    /// <param name="zoom">Zoom</param>
    /// <returns>Quadkey</returns>
    public static string TileToQuadKey(int x, int y, int zoom)
    {
        StringBuilder quadKey = new StringBuilder();
        for (int i = zoom; i > 0; i--)
        {
            char digit = '0';
            int mask = 1 << (i - 1);
            if ((x & mask) != 0) digit++;
            if ((y & mask) != 0)
            {
                digit++;
                digit++;
            }
            quadKey.Append(digit);
        }
        return quadKey.ToString();
    }

    public static IEnumerable<int> Triangulate(List<Vector2> points)
    {
        List<int> indices = new List<int>();

        int n = points.Count;
        if (n < 3) return indices;

        int[] V = new int[n];
        if (TriangulateArea(points) > 0) for (int v = 0; v < n; v++) V[v] = v;
        else for (int v = 0; v < n; v++) V[v] = (n - 1) - v;

        int nv = n;
        int count = 2 * nv;
        for (int v = nv - 1; nv > 2; )
        {
            if ((count--) <= 0) return indices;

            int u = v;
            if (nv <= u) u = 0;
            v = u + 1;
            if (nv <= v) v = 0;
            int w = v + 1;
            if (nv <= w) w = 0;

            if (TriangulateSnip(points, u, v, w, nv, V))
            {
                int s, t;
                indices.Add(V[u]);
                indices.Add(V[v]);
                indices.Add(V[w]);
                for (s = v, t = v + 1; t < nv; s++, t++) V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        indices.Reverse();
        return indices;
    }

    private static float TriangulateArea(List<Vector2> points)
    {
        int n = points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = points[p];
            Vector2 qval = points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    private static bool TriangulateInsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float bp = (C.x - B.x) * (P.y - B.y) - (C.y - B.y) * (P.x - B.x);
        float ap = (B.x - A.x) * (P.y - A.y) - (B.y - A.y) * (P.x - A.x);
        float cp = (A.x - C.x) * (P.y - C.y) - (A.y - C.y) * (P.x - C.x);
        return ((bp >= 0.0f) && (cp >= 0.0f) && (ap >= 0.0f));
    }

    private static bool TriangulateSnip(List<Vector2> points, int u, int v, int w, int n, int[] V)
    {
        Vector2 A = points[V[u]];
        Vector2 B = points[V[v]];
        Vector2 C = points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x)))) return false;
        for (int p = 0; p < n; p++)
        {
            if (p == u || p == v || p == w) continue;
            if (TriangulateInsideTriangle(A, B, C, points[V[p]])) return false;
        }
        return true;
    }
}