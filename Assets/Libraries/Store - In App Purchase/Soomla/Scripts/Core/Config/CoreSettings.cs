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
using System;
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
	public class CoreSettings : ISoomlaSettings
	{

		private static string CoreModulePrefix = "Core";

#if UNITY_EDITOR
		public static string DB_KEY_PREFIX = "soomla.";

		static CoreSettings instance = new CoreSettings();

		static string currentModuleVersion = "1.3.2";

		static CoreSettings()
		{
			SoomlaEditorScript.addSettings(instance);

#if UNITY_4
			foreach (string fPath in System.IO.Directory.GetFiles("Assets/Plugins/WP8/Soomla/Placeholder")) {
				if(!fPath.Contains(".meta")){
					FileUtil.MoveFileOrDirectory(fPath, fPath.Replace("WP8/Soomla/Placeholder",""));
				}
				AssetDatabase.Refresh();
			}
#endif

			SoomlaEditorScript.addFileList("Core", "Assets/Libraries/Store - In App Purchase/Soomla/core_file_list", new string[]{});
		}

//		GUIContent emptyContent = new GUIContent("");

		GUIContent frameworkVersion = new GUIContent("Core Version [?]", "The SOOMLA Framework version. ");
		
		public void OnEnable() {
			// Generating AndroidManifest.xml
			SoomlaManifestTools.GenerateManifest();
		}

		public void OnModuleGUI() {

		}

		public void OnAndroidGUI() {

		}

		public void OnIOSGUI(){

		}

		public void OnWP8GUI(){

		}

		public void OnInfoGUI() {
			EditorGUILayout.HelpBox("SOOMLA Framework Info", MessageType.None);
			SoomlaEditorScript.RemoveSoomlaModuleButton(frameworkVersion, currentModuleVersion, "Core");
			SoomlaEditorScript.LatestVersionField ("unity3d-core", currentModuleVersion, "New version available!", "http://library.soom.la/fetch/unity3d-core/latest?cf=unity");
			EditorGUILayout.Space();
		}

		GUIContent soomlaSecLabel = new GUIContent("Soomla Secret [?]:", "All the user information will be encrypted using this secret.");
		GUIContent debugMsgsLabel = new GUIContent("Debug Native [?]:", "Check if you want to show debug messages from native code in the log (iOS and Android).");
		GUIContent debugUnityMsgsLabel = new GUIContent("Debug Unity [?]:", "Check if you want to show debug messages from Unity code in the log (Editor, iOS and Android).");

		public void OnSoomlaGUI() {
			FileStream fs = new FileStream(Application.dataPath + @"/Soomla/Resources/soom_logo.png", FileMode.Open, FileAccess.Read);
			byte[] imageData = new byte[fs.Length];
			fs.Read(imageData, 0, (int)fs.Length);
			Texture2D logoTexture = new Texture2D(300, 92);
			logoTexture.LoadImage(imageData);

			EditorGUILayout.BeginHorizontal();
			GUIContent logoImgLabel = new GUIContent (logoTexture);
			EditorGUILayout.LabelField(logoImgLabel, GUILayout.MaxHeight(70), GUILayout.ExpandWidth(true));
			EditorGUILayout.EndHorizontal();

			GameObject.DestroyImmediate(logoTexture);

			EditorGUILayout.HelpBox("Make sure you fill out all the information below", MessageType.None);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(soomlaSecLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
			SoomlaSecret = EditorGUILayout.TextField(SoomlaSecret, SoomlaEditorScript.FieldHeight);
			EditorGUILayout.EndHorizontal();

			DebugMessages = EditorGUILayout.Toggle(debugMsgsLabel, DebugMessages);
			DebugUnityMessages = EditorGUILayout.Toggle(debugUnityMsgsLabel, DebugUnityMessages);

			EditorGUILayout.Space();


			if (!SoomlaAndroidUtil.IsSetupProperly())
			{
				var msg = "You have errors in your Android setup. More info in the SOOMLA docs.";
				switch (SoomlaAndroidUtil.SetupError)
				{
				case SoomlaAndroidUtil.ERROR_NO_SDK:
					msg = "You need to install the Android SDK!  Set the location of Android SDK in: " + (Application.platform == RuntimePlatform.OSXEditor ? "Unity" : "Edit") + "->Preferences->External Tools";
					break;
				case SoomlaAndroidUtil.ERROR_NO_KEYSTORE:
					msg = "Your defined keystore doesn't exist! You'll need to create a debug keystore or point to your keystore in 'Publishing Settings' from 'File -> Build Settings -> Player Settings...'";
					break;
				}

				EditorGUILayout.HelpBox(msg, MessageType.Error);
			}
		}
#endif

		public static string ONLY_ONCE_DEFAULT = "SET ONLY ONCE";

		private static string soomlaSecret;
		public static string SoomlaSecret
		{
			get {
				if (soomlaSecret == null) {
					soomlaSecret = SoomlaEditorScript.GetConfigValue (CoreModulePrefix, "SoomlaSecret");
					if (soomlaSecret == null) {
						soomlaSecret = ONLY_ONCE_DEFAULT;
					}
				}
				return soomlaSecret;
			}
			set
			{
				if (soomlaSecret != value)
				{
					soomlaSecret = value;
					SoomlaEditorScript.SetConfigValue(CoreModulePrefix, "SoomlaSecret", value.ToString());
					SoomlaEditorScript.DirtyEditor();
				}
			}
		}
		private static string debugMessages;
		public static bool DebugMessages
		{
			get {
				if (debugMessages == null) {
					debugMessages = SoomlaEditorScript.GetConfigValue (CoreModulePrefix, "DebugMessages");
					if (debugMessages == null) {
						debugMessages = false.ToString ();
					}
				}
				return Convert.ToBoolean(debugMessages);
			}
			set
			{
				if (Convert.ToBoolean(debugMessages) != value )
				{
					debugMessages = value.ToString();
					SoomlaEditorScript.SetConfigValue(CoreModulePrefix, "DebugMessages", value.ToString());
					SoomlaEditorScript.DirtyEditor();
				}
			}
		}

		private static string debugUnityMessages;
		public static bool DebugUnityMessages
		{
			get {
				if (debugUnityMessages == null) {
					debugUnityMessages = SoomlaEditorScript.GetConfigValue (CoreModulePrefix, "DebugUnityMessages");
					if (debugUnityMessages == null) {
						debugUnityMessages = true.ToString();
					}
				}
				return Convert.ToBoolean(debugUnityMessages);
			}
			set
			{
				if (Convert.ToBoolean(debugUnityMessages) != value )
				{
					debugUnityMessages = value.ToString();
					SoomlaEditorScript.SetConfigValue(CoreModulePrefix, "DebugUnityMessages", value.ToString());
					SoomlaEditorScript.DirtyEditor();
				}
			}
		}

	}
}
