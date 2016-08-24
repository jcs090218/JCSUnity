/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Built-in buildings generator.
/// </summary>
[AddComponentMenu("")]
public class OnlineMapsBuildingBuiltIn : OnlineMapsBuildingBase
{
    private OnlineMapsOSMWay way;

    private static void AddHouseWallVerticle(List<Vector3> vertices, Vector3 p, float topPoint)
    {
        Vector3 tv = new Vector3(p.x, topPoint, p.z);

        vertices.Add(p);
        vertices.Add(tv);
    }

    private static void AnalizeHouseRoofType(OnlineMapsOSMWay way, ref float baseHeight, ref OnlineMapsBuildingRoofType roofType, ref float roofHeight)
    {
        string roofShape = way.GetTagValue("roof:shape");
        string roofHeightStr = way.GetTagValue("roof:height");
        string minHeightStr = way.GetTagValue("min_height");
        if (!String.IsNullOrEmpty(roofShape))
        {
            if ((roofShape == "dome" || roofShape == "pyramidal") && !String.IsNullOrEmpty(roofHeightStr))
            {
                GetHeightFromString(roofHeightStr, ref roofHeight);
                baseHeight -= roofHeight;
                roofType = OnlineMapsBuildingRoofType.dome;
            }
        }
        else if (!String.IsNullOrEmpty(roofHeightStr))
        {
            GetHeightFromString(roofHeightStr, ref roofHeight);
            baseHeight -= roofHeight;
            roofType = OnlineMapsBuildingRoofType.dome;
        }
        else if (!String.IsNullOrEmpty(minHeightStr))
        {
            float totalHeight = baseHeight;
            GetHeightFromString(minHeightStr, ref baseHeight);
            roofHeight = totalHeight - baseHeight;
            roofType = OnlineMapsBuildingRoofType.dome;
        }
    }

    private static void AnalizeHouseTags(OnlineMapsOSMWay way, ref Material wallMaterial, ref Material roofMaterial, ref float baseHeight)
    {
        string heightStr = way.GetTagValue("height");
        string levelsStr = way.GetTagValue("building:levels");
        GetHeightFromString(heightStr, ref baseHeight);
        if (String.IsNullOrEmpty(heightStr) && !String.IsNullOrEmpty(levelsStr))
            baseHeight = float.Parse(levelsStr) * OnlineMapsBuildings.instance.levelHeight;
        else baseHeight = Random.Range(OnlineMapsBuildings.instance.levelsRange.min, OnlineMapsBuildings.instance.levelsRange.max) * OnlineMapsBuildings.instance.levelHeight;

        if (baseHeight < OnlineMapsBuildings.instance.minHeight) baseHeight = OnlineMapsBuildings.instance.minHeight;

        string colorStr = way.GetTagValue("building:colour");
        if (!String.IsNullOrEmpty(colorStr)) wallMaterial.color = roofMaterial.color = StringToColor(colorStr);
    }

