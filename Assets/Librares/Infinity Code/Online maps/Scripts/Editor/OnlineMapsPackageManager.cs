/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.IO;
using UnityEditor;
using UnityEngine;

public class OnlineMapsPackageManager
{
    [MenuItem("Component/Infinity Code/Online Maps/Packages/Playmaker Integration Kit")]
    public static void ImportPlayMakerIntegrationKit()
    {
        if (EditorUtility.DisplayDialog("Playmaker Integration Kit", "You have Playmaker in your project?", "Yes, I have a Playmaker", "Cancel"))
        {
            string[] files = Directory.GetFiles("Assets", "OnlineMaps-Playmaker-Integration-Kit.unitypackage", SearchOption.AllDirectories);
            if (files.Length == 0) Debug.LogError("Could not find Playmaker Integration Kit.");
            else AssetDatabase.ImportPackage(files[0], true);
        }
    }
}