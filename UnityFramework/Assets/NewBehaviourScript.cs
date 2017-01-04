using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    /// <summary>
    /// 设置etc 压缩用 etc1 加 alpha 混合
    /// </summary>
    /// <param name="ai"></param>
    /// <param name="tag"></param>
    private static void SetTextureSprite(AssetImporter ai, string tag = null)
    {
        TextureImporter importer = ai as TextureImporter;
        if (importer == null) return;
        importer.textureType = TextureImporterType.Sprite;
        importer.mipmapEnabled = false;
        importer.isReadable = false;
        if (tag != null)
        {
            //tag = tag.Replace(GameConfig.AB_EXT, "");
            tag = tag.ToLower();
            importer.spritePackingTag = tag;
        }

#if UNITY_ANDROID
        int maxSize = 1024;  
        TextureImporterFormat format = TextureImporterFormat.AutomaticCompressed;  
        int quality = 50;  
        importer.GetPlatformTextureSettings("Android", out maxSize, out format, out quality);  
  
        // 压缩的格式，android下修改为分离alpha通道的etc1  
        if (format == TextureImporterFormat.AutomaticCompressed)  
        {  
            importer.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.ETC_RGB4, quality, true);  
            importer.SetAllowsAlphaSplitting(true);  
        }  
#else
        // iPhone  
#endif
    }
}
