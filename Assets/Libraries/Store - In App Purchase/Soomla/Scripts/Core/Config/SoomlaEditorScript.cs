/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Soomla
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	/// <summary>
	/// This class holds the store's configurations.
	/// </summary>
	public class SoomlaEditorScript : ScriptableObject
	{
#if UNITY_EDITOR
		private static BuildTargetGroup[] supportedPlatforms =
		{
			BuildTargetGroup.Android,
#if UNITY_5
			BuildTargetGroup.iOS,
#else
			BuildTargetGroup.iPhone,
#endif
			BuildTargetGroup.WebPlayer,
			BuildTargetGroup.Standalone
		};
#endif
		public static string AND_PUB_KEY_DEFAULT = "YOUR GOOGLE PLAY PUBLIC KEY";
		public static string ONLY_ONCE_DEFAULT = "SET ONLY ONCE";

		const string soomSettingsAssetName = "SoomlaEditorScript";
		const string soomSettingsPath = "Soomla/Resources";
		const string soomSettingsAssetExtension = ".asset";

		private static SoomlaEditorScript instance;
#if UNITY_EDITOR
		private static void ToggleOpenSourceFlag(bool remove) {
			if (remove) {
				return;
			}
			foreach (BuildTargetGroup target in supportedPlatforms) {
				string targetFlag = "SOOMLA_OPEN_SOURCE";
				string scriptDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
				List<string> flags = new List<string>(scriptDefines.Split(';'));

				if (flags.Contains(targetFlag)) {
					if (remove) {
						flags.Remove(targetFlag);
					}
				}
				else {
					if (!remove) {
						flags.Add(targetFlag);
					}
				}

				string result = string.Join(";", flags.ToArray());
				if (scriptDefines != result) {
					PlayerSettings.SetScriptingDefineSymbolsForGroup(target, result);
				}
			}
		}
#endif

		public static SoomlaEditorScript Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load(soomSettingsAssetName) as SoomlaEditorScript;

					if (instance == null)
					{
						// If not found, autocreate the asset object.
						instance = CreateInstance<SoomlaEditorScript>();
#if UNITY_EDITOR
						string properPath = Path.Combine(Application.dataPath, soomSettingsPath);
						if (!Directory.Exists(properPath))
						{
							AssetDatabase.CreateFolder("Assets/Soomla", "Resources");
						}

						string fullPath = Path.Combine(Path.Combine("Assets", soomSettingsPath),
										soomSettingsAssetName + soomSettingsAssetExtension);
						AssetDatabase.CreateAsset(instance, fullPath);
#endif
					}
				}
				return instance;
			}
		}

