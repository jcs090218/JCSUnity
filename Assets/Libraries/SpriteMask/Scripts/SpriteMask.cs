//----------------------------------------------
//                 SpriteMask
//          Copyright © 2015 TrueSoft
//             support@truesoft.pl
//----------------------------------------------

//#define SPRITE_MASK_DEBUG

#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5
#define BEFORE_4_6
#endif
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpriteMask : SpriteMaskingComponent
{
    public const string SHADER_SPRITE_DEFAULT = "SpriteMask/Default";
    public const string SHADER_SPRITE_DIFFUSE = "SpriteMask/Diffuse";
    public const string SHADER_MASK_ROOT = "SpriteMask/Mask";
    private const string STR_STENCIL = "_Stencil";
    private const string STR_STENCIL_COMPARISON = "_StencilComp";
    private const string STR_STENCIL_READ_MASK = "_StencilReadMask";
    private const string STR_COLOR_MASK = "_ColorMask";

    /// <summary>
    /// Max supported mask levels. Allowed values: 1 - 7
    /// Default value is 3.
    /// </summary>
    private const int MAX_LEVELS = 3;

    /// <summary>
    /// Base render queue used by this mask renderer. 
    /// </summary>
    private const int BASE_RENDER_QUEUE = 3000;

    /// <summary>
    /// List of object that should be masked by this mask. 
    /// To the list you should add object that are outside of this SpriteMask object. 
    /// It is used in the "flat hierarchy" scenario.
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs ("maskingRoots"), SerializeField]
    public List<Transform>
        maskedObjects = new List<Transform> ();

    /// <summary>
    /// The force default material on childs.
    /// </summary>
    public bool forceDefaultMaterialOnChilds = false;
    
    //-----------------------------------------------------------

    [SerializeField]
    private Type
        _type = Type.Rectangle;
    [SerializeField]
    private Vector2
        _size = new Vector2 (100f, 100f);
    [SerializeField]
    private Vector2
        _pivot = new Vector2 (0.5f, 0.5f);
    [SerializeField]
    private Texture2D
        _texture;
    [SerializeField]
    private Sprite
        _sprite;
    [SerializeField]
    private bool
        _inverted;
    [SerializeField]
    private bool
        _showMaskGraphics = false;

    //-----------------------------------------------------------

    private static bool[] stencilIds = new bool [256];
    private int propertyStencilReadMask = -1;
    private int propertyStencilComp = -1;
    private int propertyColorMask = -1;
    private int propertyStencil = -1;
    
    //-----------------------------------------------------------

#if !BEFORE_4_6
    private static List<Renderer> rendererComponents = new List<Renderer> ();
    private static List<SpriteMask> maskComponents = new List<SpriteMask> ();
    private static List<SpriteMaskingComponent> maskingComponents = new List<SpriteMaskingComponent> ();
    private static List<SpriteMaskingPart> maskingPartComponents = new List<SpriteMaskingPart> ();
    private static List<SkipMasking> skipMaskingComponents = new List<SkipMasking> ();
#endif
    private Vector3[] vertices = new Vector3[4];
    private Vector2[] uv = new Vector2[4] {
        new Vector2 (0, 1),
        new Vector2 (1, 1),
        new Vector2 (1, 0),
        new Vector2 (0, 0)
    };
    private int[] triangles = new int[6] {0,1,2,2,3,0};

