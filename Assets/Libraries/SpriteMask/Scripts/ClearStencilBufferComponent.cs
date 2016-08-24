using UnityEngine;
using System.Collections;

/// <summary>
/// This component is used do clear Stencil buffer. 
/// If you want to use Unity UI Mask in the same time with SpriteMask, 
/// you need to clear Stencil buffer just before rendering Unity UI components.
/// 
/// To get it working you must create an GameObject and attach to it ClearStencilBufferComponent. 
/// Next you have to set sortingLayer and sortingOrder in that way, 
/// that it shoud be rendered between SpriteMask and Unity UI. 
/// You can see how does it work on scene '10 - SpriteMask & UI Mask'. 
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ClearStencilBufferComponent : SkipMasking
{
    protected static volatile ClearStencilBufferComponent _instance;
    public const string SHADER_MASK_CLEAR = "SpriteMask/ClearStencil";
    [SerializeField]
    private Vector2
        _size = new Vector2 (100, 100);
    [SerializeField]
    private Vector2
        _pivot = new Vector2 (0.5f, 0.5f);
    private Vector3[] vertices = new Vector3[4];
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private int lastSortingOrder;
    private string lastSortingLayerName;

    /// <summary>
    /// Specify the screen region that should be cleared.
    /// </summary>
    public Vector2 size {
        get {
            return _size;
        }
        set {
            _size = value;
            updateMeshSize ();
        }
    }

    /// <summary>
    /// Pivot of clearing region.
    /// </summary>
    /// <value>The pivot.</value>
    public Vector2 pivot {
        get {
            return _pivot;
        }
        set {
            _pivot = value;
            updateMeshSize ();
        }
    }

    public static ClearStencilBufferComponent Instance {
        get {
#if UNITY_EDITOR
            if (Application.isPlaying) {
#endif
            if (_instance == null) {
                _instance = findInstance ();
            }
            return _instance;
#if UNITY_EDITOR
            } else {
                return findInstance ();
            }
#endif
        }
    }

    void Awake ()
    {
        name = "CLEAR_STENCIL_BUFFER";

        meshFilter = GetComponent<MeshFilter> ();
        if (meshFilter == null) {
            meshFilter = gameObject.AddComponent<MeshFilter> ();
        }

        if (meshFilter.sharedMesh == null) {
            Mesh m = new Mesh ();
            m.hideFlags = HideFlags.NotEditable;
            m.name = string.Concat ("RectMesh");

            m.vertices = vertices;
            m.uv = new Vector2[4] {
                new Vector2 (0, 1),
                new Vector2 (1, 1),
                new Vector2 (1, 0),
                new Vector2 (0, 0)
            };
            m.triangles = new int[6] {0,1,2,2,3,0};
            m.RecalculateNormals ();
            m.RecalculateBounds ();

            meshFilter.sharedMesh = m;
        }

        meshRenderer = GetComponent<MeshRenderer> ();
        if (meshRenderer == null) {
            meshRenderer = gameObject.AddComponent<MeshRenderer> ();
        }
        meshRenderer.sortingOrder = 10000;

        if (meshRenderer.sharedMaterial == null) {
            Shader shader = Shader.Find (SHADER_MASK_CLEAR);
            Material mat = new Material (shader);
            mat.hideFlags = HideFlags.NotEditable;
            mat.name = shader.name;
            meshRenderer.sharedMaterial = mat;
        }

        updateMeshSize ();
    }

    void OnDrawGizmos ()
    {
        if (enabled) {
            Renderer r = getRenderer ();
            if (r != null) {
                Gizmos.color = Color.magenta;
                Gizmos.matrix = Matrix4x4.TRS (transform.position, transform.rotation, transform.lossyScale);
                Bounds b = r.bounds;

                Gizmos.DrawWireCube (b.center, b.size);
            }
        }
    }

    protected virtual void OnDestroy ()
    {
        _instance = null;
    }

    protected virtual void Reset ()
    {
        Object[] objs = FindObjectsOfType (typeof(ClearStencilBufferComponent));
        if (objs != null && objs.Length > 1) {
            Debug.LogError ("ClearStencilBufferComponent is already in the scene!");
        }
    }

    public MeshRenderer getRenderer ()
    {
        return meshRenderer;
    }

    private void updateMeshSize ()
    {
        if (meshFilter != null) {
            float xMin = -_pivot.x * _size.x;
            float yMin = -_pivot.y * _size.y;
            float xMax = xMin + _size.x;
            float yMax = yMin + _size.y;
            
            vertices [0] = new Vector3 (xMin, yMax, 0);
            vertices [1] = new Vector3 (xMax, yMax, 0);
            vertices [2] = new Vector3 (xMax, yMin, 0);
            vertices [3] = new Vector3 (xMin, yMin, 0);
            meshFilter.sharedMesh.vertices = vertices;
            
            meshFilter.sharedMesh.RecalculateBounds ();
        }
    }

    private static ClearStencilBufferComponent findInstance ()
    {
        return FindObjectOfType (typeof(ClearStencilBufferComponent)) as ClearStencilBufferComponent;
    }
}