#if UNITY_EDITOR

		private static List<ISoomlaSettings> mSoomlaSettings = new List<ISoomlaSettings>();
		private static Dictionary<string, string[]>mFileList = new Dictionary<string, string[]>();

		public static void addSettings(ISoomlaSettings spp) {
			mSoomlaSettings.Add(spp);
			ToggleOpenSourceFlag(mSoomlaSettings.Count <= 1);
		}

		public static void addFileList(string moduleId, string fileListPath, string[] additionalFiles) {
			List<string> foldersFiles = new List<string>();
			foldersFiles.Add(fileListPath);
			foldersFiles.AddRange (additionalFiles);
			string line;
			StreamReader reader = new StreamReader(fileListPath);
			do {
				line = reader.ReadLine();
				if (line != null) {
					foldersFiles.Add(line);
#if UNITY_4
					string placeHolder = "WP8/Soomla/Placeholder";
					if(line.Contains(placeHolder)){
						line = line.Replace(placeHolder, "");
					}
					foldersFiles.Add(line);
#endif
				}
			} while (line != null);
			reader.Close();
			mFileList.Add(moduleId, foldersFiles.ToArray());
		}

		public static void OnEnable() {
			foreach(ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnEnable();
			}
		}

		#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7
		private static bool showIOSSettings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone);
		#else
		private static bool showIOSSettings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS);
		#endif
		private static bool showWP8Settings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WP8Player);
		private static bool showAndroidSettings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android);

		public static void OnInspectorGUI() {
			foreach(ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnSoomlaGUI();
				EditorGUILayout.Space();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			foreach (ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnModuleGUI ();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			showAndroidSettings = EditorGUILayout.Foldout(showAndroidSettings, "Android Settings");
			foreach (ISoomlaSettings settings in mSoomlaSettings) {
				if (showAndroidSettings)
					settings.OnAndroidGUI ();
			}
			showIOSSettings = EditorGUILayout.Foldout(showIOSSettings, "iOS Settings");
			foreach (ISoomlaSettings settings in mSoomlaSettings) {
				if (showIOSSettings)
					settings.OnIOSGUI ();
			}
            showWP8Settings = EditorGUILayout.Foldout(showWP8Settings, "WP8 Settings");
			foreach (ISoomlaSettings settings in mSoomlaSettings) {
				if (showWP8Settings)
					settings.OnWP8GUI ();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			foreach(ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnInfoGUI();
				EditorGUILayout.Space();
			}
		}
#endif
#if UNITY_EDITOR && SOOMLA_OPEN_SOURCE
		[MenuItem("Window/Soomla/Edit Settings")]
		public static void Edit()
		{
			Selection.activeObject = Instance;
		}

		[MenuItem("Window/Soomla/Remove Soomla")]
		public static void Remove()
		{
			string fullPath = Path.Combine(Path.Combine("Assets", soomSettingsPath),
			                               soomSettingsAssetName + soomSettingsAssetExtension);
			AssetDatabase.DeleteAsset (fullPath);
			SoomlaManifestTools.ClearManifest( );
			if (EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to remove SOOMLA?", "Yes", "No")) {
				foreach (KeyValuePair<string, string[]> attachStat in mFileList) {
					RemoveModule(attachStat.Value);
				}
			}
		}

		[MenuItem("Window/Soomla/Framework Page")]
		public static void OpenFramework()
		{
			string url = "https://www.github.com/soomla/unity3d-store";
			Application.OpenURL(url);
		}

		[MenuItem("Window/Soomla/Report an issue")]
		public static void OpenIssue()
		{
			string url = "https://answers.soom.la";
			Application.OpenURL(url);
		}

#endif

		public static void DirtyEditor()
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(Instance);
#endif
		}

		[SerializeField]
		public ObjectDictionary SoomlaSettings = new ObjectDictionary();


	/** SOOMLA Core UI **/
#if UNITY_EDITOR
		public static GUILayoutOption FieldHeight = GUILayout.Height(16);
		public static GUILayoutOption FieldWidth = GUILayout.Width(120);
		public static GUILayoutOption SpaceWidth = GUILayout.Width(24);
		public static GUIContent EmptyContent = new GUIContent("");

		public static JSONObject versionJson;
		public static WWW www;

		public static void RemoveModule(string[] filePaths)
		{
			List<string> folders = new List<string>();
			foreach (string file in filePaths) {
				if (file.EndsWith ("SoomlaShared")) {
					continue;
				}
				FileUtil.DeleteFileOrDirectory(file);
				string folderPath = Path.GetDirectoryName(file);
				do {
					if (!folders.Contains(folderPath)) {
						folders.Add(folderPath);
					}
					folderPath = Path.GetDirectoryName(folderPath);
				} while (folderPath != "");
			}
			folders.Sort((a, b) => b.Length.CompareTo(a.Length));
			foreach (string fPath in folders) {
				AssetDatabase.Refresh();
				if (Directory.Exists(fPath)) {
					if (System.IO.Directory.GetFiles(fPath).Length == 0) {
						FileUtil.DeleteFileOrDirectory(fPath);
					}
				}
			}
			AssetDatabase.Refresh();
		}

        public static void LatestVersionField(string moduleId, string currentVersion, string versionPrompt, string downloadLink)
		{
			if (www == null || (www.error != null && www.error.Length > 0)) {
				www = new WWW("http://library.soom.la/fetch/info");
			}
			string latestVersion = null;
			if (versionJson == null) {
				if (www.isDone) {
					versionJson = new JSONObject(www.text);
				}
				DirtyEditor();
			}
			else {
				latestVersion = versionJson.GetField (moduleId).GetField ("latest").str;
			}

			EditorGUILayout.BeginHorizontal();
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.blue;
			if (GUILayout.Button ((latestVersion != null && currentVersion != latestVersion) ? versionPrompt : "", style, GUILayout.Width (170), FieldHeight)) {
				if (latestVersion != null && currentVersion != latestVersion) {
					Application.OpenURL(downloadLink);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		public static void RemoveSoomlaModuleButton(GUIContent label, string value, string moduleId)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label, GUILayout.Width(140), FieldHeight);
			EditorGUILayout.SelectableLabel(value, GUILayout.Width(40), FieldHeight);

			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.blue;
			if (GUILayout.Button("Remove", style, GUILayout.Width(60), FieldHeight)) {
				if (EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to delete " + moduleId + " ?", "Yes", "No")) {
					SoomlaManifestTools.ClearManifest(moduleId);
					RemoveModule(mFileList[moduleId]);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		public static void SelectableLabelField(GUIContent label, string value)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label, GUILayout.Width(140), FieldHeight);
			EditorGUILayout.SelectableLabel(value, FieldHeight);
			EditorGUILayout.EndHorizontal();
		}
#endif
		public static void SetConfigValue(string prefix, string key, string value) {
			PlayerPrefs.SetString("Soomla." + prefix + "." + key, value);
			Instance.SoomlaSettings["Soomla." + prefix + "." + key] = value;
			PlayerPrefs.Save();
		}

		public static string GetConfigValue(string prefix, string key) {
			string value;
			if (Instance.SoomlaSettings.TryGetValue("Soomla." + prefix + "." + key, out value) && value.Length > 0) {
				return value;
			} else {
				value = PlayerPrefs.GetString("Soomla." + prefix + "." + key);
				SetConfigValue(prefix, key, value);
				return value.Length > 0 ? value : null;
			}
		}
	}
}