// http://forum.unity3d.com/threads/released-sprite-mask-masking-system-for-unity-sprite.292453/page-3#post-2347416
//    [SerializeField]
    private Material
        _defaultSpriteMaterial;
    [SerializeField]
    private string
        _defaultSpriteShaderName = SHADER_SPRITE_DEFAULT;
    private SpriteRenderer spriteRenderer;
    private ClearStencilBufferComponent clearStencilComponent;
    private MeshRenderer meshRenderer;
    private bool isTypeApplyed = false;
    private Material _maskMaterial;
    private MeshFilter meshFilter;
    private int? parentStencilId;
    private string instanceId;
    private int _stencilId;
    private int _level = 0;

    //-----------------------------------------------------------

    /// <summary>
    /// Stencil ID value for this mask.
    /// </summary>
    public int stencilId {
        get {
            return _stencilId;
        }
    }
    
    /// <summary>
    /// Mask ID per level.
    /// </summary>
    public int maskIdPerLevel {
        get {
            return _stencilId & (255 >> MAX_LEVELS);
        }
    }

    /// <summary>
    /// Current masking level for this mask.
    /// </summary>
    public int level {
        get {
            return _level;
        }
    }

    /// <summary>
    /// Current used render queue value for this mask.
    /// </summary>
    public int maskRenderQueue {
        get {
            return BASE_RENDER_QUEUE + (100 * _level);
        }
    }

    /// <summary>
    /// Current used render queue value for sprite childs.
    /// All sprites must be renderer after mask render.
    /// </summary>
    public int spriteRenderQueue {
        get {
            return maskRenderQueue; // + 1;
        }
    }

    /// <summary>
    /// Is this mask at level 0?
    /// </summary>
    public bool isRoot {
        get {
            return _level == 0;
        }
    }

    [System.Obsolete("Use maskedObjects")]
    public List<Transform> maskingRoots {
        get {
            return maskedObjects;
        }
    }

    /// <summary>
    /// Is this mask a child of another mask?
    /// </summary>
    public bool isChild {
        get {
            return _level > 0;
        }
    }
    /// <summary>
    /// Is masking enabled on this component?
    /// </summary>
    public override bool isEnabled {
        get {
            return enabled;
        }
    }
    
    /// <summary>
    /// Current used masking pattern type.
    /// </summary>
    public Type type {
        get {
            return _type;
        }
        set {
            if (_type != value || !isTypeApplyed) {
                _type = value;

#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                applyType ();
            }
        }
    }

    /// <summary>
    /// Mask size. 
    /// Supported only on types: Rectangle, Texture.
    /// </summary>
    public Vector2 size {
        get {
            switch (_type) {
            case Type.Texture:
            case Type.Rectangle:
                return _size;
            case Type.Sprite:
                if (spriteRenderer != null && spriteRenderer.sprite != null) {
                    return spriteRenderer.sprite.bounds.size;
                }
                break;
            }
            
            return Vector2.zero;
        }
        set {
            if (!isTypeApplyed) {
                applyType ();
            }

            _size = value;

            if (_type == Type.Rectangle || _type == Type.Texture) {
                updateMeshSize ();
            } else {
                Debug.LogWarning (string.Concat ("Size change not supported on mask type: ", _type));
            }
        }
    }

    /// <summary>
    /// Mask pivot. 
    /// Supported only on types: Rectangle, Texture.
    /// </summary>
    public Vector2 pivot {
        get {
            switch (_type) {
            case Type.Texture:
            case Type.Rectangle:
                return _pivot;
            case Type.Sprite:
                if (spriteRenderer != null && spriteRenderer.sprite != null) {
                    Bounds b = spriteRenderer.sprite.bounds;
                    Vector2 m = b.min;
                    Vector2 s = b.size;
                    return new Vector2 (-m.x / s.x, -m.y / s.y);
                }
                break;
            }
            
            return Vector2.zero;
        }
        set {
            if (!isTypeApplyed) {
                applyType ();
            }

            _pivot = value;

            if (_type == Type.Rectangle || _type == Type.Texture) {
                updateMeshSize ();
            } else {
                Debug.LogWarning (string.Concat ("Pivot change not supported on mask type: ", _type));
            }
        }
    }

    /// <summary>
    /// Sprite used for masking. 
    /// Supported only on type: Sprite.
    /// </summary>
    public Sprite sprite {
        get {
            return _sprite;
        }
        set {
            if (!isTypeApplyed) {
                applyType ();
            }

            if (_type == Type.Sprite) {
                spriteRenderer.sprite = _sprite = value;
            } else {
                Debug.LogWarning (string.Concat ("Sprite change not supported on mask type: ", _type));
            }
        }
    }

    /// <summary>
    /// Texture used for masking. 
    /// Supported only on type: Texture.
    /// </summary>
    public Texture2D texture {
        get {
            return _texture;
        }
        set {
            if (!isTypeApplyed) {
                applyType ();
            }

            if (_type == Type.Texture) {
                maskMaterial.mainTexture = _texture = value;
            } else {
                Debug.LogWarning (string.Concat ("Texture change not supported on mask type: ", _type));
            }
        }
    }

    /// <summary>
    /// Inverted masking mode.
    /// It is an experimental feature. It may not work properly in some multi level use cases.
    /// </summary>
    public bool inverted {
        get {
            return _inverted;
        }
        set {
            if (_inverted != value) {
                _inverted = value;

                updateSprites ();
            }
        }
    }

    /// <summary>
    /// Display this mask graphics.
    /// </summary>
    public bool showMaskGraphics {
        get {
            return _showMaskGraphics;
        }
        set {
            if (_showMaskGraphics != value) {
                _showMaskGraphics = value;

                maskMaterial.SetFloat (propertyColorMask, _showMaskGraphics ? 15 : 0);
            }
        }
    }

    /// <summary>
    /// Gets or sets default sprite shader.
    /// </summary>
    public Shader spritesShader {
        get {
            return defaultSpriteMaterial.shader;
        }
        set {
            if (value == null) {
                Debug.LogWarning ("Shader is null");
                return;
            }

            if (!value.isSupported) {
                Debug.LogWarning ("Shader '" + value.name + "' not supported!");
                return;
            }

            Material m = defaultSpriteMaterial;

            if (m.shader != value) {
                Shader s = m.shader;
                m.shader = value;
                if (!hasStencilSupport (m)) {
                    Debug.LogWarning ("Shader '" + value.name + "' doesn't support Stencil buffer");
                    m.shader = s;
                }
                _defaultSpriteShaderName = m.shader.name;
            }
        }
    }

    /// <summary>
    /// Default material used for all childs of type Sprite.
    /// </summary>
    private Material defaultSpriteMaterial {
        get {
            if (_defaultSpriteMaterial == null) {
                Shader shader = Shader.Find (_defaultSpriteShaderName);
                _defaultSpriteMaterial = new Material (shader);
//                _defaultSpriteMaterial.hideFlags = HideFlags.HideAndDontSave;
                _defaultSpriteMaterial.name = string.Concat (shader.name, " OWNER_ID:", instanceId);
            }
            return _defaultSpriteMaterial;
        }
    }

    /// <summary>
    /// Material used by this mask.
    /// </summary>
    private Material maskMaterial {
        get {
            if (_maskMaterial == null) {
                Shader shader = Shader.Find (SHADER_MASK_ROOT);
                _maskMaterial = new Material (shader);
                _maskMaterial.hideFlags = HideFlags.HideAndDontSave;
                _maskMaterial.name = string.Concat (shader.name, " OWNER_ID:", instanceId);
                _maskMaterial.SetFloat (propertyColorMask, _showMaskGraphics ? 15 : 0);
            }
            return _maskMaterial;
        }
    }

    //-----------------------------------------------------------

    void Awake ()
    {
#if SPRITE_MASK_DEBUG
        log ("Awake >>>");
#endif
        instanceId = GetInstanceID ().ToString ();
        owner = this;

        propertyStencil = Shader.PropertyToID (STR_STENCIL);
        propertyStencilComp = Shader.PropertyToID (STR_STENCIL_COMPARISON);
        propertyStencilReadMask = Shader.PropertyToID (STR_STENCIL_READ_MASK);
        propertyColorMask = Shader.PropertyToID (STR_COLOR_MASK);

        clearChildsMaterial (transform);

        spriteRenderer = GetComponent <SpriteRenderer> ();
        meshRenderer = GetComponent <MeshRenderer> ();
        meshFilter = GetComponent <MeshFilter> ();
        if (meshFilter != null && meshFilter.sharedMesh != null) {
            meshFilter.sharedMesh = null;
        }

//        if (!enabled) {
//            OnEnable ();
//            Start ();
//            OnDisable ();
//        }
#if SPRITE_MASK_DEBUG
        log ("Awake <<<");
#endif
    }
    
    void OnEnable ()
    {
#if SPRITE_MASK_DEBUG
        log ("OnEnable >>>");
#endif
        parentStencilId = null;

        Renderer r = getRenderer (this);
        if (r != null) {
            r.enabled = true;
        }

        if (isTypeApplyed) {
            update ();
        }

#if SPRITE_MASK_DEBUG
        log ("OnEnable <<<");
#endif
    }

    void Start ()
    {
#if SPRITE_MASK_DEBUG
        log ("Start >>>");
#endif

        if (!isTypeApplyed) {
            applyType ();
        }

        update ();

#if SPRITE_MASK_DEBUG
        log ("Start <<<");
#endif
    }

