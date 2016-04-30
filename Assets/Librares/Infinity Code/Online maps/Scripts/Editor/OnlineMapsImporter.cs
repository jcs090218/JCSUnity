/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

public class OnlineMapsImporter : AssetPostprocessor
{  
	void OnPreprocessTexture()
	{
		if (assetPath.Contains("Resources/OnlineMapsTiles")) 
        {
			TextureImporter textureImporter = assetImporter as TextureImporter;
			textureImporter.mipmapEnabled = false;
			textureImporter.isReadable = true;
			textureImporter.textureFormat = TextureImporterFormat.RGB24;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
			textureImporter.maxTextureSize = 256;
		}
	}
}