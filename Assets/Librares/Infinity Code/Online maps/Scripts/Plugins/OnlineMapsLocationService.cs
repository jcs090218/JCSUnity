/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls map using GPS.\n
/// Online Maps Location Service is a wrapper for Unity Location Service.\n
/// http://docs.unity3d.com/ScriptReference/LocationService.html
/// </summary>
[Serializable]
[AddComponentMenu("Infinity Code/Online Maps/Plugins/Location Service")]
// ReSharper disable once UnusedMember.Global
public class OnlineMapsLocationService : MonoBehaviour
{
    private static OnlineMapsLocationService _instance;

    /// <summary>
    /// This event is called when the user rotates the device.
    /// </summary>
    public Action<float> OnCompassChanged;

    /// <summary>
    /// This event is called when changed your GPS location.
    /// </summary>
    public Action<Vector2> OnLocationChanged;

    /// <summary>
    /// Update stop position when user input.
    /// </summary>
    public bool autoStopUpdateOnInput = true;

    /// <summary>
    /// Specifies the need to create a marker that indicates the current GPS coordinates.
    /// </summary>
    public bool createMarkerInUserPosition = false;

    /// <summary>
    /// Desired service accuracy in meters. 
    /// </summary>
    public float desiredAccuracy = 10;

    /// <summary>
    /// Emulated compass trueHeading.\n
    /// Do not use.\n
    /// Use OnlineMapsLocationService.trueHeading.
    /// </summary>
    public float emulatorCompass;

    /// <summary>
    /// Emulated GPS position.\n
    /// Do not use.\n
    /// Use OnlineMapsLocationService.position.
    /// </summary>
    public Vector2 emulatorPosition;

    /// <summary>
    /// Tooltip of the marker.
    /// </summary>
    public string markerTooltip;

    /// <summary>
    /// Type of the marker.
    /// </summary>
    public OnlineMapsLocationServiceMarkerType markerType = OnlineMapsLocationServiceMarkerType.twoD;

    /// <summary>
    /// Align of the 2D marker.
    /// </summary>
    public OnlineMapsAlign marker2DAlign = OnlineMapsAlign.Center;

    /// <summary>
    /// Texture of 2D marker.
    /// </summary>
    public Texture2D marker2DTexture;

    /// <summary>
    /// Prefab of 3D marker.
    /// </summary>
    public GameObject marker3DPrefab;

    /// <summary>
    /// The maximum number of stored positions./n
    /// It is used to calculate the speed.
    /// </summary>
    public int maxPositionCount = 3;

    /// <summary>
    /// Current GPS coordinates.\n
    /// <strong>Important: position not available Start, because GPS is not already initialized. \n
    /// Use OnLocationChanged event, to determine the initialization of GPS.</strong>
    /// </summary>
    public Vector2 position = Vector2.zero;

    /// <summary>
    /// Use the GPS coordinates after seconds of inactivity.
    /// </summary>
    public int restoreAfter = 10;

    /// <summary>
    /// The heading in degrees relative to the geographic North Pole.\n
    /// <strong>Important: position not available Start, because compass is not already initialized. \n
    /// Use OnCompassChanged event, to determine the initialization of compass.</strong>
    /// </summary>
    public float trueHeading = 0;

    /// <summary>
    ///  The minimum distance (measured in meters) a device must move laterally before location is updated.
    /// </summary>
    public float updateDistance = 10;

    /// <summary>
    /// Specifies whether the script will automatically update the location.
    /// </summary>
    public bool updatePosition = true;

    /// <summary>
    /// Specifies the need for marker rotation.
    /// </summary>
    public bool useCompassForMarker = false;

    /// <summary>
    /// Specifies GPS emulator usage. \n
    /// Works only in Unity Editor.
    /// </summary>
    public bool useGPSEmulator = false;

    private OnlineMaps api;
    private bool allowUpdatePosition = true;
    private long lastPositionChangedTime;
    private bool lockDisable;
    
    private OnlineMapsMarkerBase _marker;
    private List<LastPositionItem> lastPositions;
    private double lastLocationInfoTimestamp;
    private float _speed = 0;
    private bool started = false;

    private string errorMessage = "";

