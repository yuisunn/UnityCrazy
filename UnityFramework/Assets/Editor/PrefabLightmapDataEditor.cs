using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PrefabLightmapDataEditor : Editor
{
    // 把renderer上面的lightmap信息保存起来，以便存储到prefab上面
    [MenuItem("GameObject/Lightmap/Save", false, 0)]
    static void SaveLightmapInfo()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;
        PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
        if (data == null)
        {
            data = go.AddComponent<PrefabLightmapData>();
        }

        data.SaveLightmap();
        EditorUtility.SetDirty(go);
    }

    // 把保存的lightmap信息恢复到renderer上面
    [MenuItem("GameObject/Lightmap/Load", false, 0)]
    static void LoadLightmapInfo()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
        if (data == null) return;

        data.LoadLightmap();
        EditorUtility.SetDirty(go);

        new GameObject();
    }

    [MenuItem("GameObject/Lightmap/Clear", false, 0)]
    static void ClearLightmapInfo()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
        if (data == null) return;

        data.m_RendererInfo.Clear();
        EditorUtility.SetDirty(go);
    }
}