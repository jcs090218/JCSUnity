using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.IO;

namespace Soomla
{

	public class SoomlaAndroidUtil
    {
#if UNITY_EDITOR
		private static char DSC = System.IO.Path.DirectorySeparatorChar;

        public const string ERROR_NO_SDK = "no_android_sdk";
        public const string ERROR_NO_KEYSTORE = "no_android_keystore";

        private static string setupError;

        public static bool IsSetupProperly()
        {
			if (setupError == "none") {
				return true;
			}
			if (setupError == null) {
				if (!HasAndroidSDK())
				{
					setupError = ERROR_NO_SDK;
					return false;
				}
				if (!HasAndroidKeystoreFile())
				{
					setupError = ERROR_NO_KEYSTORE;
					return false;
				}

				setupError = "none";
				return true;
			} else {
				return false;
			}
        }

		private static string HomeFolderPath
		{
			get 
			{
				string homeFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				switch(System.Environment.OSVersion.Platform) {
				case System.PlatformID.WinCE:
				case System.PlatformID.Win32Windows:
				case System.PlatformID.Win32S:
				case System.PlatformID.Win32NT:
					homeFolder = System.IO.Directory.GetParent(homeFolder).FullName;
					break;
				default:
					break;
				}

				return homeFolder;
			}
		}

		public static string KeyStorePass
		{
			get
			{
				string keyStorePass = PlayerSettings.Android.keystorePass;
				if (string.IsNullOrEmpty(keyStorePass)) {
					keyStorePass = @"android";
				}
				return keyStorePass;
			}
		}

        public static string KeyStorePath
        {
            get
            {
				string keyStore = PlayerSettings.Android.keystoreName;
				if (string.IsNullOrEmpty(keyStore)) {
					keyStore = HomeFolderPath + DSC + @".android" + DSC + @"debug.keystore";
				}
				return keyStore;
			}
        }

        public static string SetupError
        {
            get
            {
                return setupError;
            }
        }

        public static bool HasAndroidSDK()
        {
            return EditorPrefs.HasKey("AndroidSdkRoot") && System.IO.Directory.Exists(EditorPrefs.GetString("AndroidSdkRoot"));
        }

        public static bool HasAndroidKeystoreFile()
        {
		    return System.IO.File.Exists(KeyStorePath);
        }



#endif
    }
}
