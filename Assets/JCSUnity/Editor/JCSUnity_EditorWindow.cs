/**
 * $File: JCSUnity_EditorWindow.cs $
 * $Date: 2017-01-22 04:42:16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Main editor window for JCSUnity.
    /// </summary>
    public class JCSUnity_EditorWindow : EditorWindow
    {
        /* Variables*/

        public const string MI_BaseName = "Tools/JCSUnity";

        public const int MI_BasePriority = -24;

        public static JCSUnity_EditorWindow instance = null;

        public static string NAME
        {
            get
            {
                JCSUnity_About.ReadINIFile();
                return JCSUnity_About.EDITOR_INI["editor_title"];
            }
        }

        private static bool prefsLoaded = false;

        private bool mFO_Project = false;
        private bool mFO_Basic = false;
        private bool mFO_Scene = false;
        private bool mFO_UI = false;
        private bool mFO_Input = false;
        private bool mFO_Effect = false;
        private bool mFO_ARVR = false;

        public string PROJECT_NAME = "_Project";

        public const string PROJECT_NAME_SUFFIX = "";

        public string[] ProjectSubFolders = {
            "Animations",
            "Editor",
            "Materials",
            "Models",
            "Movies",
            "Scenes",
            "Scripts",
            "Shaders",
            "Sounds",
            "Sprites",
        };

        /* Setter & Getter */

        /* Functions */

        private void OnEnable()
        {
            instance = this;
        }

        private void OnGUI()
        {
            JCSUnity_About.ReadINIFile();

            Init();
            Draw();
            SavePref();
        }

        private void Init()
        {
            if (prefsLoaded)
                return;

            JCS_InputController.GAMEPAD_COUNT = EditorPrefs.GetInt(JCSUnity_EditortUtil.FormKey("GAMEPAD_COUNT"), 0);
            JCS_InputController.SelectGamepadType = EditorPrefs.GetInt(JCSUnity_EditortUtil.FormKey("SelectGamepadType"), 0);

            prefsLoaded = true;
        }

        private void Draw()
        {
            mFO_Project = JCSUnity_EditortUtil.Foldout(mFO_Project, "Project", Part_Project, "d_Folder Icon");
            mFO_Basic = JCSUnity_EditortUtil.Foldout(mFO_Basic, "Basic", Part_Basic, "_Popup");
            mFO_Scene = JCSUnity_EditortUtil.Foldout(mFO_Scene, "Scene", Part_Scene, "d_ToggleUVOverlay");
            mFO_UI = JCSUnity_EditortUtil.Foldout(mFO_UI, "UI", Part_UI, "d_UnityEditor.SceneHierarchyWindow");
            mFO_Input = JCSUnity_EditortUtil.Foldout(mFO_Input, "Input", Part_Input, "d_UnityEditor.GameView");
            mFO_Effect = JCSUnity_EditortUtil.Foldout(mFO_Effect, "Effect", Part_Effect, "ParticleSystem Gizmo");
            mFO_ARVR = JCSUnity_EditortUtil.Foldout(mFO_ARVR, "AR / VR", Part_ARVR, "animationvisibilitytoggleon");
        }

        private void SavePref()
        {
            EditorPrefs.SetInt(JCSUnity_EditortUtil.FormKey("GAMEPAD_COUNT"), JCS_InputController.GAMEPAD_COUNT);
            EditorPrefs.SetInt(JCSUnity_EditortUtil.FormKey("SelectGamepadType"), JCS_InputController.SelectGamepadType);
        }

        /// <summary>
        /// Initialize the one click serialize part buttons.
        /// </summary>
        private void Part_Scene()
        {
            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                if (GUILayout.Button("Convert to 2D scene"))
                    ConvertTo2D();

                if (GUILayout.Button("Convert to 3D scene"))
                    ConvertTo3D();
            });
        }

        /// <summary>
        /// Initialize the base object part buttons.
        /// </summary>
        private void Part_Basic()
        {
            GUILayout.Label("Managers / Settings", EditorStyles.boldLabel);

            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                if (GUILayout.Button("Create Settings"))
                    CreateSettings();

                if (GUILayout.Button("Create Managers"))
                    CreateManagers();
            });

            GUILayout.Label("Camera", EditorStyles.boldLabel);

            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                if (GUILayout.Button("Create 2D camera"))
                    Create2DCamera();

                if (GUILayout.Button("Create 3D camera"))
                    Create3DCamera();
            });

            GUILayout.Label("Canvas", EditorStyles.boldLabel);

            if (GUILayout.Button("Create Canvas"))
                CreateJCSCanvas();

            GUILayout.Label("Background Music", EditorStyles.boldLabel);

            if (GUILayout.Button("Create BGM Player"))
                CreateBGMPlayer();

            GUILayout.Label("Debug Tools", EditorStyles.boldLabel);

            if (GUILayout.Button("Create Debug Tools"))
                CreateDebugTools();
        }

        /// <summary>
        /// Compile the GUI part to Unity's GUI inspector.
        /// </summary>
        private void Part_UI()
        {
            GUILayout.Label("Cursor", EditorStyles.boldLabel);

            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                if (GUILayout.Button("Create 2D Cursor"))
                    Create2DCurosr();

                if (GUILayout.Button("Create 3D Cursor"))
                    Create3DCurosr();
            });

            GUILayout.Label("Panel", EditorStyles.boldLabel);

            if (GUILayout.Button("Create Base Panel"))
                CreateBasePanel();

            if (GUILayout.Button("Create Dialogue Panel"))
                CreateDialoguePanel();

            if (GUILayout.Button("Create Tween Panel"))
                CreateTweenPanel();

            if (GUILayout.Button("Create Slide Panel 9x9 - 16:9"))
                CreateSlidePanel();


            GUILayout.Label("Undo/Redo", EditorStyles.boldLabel);

            if (GUILayout.Button("Create Undo Redo System"))
                CreateUndoRedoSystem();
        }

        /// <summary>
        /// Compile the Effect inspector.
        /// </summary>
        private void Part_Effect()
        {
            GUILayout.Label("Damage Text", EditorStyles.boldLabel);

            if (GUILayout.Button("Create mix damage pool"))
                CreateMixDamageTextPool();
        }

        /// <summary>
        /// Compile the AR/VR part to GUI inspector.
        /// </summary>
        private void Part_ARVR()
        {
            GUILayout.Label("N/A...");
        }

        /// <summary>
        /// Compile the Input part to Input inspector.
        /// </summary>
        private void Part_Input()
        {
            GUILayout.Label("Gamepad", EditorStyles.boldLabel);

            JCS_InputController.SelectGamepadType = EditorGUILayout.Popup("Gamepad Type", JCS_InputController.SelectGamepadType, JCS_InputController.GamepadPlatform);
            JCS_InputController.GAMEPAD_COUNT = (int)EditorGUILayout.Slider("Gamepad Count", JCS_InputController.GAMEPAD_COUNT, 0, JCS_InputSettings.MAX_JOYSTICK_COUNT);

            GUILayout.Label("Input Manager", EditorStyles.boldLabel);

            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                if (GUILayout.Button("Update"))
                    UpdateInputManager();

                if (GUILayout.Button("Clear"))
                    ClearInputManager();

                if (GUILayout.Button("Revert"))
                    RevertDefaultInputManager();
            });
        }

        /// <summary>
        /// Compile the Tool part to Tool inspector.
        /// </summary>
        private void Part_Project()
        {
            /* Project Name */
            {
                // Provide default project name.
                if (instance.PROJECT_NAME == "")
                    instance.PROJECT_NAME = GetProjectName();

                instance.PROJECT_NAME = EditorGUILayout.TextField("Project Name: ", instance.PROJECT_NAME);
            }

            /* List of project sub folders. */
            {
                ScriptableObject target = this;
                SerializedObject so = new SerializedObject(target);
                SerializedProperty stringsProperty = so.FindProperty("ProjectSubFolders");

                EditorGUILayout.PropertyField(stringsProperty, true);
                so.ApplyModifiedProperties();
            }


            if (GUILayout.Button("Create project assets folder"))
                CreateProjectAssetsFolder();
        }


        /// <summary>
        /// Main editor window initialize function.
        /// </summary>
        [MenuItem(MI_BaseName + "/Window", false, MI_BasePriority + 1)]
        private static void JCSUnityEditor()
        {
            var window = GetWindow<JCSUnity_EditorWindow>(false, NAME, true);
            window.Show();
        }

        /// <summary>
        /// Serialize the current scene into 2D style.
        /// </summary>
        [MenuItem(MI_BaseName + "/Scene/Convert to 2D scene", false, MI_BasePriority + 2)]
        private static void ConvertTo2D()
        {
            // create settings
            CreateSettings();

            // create managers
            CreateManagers();

            Create2DCamera();

            // BGM player
            CreateBGMPlayer();

            // create canvas
            GameObject canvasObj = CreateJCSCanvas();

            CreateDescribePanel(canvasObj);
        }

        /// <summary>
        /// Serialize the current scene into 3D style.
        /// </summary>
        [MenuItem(MI_BaseName + "/Scene/Convert to 3D scene", false, MI_BasePriority + 2)]
        private static void ConvertTo3D()
        {
            // create settings
            CreateSettings();

            // create managers
            CreateManagers();

            // create 3d camera
            Create3DCamera();

            // BGM player
            CreateBGMPlayer();

            // create canvas
            GameObject canvasObj = CreateJCSCanvas();

            CreateDescribePanel(canvasObj);
        }

        private static void CreateDescribePanel(GameObject parent)
        {
            const string desc_path = "UI/JCS Describe Panel";

            GameObject desc_obj = JCS_Util.Instantiate(desc_path);
            JCS_Util.RemoveCloneString(desc_obj);

            desc_obj.transform.SetParent(parent.transform);
            desc_obj.transform.localPosition = Vector3.zero;
            desc_obj.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Create managers for 3d game combine with JCSUnity.
        /// </summary>
        [MenuItem(MI_BaseName + "/Basic/Create Managers", false, MI_BasePriority + 10)]
        private static GameObject CreateManagers()
        {
            const string manager_path = "JCS_Managers";
            GameObject gameObj = CreateHierarchyObject(manager_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS Managers");

            return gameObj;
        }

        /// <summary>
        /// Create settings for 3d game combine with JCSUnity.
        /// </summary>
        [MenuItem(MI_BaseName + "/Basic/Create Settings", false, MI_BasePriority + 10)]
        private static GameObject CreateSettings()
        {
            const string setting_path = "JCS_Settings";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS Settings");

            return gameObj;
        }

        /// <summary>
        /// BGM player for game.
        /// </summary>
        [MenuItem(MI_BaseName + "/Basic/Create BGM Player", false, MI_BasePriority + 11)]
        private static void CreateBGMPlayer()
        {
            const string player_path = "Sound/JCS_BGMPlayer";
            GameObject gameObj = CreateHierarchyObject(player_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create BGM Player");
        }

        /// <summary>
        /// Debug tool using in JCSUnity.
        /// </summary>
        [MenuItem(MI_BaseName + "/Basic/Create Debug Tools", false, MI_BasePriority + 12)]
        private static void CreateDebugTools()
        {
            const string tools_path = "Tools/JCS_Tools";
            GameObject gameObj = CreateHierarchyObject(tools_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create Debug Tools");
        }

        /// <summary>
        /// Create settings for 3d game combine with JCSUnity.
        /// </summary>
        [MenuItem(MI_BaseName + "/Input/Update", false, MI_BasePriority + 15)]
        private static void UpdateInputManager()
        {
            JCS_InputController.SetupInputManager();
        }

        [MenuItem(MI_BaseName + "/Input/Clear", false, MI_BasePriority + 15)]
        private static void ClearInputManager()
        {
            JCS_InputController.ClearInputManagerSettings();
        }

        [MenuItem(MI_BaseName + "/Input/Revert", false, MI_BasePriority + 15)]
        private static void RevertDefaultInputManager()
        {
            JCS_InputController.DefaultInputManagerSettings();
        }

        /// <summary>
        /// Create canvas for JCSUnity.
        /// </summary>
        private static GameObject CreateJCSCanvas()
        {
            /* Canvas */
            const string canvas_path = "UI/JCS_Canvas";
            GameObject canvasObj = CreateHierarchyObject(canvas_path);

            var canvas = canvasObj.GetComponent<Canvas>();
            {
                // auto assign camera.
                canvas.worldCamera = Camera.main;

                // Place canvas somewhere closer to the camera.
                //
                // `10.0f` is just a good number for the plane distance.
                canvas.planeDistance = 10.0f;
            }

            Undo.RegisterCreatedObjectUndo(canvasObj, "Create JCS Canvas");

            /* Event System */
            const string eventSystem_path = "UI/EventSystem";
            GameObject evtSystemObj = CreateHierarchyObject(eventSystem_path);

            Undo.RegisterCreatedObjectUndo(evtSystemObj, "Create Event System");

            return canvasObj;
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        [MenuItem(MI_BaseName + "/Tool/Create project assets folder", false, MI_BasePriority + 20)]
        private static void CreateProjectAssetsFolder()
        {
            string parentFolder = "Assets";
            string newFolderName = instance.PROJECT_NAME + PROJECT_NAME_SUFFIX;

            string assetsPath = Application.dataPath + "/";
            string newProjectPath = assetsPath + newFolderName + "/";

            if (!Directory.Exists(newProjectPath))
                AssetDatabase.CreateFolder(parentFolder, newFolderName);

            foreach (string subFolderName in instance.ProjectSubFolders)
            {
                string newProjectName = parentFolder + "/" + newFolderName;
                string newSubFolderPath = newProjectPath + subFolderName + "/";

                if (!Directory.Exists(newSubFolderPath))
                    AssetDatabase.CreateFolder(newProjectName, subFolderName);
            }
        }

        /// <summary>
        /// Update JCSUnity
        /// </summary>
        [MenuItem(MI_BaseName + "/Check for Update", false, MI_BasePriority + 75)]
        private static void UpdateJCSUnity()
        {
            // TODO(jenchieh): check framework need to update or not?
            bool upToDate = true;
            const string title = "Check for Update - JCSUnity";

            if (upToDate)
            {
                EditorUtility.DisplayDialog(title, "Already up to date.", "Close");
            }
            // TODO(jenchieh): not up to date...
            else
            {
                bool option = EditorUtility.DisplayDialog(title, "", "Close");

                if (option)
                {

                }
                else
                {

                }
            }
        }

        /**
         * GUI
         */

        /// <summary>
        /// Create the cursor game object.
        /// </summary>
        private static GameObject Create2DCurosr()
        {
            const string setting_path = "UI/JCS_2DCursor";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create 3D Cursor");

            gameObj.name = "_2DCursor (Created)";

            return gameObj;
        }

        /// <summary>
        /// Create the cursor game object.
        /// </summary>
        private static GameObject Create3DCurosr()
        {
            const string setting_path = "UI/JCS_3DCursor";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create 3D Cursor");

            gameObj.name = "_3DCursor (Created)";

            return gameObj;
        }

        /// <summary>
        /// Create the clean base gui panel for JCSUnity and add in under to 
        /// the canvas.
        /// 
        /// Need:
        ///     1) JCS_Canvas
        /// in the scene before create base panel.
        /// </summary>
        private static GameObject CreateBasePanel()
        {
            var canvas = JCS_Util.FindObjectByType(typeof(JCS_Canvas)) as JCS_Canvas;
            if (canvas == null)
            {
                Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            const string setting_path = "UI/JCS_BasePanel";
            GameObject basePanel = CreateHierarchyObjectUnderCanvas(setting_path);

            Undo.RegisterCreatedObjectUndo(basePanel, "Create Base Panel");

            basePanel.transform.localScale = Vector3.one;
            basePanel.name = "_BasePanel (Created)";

            return basePanel;
        }

        /// <summary>
        /// Create the clean dialogue panel for JCSUnity; and add in under to 
        /// the canvas.
        /// 
        /// Need:
        ///     1) JCS_Canvas
        /// in the scene before create dialogue panel.
        /// </summary>
        private static GameObject CreateDialoguePanel()
        {
            var canvas = JCS_Util.FindObjectByType(typeof(JCS_Canvas)) as JCS_Canvas;
            if (canvas == null)
            {
                Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            const string setting_path = "UI/JCS_DialoguePanel";
            GameObject dialoguePanel = CreateHierarchyObjectUnderCanvas(setting_path);

            Undo.RegisterCreatedObjectUndo(dialoguePanel, "Create Dialogue Panel");

            dialoguePanel.transform.localScale = Vector3.one;
            dialoguePanel.name = "_DialoguePanel (Created)";

            return dialoguePanel;
        }

        /// <summary>
        /// Create 9x9 slide panel.
        /// 
        /// Need:
        ///     1) JCS_Camera
        ///     2) JCS_Canvas
        /// in the scene before create 9 x 9 slide panel.
        /// </summary>
        private static void CreateSlidePanel()
        {
            var canvas = JCS_Util.FindObjectByType(typeof(JCS_Canvas)) as JCS_Canvas;

            if (canvas == null)
            {
                Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return;
            }

            const string settingPath = "UI/System/JCS_SlideScreenPanelHolder";

            // spawn the pane holder.
            JCS_SlideScreenPanelHolder panelHolder9x9 = CreateHierarchyObjectUnderCanvas(settingPath, canvas).GetComponent<JCS_SlideScreenPanelHolder>();

            // create the array of panel.
            panelHolder9x9.slidePanels = new RectTransform[9];

            int starting_pos_x = -1920;
            int starting_pos_y = 1080;

            const string slidePanelPath = "UI/System/JCS_SlidePanel";

            int index = 0;

            // create all nine panel and assign to the slide panel.
            for (int row = 0; row < 3; ++row)
            {
                for (int column = 0; column < 3; ++column)
                {
                    // get the rect transform from the slide panel object.
                    var slidePanel = CreateHierarchyObjectUnderCanvas(slidePanelPath, canvas).GetComponent<RectTransform>();

                    // set the position into 9x9.
                    Vector3 slidePanelNewPos = slidePanel.localPosition;
                    slidePanelNewPos.x = starting_pos_x - (starting_pos_x * column);
                    slidePanelNewPos.y = starting_pos_y - (starting_pos_y * row);
                    slidePanel.localPosition = slidePanelNewPos;

                    // set scale to one.
                    slidePanel.localScale = Vector3.one;

                    var panelImage = slidePanel.GetComponent<Image>();

                    if (panelImage != null)
                    {
                        panelImage.color = JCS_Random.Color();
                    }

                    // assign to slide panel holder.
                    panelHolder9x9.slidePanels[index] = slidePanel;

                    slidePanel.name = "_SlidePanel_" + index + " (Created) ";

                    ++index;
                }
            }

            const string slideScreenCameraPath = "Camera/JCS_2DSlideScreenCamera";
            var slideScreenCamera = CreateHierarchyObject(slideScreenCameraPath).GetComponent<JCS_2DSlideScreenCamera>();

            Undo.RegisterCreatedObjectUndo(slideScreenCamera, "Create 2D Slide Screen Camera");

            slideScreenCamera.name = "_SlideScreenCamera (Created)";

            // set the panel holder.
            slideScreenCamera.PanelHolder = panelHolder9x9;

            // set to default 2d.
            slideScreenCamera.UnityGUIType = JCS_UnityGUIType.uGUI_2D;
        }

        /// <summary>
        /// Create a tween panel.
        /// </summary>
        private static GameObject CreateTweenPanel()
        {
            var canvas = JCS_Util.FindObjectByType(typeof(JCS_Canvas)) as JCS_Canvas;

            if (canvas == null)
            {
                Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            const string setting_path = "UI/JCS_TweenPanel";
            GameObject tweenPanel = CreateHierarchyObjectUnderCanvas(setting_path);

            Undo.RegisterCreatedObjectUndo(tweenPanel, "Create Tween Panel");

            tweenPanel.transform.localScale = Vector3.one;
            tweenPanel.name = "_TweenPanel (Created)";

            return tweenPanel;
        }

        /// <summary>
        /// Create the undo redo system object.
        /// </summary>
        /// <returns></returns>
        private static GameObject CreateUndoRedoSystem()
        {
            const string setting_path = "UI/JCS_UndoRedoSystem";
            GameObject undoRedoSystem = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(undoRedoSystem, "Create Undo Redo System");

            undoRedoSystem.name = "_UndoRedoSystem (Created)";

            return undoRedoSystem;
        }

        /**
         * 2D
         */

        /// <summary>
        /// Create camera for 2d game combine 
        /// with JCSUnity.
        /// </summary>
        private static void Create2DCamera()
        {
            const string camera_path = "Camera/JCS_2DCamera";
            GameObject gameObj = CreateHierarchyObject(camera_path);

            // set camera depth to default -10.
            gameObj.transform.position = new Vector3(0, 0, -10);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS 2D Camera");
        }

        /// <summary>
        /// Create a mix damage text pool.
        /// </summary>
        private static void CreateMixDamageTextPool()
        {
            const string setting_path = "UI/DamageText/JCS_MixDamageTextPool";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create Min Damage Text Pool");
        }

        /**
         * 3D
         */

        /// <summary>
        /// Create camera for 3d game combine 
        /// with JCSUnity.
        /// </summary>
        private static void Create3DCamera()
        {
            const string camera_path = "Camera/JCS_3DCamera";
            GameObject gameObj = CreateHierarchyObject(camera_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS 3D Camera");
        }

        /**
         * Utils
         */

        /// <summary>
        /// Create the Game Object during editing time.
        /// </summary>
        /// <returns></returns>
        private static GameObject CreateHierarchyObject(string settingPath)
        {
            // spawn the game object.
            GameObject hierarchyObj = JCS_Util.Instantiate(settingPath);

            // take away clone sign.
            JCS_Util.RemoveCloneString(hierarchyObj);

            return hierarchyObj;
        }

        /// <summary>
        /// Create the Game Object during the editing time and 
        /// put under JCS_Canvas object.
        /// </summary>
        /// <param name="settingPath"> path to spawn </param>
        /// <returns> object just spawned. </returns>
        private static GameObject CreateHierarchyObjectUnderCanvas(string settingPath)
        {
            // since this will be in the editing time.
            // so we don't worry to much about the performance.
            var canvas = JCS_Util.FindObjectByType(typeof(JCS_Canvas)) as JCS_Canvas;

            return CreateHierarchyObjectUnderCanvas(settingPath, canvas);
        }

        /// <summary>
        /// Create the Game Object during the editing time and 
        /// put under JCS_Canvas object.
        /// 
        /// Save O(n) time complexity during editing time.
        /// </summary>
        /// <param name="settingPath"> path to spawn </param>
        /// <param name="canvas"> canvas to set on. </param>
        /// <returns> object just spawned. </returns>
        private static GameObject CreateHierarchyObjectUnderCanvas(string settingPath, JCS_Canvas canvas)
        {
            if (canvas == null)
            {
                Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            // spawn the object first.
            GameObject hierarchyObj = CreateHierarchyObject(settingPath);

            // set the canvas as parent.
            hierarchyObj.transform.SetParent(canvas.transform);

            // init position.
            hierarchyObj.transform.localPosition = Vector3.zero;

            return hierarchyObj;
        }

        public string GetProjectName()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            return projectName;
        }
    }
}