#if UNITY_EDITOR
    void Update ()
    {
        if (!Application.isPlaying) {
            if (!isTypeApplyed) {
                applyType ();
            }

            update ();
        }
    }

    void RequestTypeApply () {
        isTypeApplyed = false;
    }
#endif

    void OnDisable ()
    {
#if SPRITE_MASK_DEBUG
        log ("OnDisable >>>");
#endif
        // Clear stencil ID
        if (_stencilId > 0) {
            releaseId (_stencilId);
            _stencilId = 0;
        }

        if (gameObject.activeInHierarchy) {
            // Just disabled

            Renderer r = getRenderer (this);
            if (r != null) {
                // Turn off renderer
                r.enabled = false;
            }

            // If there is enabled parent mask, then all childs sprites should use its stencil ID
            SpriteMask parentMask = getParentMask (transform);
            if (parentMask != null) {
                parentStencilId = parentMask.stencilId;
            } else {
                parentStencilId = null;
            }

            updateSprites ();
            updateMaskingComponents ();
        }

#if SPRITE_MASK_DEBUG
        log ("OnDisable <<<");
#endif
    }

    //-----------------------------------------------------------

    /// <summary>
    /// Call this function after you change mask hierarchy at runtime.
    /// </summary>
    [ContextMenu ("Update Mask")]
    public void updateMask ()
    {
#if SPRITE_MASK_DEBUG
        log ("updateMask >>>");
#endif
        if (!isTypeApplyed) {
            return;
        }

        if (_stencilId > 0) {
            releaseId (_stencilId);
            _stencilId = 0;
        }
        _level = 0;

        // Get mask level
        Transform t = transform.parent;
        while (t != null) {
            SpriteMaskingComponent masking = getMaskingComponent (t);
            if (masking != null && masking.isEnabled) {
                _level++;
            }
            t = t.parent;
        }

        int maxLevel = MAX_LEVELS - 1;
        if (_level > maxLevel) {
            Debug.LogError ("Maximum number of mask levels has been exceeded: max=" + maxLevel + " current=" + _level);
            _level = maxLevel;
        }

        _stencilId = getId (_level);

#if SPRITE_MASK_DEBUG
        log ("updateMask stencil=" + _stencilId + " _level=" + _level);
#endif

        if (_stencilId == -1) {
            Debug.LogError ("Maximum number of mask per levels has been exceeded: " + (255 >> MAX_LEVELS));
            _stencilId = 0;
        }

        Material m = maskMaterial;

        int readMask;
        CompareFunction comp;
        if (isRoot) {
            readMask = 255;
            comp = CompareFunction.Always;
        } else {
            readMask = 255 >> (_level - 1);
            comp = CompareFunction.Less;
        }

        m.renderQueue = maskRenderQueue;
        updateMaterial (m, _stencilId, comp, readMask);

#if SPRITE_MASK_DEBUG
        log ("updateMask <<<");
#endif
    }

    /// <summary>
    /// Update all objects from maskedObject list. After you add 
    /// object to maskedObjects you should call this method to 
    /// get all objects from the list to be masked.
    /// </summary>
    public void updateMaskingComponents ()
    {
        if (maskedObjects == null || maskedObjects.Count == 0) {
            return;
        }
        
        bool removeEmptyElements = false;
        for (int i = 0; i < this.maskedObjects.Count; i++) {
            if (maskedObjects [i] == null) {
                removeEmptyElements = true;
                continue;
            }

            SpriteMaskingComponent masking = getMaskingComponent (maskedObjects [i]);
            if (masking != this) {
                if (masking == null) {
                    masking = this.maskedObjects [i].gameObject.AddComponent<SpriteMaskingComponent> ();
                    masking.owner = this;
                } else {
                    if (masking.owner == null) {
                        masking.owner = this;
                    } else if (masking.owner != this) {
                        continue;
                    }
                }
                updateSprites (maskedObjects [i]);
            } else {
                maskedObjects [i] = null;
                removeEmptyElements = true;
            }
        }
        
        if (removeEmptyElements) {
            for (int i=0; i<maskedObjects.Count; i++) {
                if (maskedObjects [i] == null) {
                    maskedObjects.RemoveAt (i);
                    i = 0;
                }
            }
        }
    }

    /// <summary>
    /// Call this function after you change/attach child sprites at runtime.
    /// 
    /// NOTE: Calling this method at runtime can be 'expensive', because it runs update an all child object of this mask. 
    /// If you want to update a specific sprite, please use method 'updateSprites (Transform trans)',
    /// with updates that specific sprite and its childs sprites.
    /// </summary>
    [ContextMenu ("Update Sprites")]
    public void updateSprites ()
    {
        updateSprites (transform);
    }

    /// <summary>
    /// Updates sprite that is on the 'trans' and all sprites that are "under" (are child of) the 'trans' object.
    /// </summary>
    public void updateSprites (Transform trans)
    {
#if SPRITE_MASK_DEBUG
        log ("updateSprites >>>");
#endif

        if (!isTypeApplyed) {
            return;
        }

        int stencil;
        CompareFunction comp;
        if (parentStencilId.HasValue) {
            stencil = parentStencilId.Value;
            comp = enabled && _inverted ? CompareFunction.Less : CompareFunction.Equal;
        } else {
            stencil = _stencilId;
            comp = enabled ? (_inverted ? (isRoot ? CompareFunction.NotEqual : CompareFunction.Less) : CompareFunction.Equal) : CompareFunction.Always;
        }

        doUpdateSprite (trans, comp, stencil);

#if SPRITE_MASK_DEBUG
        log ("updateSprites <<<");
#endif
    }

    /// <summary>
    /// Call this function after you change mask hierarchy or attach child sprites at runtime.
    /// </summary>
    public void update ()
    {
        updateMask ();
        updateSprites ();
        updateMaskingComponents ();
    }

    /// <summary>
    /// Find mask component for <code>t</code> and call <code>updateSprites(t)</code>.
    /// </summary>
    public static SpriteMask updateFor (Transform t)
    {
        SpriteMask mask = getParentMask (t);
        if (mask != null) {
            mask.updateSprites (t);
        }

        return mask;
    }

    /// <summary>
    /// Find mask component for <code>t</code>.
    /// </summary>
    public static SpriteMask getParentMask (Transform t)
    {
        t = t.parent;
        while (t != null) {
            SpriteMaskingComponent masking = getMaskingComponent (t);
            if (masking != null && masking.isEnabled) {
                return masking.owner;
            }
            t = t.parent;
        }
        
        return null;
    }

    //-----------------------------------------------------------

    private void applyType ()
    {
#if SPRITE_MASK_DEBUG
        log ("applyType >>>");
#endif

        switch (_type) {
        case Type.Rectangle:
            _texture = null;
            _sprite = null;
            maskMaterial.mainTexture = null;
            destroySpriteComponents ();
            
            ensureMeshComponents ();
            updateMeshSize ();
            break;
        case Type.Sprite:
            _texture = null;
            maskMaterial.mainTexture = null;
            destroyMeshComponents ();
            
            ensureSpriteRenderer ();
            spriteRenderer.sprite = _sprite;
            break;
        case Type.Texture:
            _sprite = null;
            destroySpriteComponents ();
            
            ensureMeshComponents ();
            updateMeshSize ();
            maskMaterial.mainTexture = _texture;
            break;
        case Type.None:
            _texture = null;
            _sprite = null;
            maskMaterial.mainTexture = null;
            destroySpriteComponents ();
            destroyMeshComponents ();
            break;
        }
        
        isTypeApplyed = true;

#if SPRITE_MASK_DEBUG
        log ("applyType <<<");
#endif
    }

    protected override void doUpdateSprite (Transform t, CompareFunction comp, int stencil)
    {
#if SPRITE_MASK_DEBUG
        log ("doUpdateSprite >>> t=" + t.name + " comp=" + comp + " stencil=" + stencil + " t.childsCount=" + t.childCount);
#endif

        if (hasSkipMasking (t)) {
            return;
        }

        if (hasMaskingPart (t)) {
            // 't' will take a part in masking
            Renderer r = getRenderer (t);
            if (r != null) {
                r.sharedMaterial = maskMaterial;
            }
        } else {
            SpriteMaskingComponent masking = getMaskingComponent (t);

            bool updateRenderer = true;
            if (masking != null) {
                if (masking == this) {
                    updateRenderer = false;
                } else if (masking.owner == this) {
                    updateRenderer = true;
                } else if (masking is SpriteMask) {
                    updateRenderer = false;
                    if (masking.isEnabled) {
#if UNITY_EDITOR
                    if (Application.isPlaying) // Don't update in edit mode. Update() will be called on child masks.
#endif
                        ((SpriteMask)masking).update ();
                        return;
                    }
                } else if (masking.isEnabled) {
                    return;
                }
            }

            if (updateRenderer) {
                Renderer r = getRenderer (t);
#if SPRITE_MASK_DEBUG
                log ("doUpdateSprite: renderer=" + r);
#endif
                if (r != null) {
                    Material[] ms = r.sharedMaterials;

                    if (ms.Length > 1) {
                        // More than one material
                        for (int i=0; i<ms.Length; i++) {
                            if (ms [i] == null || !hasStencilSupport (ms [i]) || forceDefaultMaterialOnChilds) {
                                ms [i].renderQueue = spriteRenderQueue;
                                updateMaterial (ms [i], stencil, comp, null);
                            }
                        }
                    } else {
                        // Has zero or one matrial
                        Material m;
                        if (ms.Length == 0) {
                            // No material on renderer
                            m = defaultSpriteMaterial;
                            r.sharedMaterial = m;
                        } else {
                            m = ms [0];
                            if (m == null || !hasStencilSupport (m) || forceDefaultMaterialOnChilds) {
                                m = defaultSpriteMaterial;
                                r.sharedMaterial = m;
                            }
                        }

                        m.renderQueue = spriteRenderQueue;
                        updateMaterial (m, stencil, comp, null);
                    }
                }
            }

            int childsCount = t.childCount;
            if (childsCount > 0) {
                for (int i=0; i<childsCount; i++) {
                    doUpdateSprite (t.GetChild (i), comp, stencil);
                }
            }
        }
#if SPRITE_MASK_DEBUG
        log ("doUpdateSprite <<<");
#endif
    }

    private void clearChildsMaterial (Transform t)
    {
        int childsCount = t.childCount;
        if (childsCount == 0) {
            return;
        }

        for (int i=0; i<childsCount; i++) {
            Transform child = t.GetChild (i);

            SpriteMaskingComponent masking = getMaskingComponent (child);
            if (masking != null) {
                continue;
            }

            Renderer r = getRenderer (child);
            if (r != null) {
                Material m = r.sharedMaterial;
                if (m != null) {
                    string ownerId = readOwnerId (m.name);
                    if (ownerId != null && !ownerId.Equals (instanceId)) {
                        // This material does not belong to this mask
                        r.sharedMaterial = null;
                    }
                }
            }
            
            clearChildsMaterial (child);
        }
    }

    private void updateMaterial (Material m, int? stencil, CompareFunction? comp, int? readMask)
    {
        if (stencil.HasValue) {
            m.SetInt (propertyStencil, stencil.Value);
        }
        
        if (comp.HasValue) {
            m.SetInt (propertyStencilComp, (int)comp.Value);
        }
        
        if (readMask.HasValue) {
            m.SetInt (propertyStencilReadMask, readMask.Value);
        }
    }

    private int getId (int level)
    {
        int v = 128 >> level;
        int maxIdsPerLevel = 255 >> MAX_LEVELS;

        for (int i=0; i<maxIdsPerLevel; i++) {
            int id = v + i;

            if (!stencilIds [id]) {
                stencilIds [id] = true;
                return id;
            }
        }

        return -1;
    }

    private void releaseId (int id)
    {
        stencilIds [id] = false;
    }

    private bool hasStencilSupport (Material m)
    {
        return m.HasProperty (propertyStencil) && m.HasProperty (propertyStencilComp);
    }

    private string readOwnerId (string str)
    {
        int idx = str.IndexOf ("OWNER_ID");
        if (idx != -1) {
            return str.Substring (idx + 9);
        } else {
            return null;
        }
    }

    private void destroySpriteComponents ()
    {
        if (spriteRenderer != null) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate (spriteRenderer);
            else
#endif
            Destroy (spriteRenderer);
            spriteRenderer = null;
        }
    }

    private void destroyMeshComponents ()
    {
        if (meshFilter != null) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate (meshFilter);
            else
#endif
            Destroy (meshFilter);
            meshFilter = null;
        }

        if (meshRenderer != null) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate (meshRenderer);
            else