    /// <summary>
    /// Creates a new building, based on Open Street Map.
    /// </summary>
    /// <param name="container">Reference to OnlineMapsBuildings.</param>
    /// <param name="way">Way of building.</param>
    /// <param name="nodes">Nodes obtained from Open Street Maps.</param>
    /// <returns>Building instance.</returns>
    public static OnlineMapsBuildingBase Create(OnlineMapsBuildings container, OnlineMapsOSMWay way, Dictionary<string, OnlineMapsOSMNode> nodes)
    {
        if (CheckIgnoredBuildings(way)) return null;

        List<OnlineMapsOSMNode> usedNodes = way.GetNodes(nodes);
        List<Vector3> points = GetLocalPoints(usedNodes);

        if (points.Count < 3) return null;
        if (points[0] == points[points.Count - 1]) points.RemoveAt(points.Count - 1);
        if (points.Count < 3) return null;

        for (int i = 0; i < points.Count; i++)
        {
            int prev = i - 1;
            if (prev < 0) prev = points.Count - 1;

            int next = i + 1;
            if (next >= points.Count) next = 0;

            float a1 = OnlineMapsUtils.Angle2D(points[prev], points[i]);
            float a2 = OnlineMapsUtils.Angle2D(points[i], points[next]);

            if (Mathf.Abs(a1 - a2) < 5)
            {
                points.RemoveAt(i);
                i--;
            }
        }

        if (points.Count < 3) return null;

        Vector4 cp = new Vector4(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
        foreach (Vector3 point in points)
        {
            if (point.x < cp.x) cp.x = point.x;
            if (point.z < cp.y) cp.y = point.z;
            if (point.x > cp.z) cp.z = point.x;
            if (point.z > cp.w) cp.w = point.z;
        }
        //centerPoint = points.Aggregate(centerPoint, (current, point) => current + point) / points.Count;
        Vector3 centerPoint = new Vector3((cp.z + cp.x) / 2, points.Min(p => p.y), (cp.y + cp.w) / 2);

        bool generateWall = true;

        if (way.HasTagKey("building"))
        {
            string buildingType = way.GetTagValue("building");
            if (buildingType == "roof") generateWall = false;
        }

        float baseHeight = 15;
        float roofHeight = 0;

        OnlineMapsBuildingMaterial material = GetRandomMaterial(container);

        Material defWallMaterial = (material != null)? material.wall: null;
        Material defRoofMaterial = (material != null) ? material.roof : null;
        Vector2 scale = (material != null) ? material.scale : Vector2.one;

        if (defWallMaterial == null) defWallMaterial = new Material(Shader.Find("Diffuse"));
        if (defRoofMaterial == null) defRoofMaterial = new Material(Shader.Find("Diffuse"));

        Material wallMaterial = new Material(defWallMaterial);
        Material roofMaterial = new Material(defRoofMaterial);

        OnlineMapsBuildingRoofType roofType = OnlineMapsBuildingRoofType.flat;
        AnalizeHouseTags(way, ref wallMaterial, ref roofMaterial, ref baseHeight);
        AnalizeHouseRoofType(way, ref baseHeight, ref roofType, ref roofHeight);

        Vector3[] baseVerticles = points.Select(p => p - centerPoint).ToArray();

        GameObject houseGO = CreateGameObject(way.id);
        MeshRenderer renderer = houseGO.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = houseGO.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh {name = way.id};

        meshFilter.sharedMesh = mesh;
        renderer.sharedMaterials = new []
        {
            wallMaterial,
            roofMaterial
        };

        OnlineMapsBuildingBuiltIn building = houseGO.AddComponent<OnlineMapsBuildingBuiltIn>();
        houseGO.transform.localPosition = centerPoint;
        houseGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

        Vector2 centerCoords = Vector2.zero;
        float minCX = float.MaxValue, minCY = float.MaxValue, maxCX = float.MinValue, maxCY = float.MinValue;

        foreach (OnlineMapsOSMNode node in usedNodes)
        {
            Vector2 nodeCoords = node;
            centerCoords += nodeCoords;
            if (nodeCoords.x < minCX) minCX = nodeCoords.x;
            if (nodeCoords.y < minCY) minCY = nodeCoords.y;
            if (nodeCoords.x > maxCX) maxCX = nodeCoords.x;
            if (nodeCoords.y > maxCY) maxCY = nodeCoords.y;
        }

        building.id = way.id;
        building.initialZoom = OnlineMaps.instance.zoom;
        building.centerCoordinates = new Vector2((maxCX + minCX) / 2, (maxCY + minCY) / 2);
        building.boundsCoords = new Bounds(building.centerCoordinates, new Vector3(maxCX - minCX, maxCY - minCY));

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> wallTriangles = new List<int>();
        List<int> roofTriangles = new List<int>();

        if (generateWall) building.CreateHouseWall(baseVerticles, baseHeight, wallMaterial, scale, ref vertices, ref uvs, ref wallTriangles);
        building.CreateHouseRoof(baseVerticles, baseHeight, roofHeight, roofType, ref vertices, ref uvs, ref roofTriangles);

        if (building.hasErrors)
        {
            DestroyImmediate(building.gameObject);
            return null;
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.subMeshCount = 2;
        mesh.SetTriangles(wallTriangles.ToArray(), 0);
        mesh.SetTriangles(roofTriangles.ToArray(), 1);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        building.buildingCollider = houseGO.AddComponent<MeshCollider>();
        (building.buildingCollider as MeshCollider).sharedMesh = mesh;

        return building;
    }

    private void CreateHouseRoof(Vector3[] baseVerticles, float baseHeight, float roofHeight, OnlineMapsBuildingRoofType roofType, ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<int> triangles)
    {
        List<Vector2> roofPoints = new List<Vector2>();
        List<Vector3> roofVertices = new List<Vector3>();

        try
        {
            CreateHouseRoofVerticles(baseVerticles, roofVertices, roofPoints, baseHeight);
            CreateHouseRoofTriangles(roofVertices, roofType, roofPoints, baseHeight, roofHeight, ref triangles);

            if (triangles.Count == 0)
            {
                hasErrors = true;
                return;
            }

            Vector3 side1 = roofVertices[triangles[1]] - roofVertices[triangles[0]];
            Vector3 side2 = roofVertices[triangles[2]] - roofVertices[triangles[0]];
            Vector3 perp = Vector3.Cross(side1, side2);

            bool reversed = perp.y < 0;
            if (reversed) triangles.Reverse();

            float minX = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxZ = float.MinValue;

            foreach (Vector3 v in roofVertices)
            {
                if (v.x < minX) minX = v.x;
                if (v.z < minZ) minZ = v.z;
                if (v.x > maxX) maxX = v.x;
                if (v.z > maxZ) maxZ = v.z;
            }

            float offX = maxX - minX;
            float offZ = maxZ - minZ;

            uvs.AddRange(roofVertices.Select(v => new Vector2((v.x - minX) / offX, (v.z - minZ) / offZ)));

            int triangleOffset = vertices.Count;
            for (int i = 0; i < triangles.Count; i++) triangles[i] += triangleOffset;

            vertices.AddRange(roofVertices);
        }
        catch (Exception)
        {
            Debug.Log(triangles.Count + "   " + roofVertices.Count);
            hasErrors = true;
            throw;
        }

        
    }

    private static void CreateHouseRoofDome(float height, List<Vector3> vertices, List<int> triangles)
    {
        Vector3 roofTopPoint = Vector3.zero;
        roofTopPoint = vertices.Aggregate(roofTopPoint, (current, point) => current + point) / vertices.Count;
        roofTopPoint.y = height;
        int vIndex = vertices.Count;

        for (int i = 0; i < vertices.Count; i++)
        {
            int p1 = i;
            int p2 = i + 1;
            if (p2 >= vertices.Count) p2 -= vertices.Count;

            triangles.AddRange(new[] { p1, p2, vIndex });
        }

        vertices.Add(roofTopPoint);
    }

    private static void CreateHouseRoofTriangles(List<Vector3> vertices, OnlineMapsBuildingRoofType roofType, List<Vector2> roofPoints, float baseHeight, float roofHeight, ref List<int> triangles)
    {
        if (roofType == OnlineMapsBuildingRoofType.flat)
            triangles.AddRange(OnlineMapsUtils.Triangulate(roofPoints).Select(index => index));
        else if (roofType == OnlineMapsBuildingRoofType.dome)
            CreateHouseRoofDome(baseHeight + roofHeight, vertices, triangles);
    }

    private static void CreateHouseRoofVerticles(Vector3[] baseVerticles, List<Vector3> verticles, List<Vector2> roofPoints, float baseHeight)
    {
        float topPoint = baseHeight * OnlineMapsBuildings.instance.heightScale;
        foreach (Vector3 p in baseVerticles)
        {
            Vector3 tv = new Vector3(p.x, topPoint, p.z);
            Vector2 rp = new Vector2(p.x, p.z);

            if (!verticles.Contains(tv)) verticles.Add(tv);
            if (!roofPoints.Contains(rp)) roofPoints.Add(rp);
        }
    }

    private void CreateHouseWall(Vector3[] baseVerticles, float baseHeight, Material material, Vector2 materialScale, ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<int> triangles)
    {
        CreateHouseWallMesh(baseVerticles, baseHeight, false, ref vertices, ref uvs, ref triangles);

        Vector2 scale = material.mainTextureScale;
        scale.x *= perimeter / 100 * materialScale.x;
        scale.y *= baseHeight / 30 * materialScale.y;
        material.mainTextureScale = scale;
    }

    private void CreateHouseWallMesh(Vector3[] baseVerticles, float baseHeight, bool inverted, ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<int> triangles)
    {
        bool reversed = CreateHouseWallVerticles(baseHeight, baseVerticles, vertices, uvs);
        if (inverted) reversed = !reversed;
        CreateHouseWallTriangles(vertices, reversed, ref triangles);
    }

    private static void CreateHouseWallTriangles(List<Vector3> vertices, bool reversed, ref List<int> triangles)
    {
        for (int i = 0; i < vertices.Count / 2 - 1; i++)
            triangles.AddRange(GetHouseWallTriangle(vertices.Count, reversed, i));
    }

    private bool CreateHouseWallVerticles(float baseHeight, Vector3[] baseVerticles, List<Vector3> vertices, List<Vector2> uvs)
    {
        float topPoint = baseHeight * OnlineMapsBuildings.instance.heightScale;

        foreach (Vector3 p in baseVerticles) AddHouseWallVerticle(vertices, p, topPoint);
        AddHouseWallVerticle(vertices, baseVerticles[0], topPoint);

        perimeter = 0;

        for (int i = 0; i <= vertices.Count / 2; i++)
        {
            int i1 = Mathf.RoundToInt(Mathf.Repeat(i * 2, vertices.Count));
            int i2 = Mathf.RoundToInt(Mathf.Repeat((i + 1) * 2, vertices.Count));
            perimeter += (vertices[i1] - vertices[i2]).magnitude;
        }

        float currentDistance = 0;

        for (int i = 0; i < vertices.Count / 2; i++)
        {
            int i1 = Mathf.RoundToInt(Mathf.Repeat(i * 2, vertices.Count));
            int i2 = Mathf.RoundToInt(Mathf.Repeat((i + 1) * 2, vertices.Count));
            float curU = currentDistance / perimeter;
            uvs.Add(new Vector2(curU, 0));
            uvs.Add(new Vector2(curU, 1));

            currentDistance += (vertices[i1] - vertices[i2]).magnitude;
        }

        int southIndex = -1;
        float southZ = float.MaxValue;

        for (int i = 0; i < baseVerticles.Length; i++)
        {
            if (baseVerticles[i].z < southZ)
            {
                southZ = baseVerticles[i].z;
                southIndex = i;
            }
        }

        int prevIndex = southIndex - 1;
        if (prevIndex < 0) prevIndex = baseVerticles.Length - 1;

        int nextIndex = southIndex + 1;
        if (nextIndex >= baseVerticles.Length) nextIndex = 0;

        float angle1 = OnlineMapsUtils.Angle2D(baseVerticles[southIndex], baseVerticles[nextIndex]);
        float angle2 = OnlineMapsUtils.Angle2D(baseVerticles[southIndex], baseVerticles[prevIndex]);

        return angle1 < angle2;
    }

    private static int[] GetHouseWallTriangle(int countVertices, bool reversed, int i)
    {
        int p1 = i * 2;
        int p2 = (i + 1) * 2;
        int p3 = (i + 1) * 2 + 1;
        int p4 = i * 2 + 1;

        if (p2 >= countVertices) p2 -= countVertices;
        if (p3 >= countVertices) p3 -= countVertices;

        if (reversed) return new[] { p1, p4, p3, p1, p3, p2 };
        return new[] { p2, p3, p1, p3, p4, p1 };
    }

    private static void GetHeightFromString(string str, ref float height)
    {
        if (!String.IsNullOrEmpty(str))
        {
            if (!float.TryParse(str, out height))
            {
                if (str.Substring(str.Length - 2, 2) == "cm")
                {
                    float.TryParse(str.Substring(0, str.Length - 2), out height);
                    height /= 10;
                }
                else if (str.Substring(str.Length - 1, 1) == "m")
                    float.TryParse(str.Substring(0, str.Length - 1), out height);
            }
        }
    }

    private static OnlineMapsBuildingMaterial GetRandomMaterial(OnlineMapsBuildings container)
    {
        if (container.materials == null || container.materials.Length == 0) return null;

        return container.materials[Random.Range(0, container.materials.Length)];
    }

    private static Color StringToColor(string str)
    {
        str = str.ToLower();
        if (str == "black") return Color.black;
        if (str == "blue") return Color.blue;
        if (str == "cyan") return Color.cyan;
        if (str == "gray") return Color.gray;
        if (str == "green") return Color.green;
        if (str == "magenta") return Color.magenta;
        if (str == "red") return Color.red;
        if (str == "white") return Color.white;
        if (str == "yellow") return Color.yellow;

        try
        {
            string hex = (str + "000000").Substring(1, 6);
            byte[] cb =
                Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();
            return new Color32(cb[0], cb[1], cb[2], 255);
        }
        catch
        {
            return Color.white;
        }
    }
}