    /// <summary>
    /// Instance of LocationService.
    /// </summary>
    public static OnlineMapsLocationService instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// Instance of marker.
    /// </summary>
    public static OnlineMapsMarkerBase marker
    {
        get { return _instance._marker; }
        set { _instance._marker = value; }
    }

    /// <summary>
    /// Speed km/h.
    /// Note: in Unity Editor will always be zero.
    /// </summary>
    public float speed
    {
        get { return _speed; }
    }

    private void OnChangePosition()
    {
        if (lockDisable) return;

        lastPositionChangedTime = DateTime.Now.Ticks;
        if (autoStopUpdateOnInput) allowUpdatePosition = false;
    }

// ReSharper disable once UnusedMember.Local
    private void OnEnable()
    {
        _instance = this;
    }

    private void OnGUI()
    {
        if (!string.IsNullOrEmpty(errorMessage)) GUI.Label(new Rect(5, 5, Screen.width, 300), errorMessage);
    }

    public OnlineMapsXML Save(OnlineMapsXML parent)
    {
        OnlineMapsXML element = parent.Create("LocationService");
        element.Create("DesiredAccuracy", desiredAccuracy);
        element.Create("UpdatePosition", updatePosition);
        element.Create("AutoStopUpdateOnInput", autoStopUpdateOnInput);
        element.Create("RestoreAfter", restoreAfter);

        element.Create("CreateMarkerInUserPosition", createMarkerInUserPosition);

        if (createMarkerInUserPosition)
        {
            element.Create("MarkerType", (int)markerType);
            
            if (markerType == OnlineMapsLocationServiceMarkerType.twoD)
            {
                element.Create("Marker2DAlign", (int) marker2DAlign);
                element.Create("Marker2DTexture", marker2DTexture);
            }
            else element.Create("Marker3DPrefab", marker3DPrefab);

            element.Create("MarkerTooltip", markerTooltip);
            element.Create("UseCompassForMarker", useCompassForMarker);
        }

        element.Create("UseGPSEmulator", useGPSEmulator);
        if (useGPSEmulator)
        {
            element.Create("EmulatorPosition", emulatorPosition);
            element.Create("EmulatorCompass", emulatorCompass);
        }

        return element;
    }

    private void Start()
    {
        api = OnlineMaps.instance;
        api.OnChangePosition += OnChangePosition;
    }

// ReSharper disable once UnusedMember.Local
	private void Update () 
    {
	    try
	    {
            if (OnlineMaps.instance == null) return;

            if (!started)
            {
#if !UNITY_EDITOR
                if (!Input.location.isEnabledByUser) return;

                Input.compass.enabled = true;
                Input.location.Start(desiredAccuracy, updateDistance);
#endif
                started = true;
            }

#if !UNITY_EDITOR
            useGPSEmulator = false;
#endif

            if (!useGPSEmulator && Input.location.status != LocationServiceStatus.Running) return;

            if (createMarkerInUserPosition && _marker == null && (useGPSEmulator || position != Vector2.zero)) UpdateMarker();

            bool compassChanged = false;

#if !UNITY_EDITOR
	        UpdateCompassFromInput(ref compassChanged);
#else
            if (useGPSEmulator) UpdateCompassFromEmulator(ref compassChanged);
            else UpdateCompassFromInput(ref compassChanged);
#endif

            bool positionChanged = false;

#if !UNITY_EDITOR
            UpdateSpeed();
            UpdatePositionFromInput(ref positionChanged);
#else
            if (useGPSEmulator) UpdatePositionFromEmulator(ref positionChanged);
            else UpdatePositionFromInput(ref positionChanged);
#endif

            if (positionChanged && OnLocationChanged != null) OnLocationChanged(position);

            if (createMarkerInUserPosition && (positionChanged || compassChanged)) UpdateMarker();

            if (updatePosition)
            {
                if (allowUpdatePosition)
                {
                    UpdatePosition();
                }
                else if (restoreAfter > 0 && DateTime.Now.Ticks > lastPositionChangedTime + OnlineMapsUtils.second * restoreAfter)
                {
                    allowUpdatePosition = true;
                    UpdatePosition();
                }
            } 
	    }
	    catch (Exception exception)
	    {
	        errorMessage = exception.Message + "\n" + exception.StackTrace;
	    }
    }