#endif
            Destroy (meshRenderer);
            meshRenderer = null;
        }
    }

    private void ensureSpriteRenderer ()
    {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent <SpriteRenderer> ();

            if (spriteRenderer == null) {
                spriteRenderer = gameObject.AddComponent <SpriteRenderer> ();
            }
        }
        spriteRenderer.sharedMaterial = maskMaterial;
    }

    private void ensureMeshComponents ()
    {
        if (meshFilter == null) {
            meshFilter = GetComponent <MeshFilter> ();

            if (meshFilter == null) {
                meshFilter = gameObject.AddComponent <MeshFilter> ();
            }
        }

        if (meshFilter.sharedMesh == null) {
            Mesh mesh = new Mesh ();
            mesh.hideFlags = HideFlags.HideAndDontSave;
            mesh.name = string.Concat ("RectMesh OWNER_ID:", instanceId);
            mesh.MarkDynamic ();

            meshFilter.sharedMesh = mesh;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            
            mesh.RecalculateNormals ();
        }

        if (meshRenderer == null) {
            meshRenderer = GetComponent <MeshRenderer> ();
            
            if (meshRenderer == null) {
                meshRenderer = gameObject.AddComponent <MeshRenderer> ();
            }
        }
        meshRenderer.sharedMaterial = maskMaterial;
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

    public Renderer getRenderer ()
    {
        return getRenderer (this);
    }

    private static Renderer getRenderer (Component c)
    {
#if BEFORE_4_6
        return c.GetComponent <Renderer> ();
#else
        c.GetComponents <Renderer> (rendererComponents);
        Renderer r = rendererComponents.Count > 0 ? rendererComponents [0] : null;
        rendererComponents.Clear ();
        return r;
#endif
    }
    
    private static bool hasMaskingPart (Component t)
    {
#if BEFORE_4_6
        return t.GetComponent <SpriteMaskingPart> () != null;
#else
        t.GetComponents <SpriteMaskingPart> (maskingPartComponents);
        bool b = maskingPartComponents.Count > 0;
        maskingPartComponents.Clear ();
        return b;
#endif
    }

    private static bool hasSkipMasking (Component t)
    {
#if BEFORE_4_6
        return t.GetComponent <SkipMasking> () != null;
#else
        t.GetComponents <SkipMasking> (skipMaskingComponents);
        bool b = skipMaskingComponents.Count > 0;
        skipMaskingComponents.Clear ();
        return b;
#endif
    }
    
    private static SpriteMaskingComponent getMaskingComponent (Component t)
    {
#if BEFORE_4_6
        return t.GetComponent <SpriteMaskingComponent> ();
#else
        t.GetComponents <SpriteMaskingComponent> (maskingComponents);
        SpriteMaskingComponent smc = maskingComponents.Count == 1 ? maskingComponents [0] : null;
        maskingComponents.Clear ();
        return smc;
#endif
    }

    private static SpriteMask getSpriteMask (Component t)
    {
#if BEFORE_4_6
        return t.GetComponent <SpriteMask> ();
#else
        t.GetComponents <SpriteMask> (maskComponents);
        SpriteMask sm = maskComponents.Count == 1 ? maskComponents [0] : null;
        maskComponents.Clear ();
        return sm;
#endif
    }

#if UNITY_EDITOR
    void OnDrawGizmos ()
    {
        Gizmos.color = enabled ? Color.red : Color.grey;
        Gizmos.matrix = Matrix4x4.TRS (transform.position, transform.rotation, transform.lossyScale);

        Vector2 s = size;
        Vector2 p = pivot;
        Vector2 c = new Vector2 ((-p.x * s.x) + (s.x / 2f), (-p.y * s.y) + (s.y / 2f));

        Gizmos.DrawWireCube (c, s);
    }

    [ContextMenu ("Print used Stencil ID's")]
    private void printUsedStencilIds () 
    {
        string ids = "";
        for (int i=0;i<stencilIds.Length;i++) {
            if (stencilIds [i]) {
                ids = string.Concat (ids, " ", i.ToString ());
            }
        }
        print ("Used Stencil IDs:" + ids);
    }
#endif

#if SPRITE_MASK_DEBUG
    private void log (string msg)
    {
        print ("[" + name + ":" + GetInstanceID () + "] @" + Time.frameCount + ": " + msg);
    }
#endif

    public enum Type
    {
        Rectangle,
        Sprite,
        Texture,
//        CustomMesh,
        None = 100
    }
}
