using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticle : UIRenderSort
{ 
    ParticleSystemRenderer m_Render;
    Material m_Material;  
    // Use this for initialization
    void Start()
    {
        m_Render = GetComponent<ParticleSystemRenderer>(); 
    }

    public override void FillDrawCall()
    {
        Shader shad;
        if (panel.hasClipping)
            shad = Shader.Find("Hidden/Unlit/Transparent Colored 1");
        else
            shad = Shader.Find("Unlit/Transparent Colored");
        m_Material = new Material(shad)
        {
            renderQueue = renderQuene,
            mainTexture = m_Render.material.mainTexture
        };
        m_Render.material = m_Material;
    }

    // Update is called once per frame
    void OnWillRenderObject()
    {
        if (panel == null) return;
        //裁剪
        if (panel.hasClipping)
        {
            Vector4 cr = panel.drawCallClipRange;
            Vector2 soft = panel.clipSoftness;
            Vector2 sharpness = new Vector2(1000.0f, 1000.0f);
            if (soft.x > 0f) sharpness.x = cr.z / soft.x;
            if (soft.y > 0f) sharpness.y = cr.w / soft.y;

            float scale = 1.0f / transform.lossyScale.x;
            Vector2 position = -panel.transform.position * scale;
             
            m_Material.SetVector(Shader.PropertyToID("_ClipRange0"), new Vector4(-cr.x / cr.z + position.x / cr.z,
                -cr.y / cr.w + position.y / cr.w,
               1f/cr.z *scale ,
               1f/cr.w * scale));
            m_Material.SetVector(Shader.PropertyToID("_ClipArgs0"), new Vector4(sharpness.x, sharpness.y, 0, 1));
        }
    }

    void Destory()
    {
        DestroyImmediate(m_Material);
        m_Material = null;
    }
}