    private void UpdateCompassFromEmulator(ref bool compassChanged)
    {
        if (trueHeading != emulatorCompass)
        {
            compassChanged = true;
            trueHeading = emulatorCompass;
            if (OnCompassChanged != null) OnCompassChanged(trueHeading / 360);
        }
    }

    private void UpdateCompassFromInput(ref bool compassChanged)
    {
        if (trueHeading != Input.compass.trueHeading)
        {
            compassChanged = true;
            trueHeading = Input.compass.trueHeading;
            if (OnCompassChanged != null) OnCompassChanged(trueHeading / 360);
        }
    }

    private void UpdateMarker()
    {
        if (_marker == null)
        {
            if (markerType == OnlineMapsLocationServiceMarkerType.twoD)
            {
                _marker = OnlineMaps.instance.AddMarker(position, marker2DTexture, markerTooltip);
                (_marker as OnlineMapsMarker).align = marker2DAlign;
            }
            else
            {
                OnlineMapsControlBase3D control = OnlineMapsControlBase3D.instance;
                if (control == null)
                {
                    Debug.LogError("You must use the 3D control (Texture or Tileset).");
                    createMarkerInUserPosition = false;
                    return;
                }
                _marker = control.AddMarker3D(position, marker3DPrefab);
                _marker.label = markerTooltip;
            }
        }
        else
        {
            _marker.position = position;
        }

        if (useCompassForMarker)
        {
            if (markerType == OnlineMapsLocationServiceMarkerType.twoD)
                (_marker as OnlineMapsMarker).rotation = trueHeading / 360;
            else (_marker as OnlineMapsMarker3D).rotation = Quaternion.Euler(0, trueHeading, 0);
        }

        api.Redraw();
    }

    /// <summary>
    /// Sets map position using GPS coordinates.
    /// </summary>
    public void UpdatePosition()
    {  
        if (!useGPSEmulator && position == Vector2.zero) return;
        if (api == null) return;

        lockDisable = true;

        Vector2 p = api.position;
        bool changed = false;

        if (p.x != position.x)
        {
            p.x = position.x;
            changed = true;
        }
        if (p.y != position.y)
        {
            p.y = position.y;
            changed = true;
        }
        if (changed)
        {
            api.position = p;
            api.Redraw();
        }

        lockDisable = false;
    }

    private void UpdatePositionFromEmulator(ref bool positionChanged)
    {
        if (position.x != emulatorPosition.x)
        {
            position.x = emulatorPosition.x;
            positionChanged = true;
        }
        if (position.y != emulatorPosition.y)
        {
            position.y = emulatorPosition.y;
            positionChanged = true;
        }
    }

    private void UpdatePositionFromInput(ref bool positionChanged)
    {
        if (position.x != Input.location.lastData.longitude)
        {
            position.x = Input.location.lastData.longitude;
            positionChanged = true;
        }
        if (position.y != Input.location.lastData.latitude)
        {
            position.y = Input.location.lastData.latitude;
            positionChanged = true;
        }
    }

    private void UpdateSpeed()
    {
        LocationInfo lastData = Input.location.lastData;
        if (lastLocationInfoTimestamp == lastData.timestamp) return;

        lastLocationInfoTimestamp = lastData.timestamp;

        if (lastPositions == null) lastPositions = new List<LastPositionItem>();

        lastPositions.Add(new LastPositionItem(lastData.longitude, lastData.latitude, lastData.timestamp));
        while (lastPositions.Count > maxPositionCount) lastPositions.RemoveAt(0);

        if (lastPositions.Count < 2)
        {
            _speed = 0;
            return;
        }

        LastPositionItem p1 = lastPositions[0];
        LastPositionItem p2 = lastPositions[lastPositions.Count - 1];

        double dx, dy;
        OnlineMapsUtils.DistanceBetweenPoints(p1.lng, p1.lat, p2.lng, p2.lat, out dx, out dy);
        double distance = Math.Sqrt(dx * dx + dy * dy);
        double time = (p2.timestamp - p1.timestamp) / 3600;
        _speed = Mathf.Abs((float) (distance / time));
    }

    internal struct LastPositionItem
    {
        public float lat;
        public float lng;
        public double timestamp;

        public LastPositionItem(float longitude, float latitude, double timestamp)
        {
            lng = longitude;
            lat = latitude;
            this.timestamp = timestamp;
        }
    }
}