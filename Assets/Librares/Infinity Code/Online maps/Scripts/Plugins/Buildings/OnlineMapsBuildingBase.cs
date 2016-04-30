/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of buildings.
/// </summary>
[AddComponentMenu("")]
public class OnlineMapsBuildingBase:MonoBehaviour
{
    /// <summary>
    /// Events that occur when user click on the building.
    /// </summary>
    public Action<OnlineMapsBuildingBase> OnClick;

    /// <summary>
    /// Events that occur when dispose building.
    /// </summary>
    public Action<OnlineMapsBuildingBase> OnDispose;

    /// <summary>
    /// Events that occur when user press on the building.
    /// </summary>
    public Action<OnlineMapsBuildingBase> OnPress;

    /// <summary>
    /// Events that occur when user release on the building.
    /// </summary>
    public Action<OnlineMapsBuildingBase> OnRelease;

    /// <summary>
    /// Geographical coordinates of the boundaries of the building.
    /// </summary>
    public Bounds boundsCoords;

    /// <summary>
    /// Geographical coordinates of the center point.
    /// </summary>
    public Vector2 centerCoordinates;

    /// <summary>
    /// ID of building.
    /// </summary>
    public string id;

    /// <summary>
    /// Zoom, in which this building was created.
    /// </summary>
    public int initialZoom;

    /// <summary>
    /// Array of building meta key-value pair.
    /// </summary>
    public OnlineMapsBuildingMetaInfo[] metaInfo;

    /// <summary>
    /// Perimeter of building.
    /// </summary>
    public float perimeter;

    private int lastTouchCount = 0;

    protected Collider buildingCollider;
    protected bool hasErrors = false;

    private bool isPressed;
    private Vector3 pressPoint;

    private void AddMetaInfo(string title, string info)
    {
        if (metaInfo == null) metaInfo = new OnlineMapsBuildingMetaInfo[0];

        List<OnlineMapsBuildingMetaInfo> metaList = new List<OnlineMapsBuildingMetaInfo>(metaInfo)
        {
            new OnlineMapsBuildingMetaInfo {info = info, title = title}
        };

        metaInfo = metaList.ToArray();
    }

    protected static bool CheckIgnoredBuildings(OnlineMapsOSMWay way)
    {
        if (way.GetTagValue("building") == "bridge") return true;

        string layer = way.GetTagValue("layer");
        if (!String.IsNullOrEmpty(layer) && Int32.Parse(layer) < 0) return true;

        return false;
    }

    protected static GameObject CreateGameObject(string id)
    {
        GameObject buildingGameObject = new GameObject(id);
        buildingGameObject.SetActive(false);

        buildingGameObject.transform.parent = OnlineMapsBuildings.buildingContainer.transform;
        return buildingGameObject;
    }

    /// <summary>
    /// Dispose of building.
    /// </summary>
    public void Dispose()
    {
        if (OnDispose != null) OnDispose(this);
    }

    /// <summary>
    /// Loads the meta data from the XML.
    /// </summary>
    /// <param name="item">Object that contains meta description.</param>
    public void LoadMeta(OnlineMapsOSMBase item)
    {
        foreach (OnlineMapsOSMTag itemTag in item.tags) AddMetaInfo(itemTag.key, itemTag.value);
    }

    protected static List<Vector3> GetLocalPoints(List<OnlineMapsOSMNode> nodes)
    {
        OnlineMaps api = OnlineMaps.instance;
        Vector2 startTilePos = OnlineMapsUtils.LatLongToTilef(api.topLeftPosition, api.buffer.apiZoom);

        List<Vector3> localPoints = new List<Vector3>();

        float sw = OnlineMapsUtils.tileSize * api.tilesetSize.x / api.tilesetWidth;
        float sh = OnlineMapsUtils.tileSize * api.tilesetSize.y / api.tilesetHeight;

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector2 p = OnlineMapsUtils.LatLongToTilef(nodes[i], api.buffer.apiZoom) - startTilePos;
            localPoints.Add(new Vector3(-p.x * sw, 0, p.y * sh));
        }
        return localPoints;
    }

    private bool HitTest()
    {
        RaycastHit hit;
        return
            buildingCollider.Raycast(
                OnlineMapsTileSetControl.instance.activeCamera.ScreenPointToRay(Input.GetTouch(0).position), out hit,
                OnlineMapsUtils.maxRaycastDistance);
    }

    protected void OnBasePress()
    {
        isPressed = true;
        if (OnPress != null) OnPress(this);
        pressPoint = Input.mousePosition;
    }

    protected void OnBaseRelease()
    {
        isPressed = false;
        if (OnRelease != null) OnRelease(this);
        if ((pressPoint - Input.mousePosition).magnitude < 10)
        {
            if (OnClick != null) OnClick(this);
        }
    }

#if !UNITY_ANDROID
    // ReSharper disable once UnusedMember.Global
    protected void OnMouseDown()
    {
        OnBasePress();
    }

    // ReSharper disable once UnusedMember.Global
    protected void OnMouseUp()
    {
        OnBaseRelease();
    }
#endif

    private void Update()
    {
        if (Input.touchCount != lastTouchCount)
        {
            if (Input.touchCount == 1)
            {
                if (HitTest())
                {
                    OnBasePress();
                }
            }
            else if (Input.touchCount == 0)
            {
                if (isPressed && HitTest()) OnBaseRelease();
                isPressed = false;
            }

            lastTouchCount = Input.touchCount;
        }
    }

    /// <summary>
    /// Type the building's roof.
    /// </summary>
    protected enum OnlineMapsBuildingRoofType
    {
        /// <summary>
        /// Dome roof.
        /// </summary>
        dome,

        /// <summary>
        /// Flat roof.
        /// </summary>
        flat
    }
}

/// <summary>
/// Building meta key-value pair.
/// </summary>
[Serializable]
public class OnlineMapsBuildingMetaInfo
{
    /// <summary>
    /// Meta value.
    /// </summary>
    public string info;

    /// <summary>
    /// Meta key.
    /// </summary>
    public string title;
}