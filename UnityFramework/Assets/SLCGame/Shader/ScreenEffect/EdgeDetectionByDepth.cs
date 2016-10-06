using UnityEngine;
using System.Collections;

namespace SLCGame
{
    public class EdgeDetectionByDepth : PostEffectsBase
    {

        public Shader edgeDetectShader;
        private Material edgeDetectMaterial = null;
        public Material material
        {
            get {
                edgeDetectMaterial = CheckShaderAndCreateMaterial(edgeDetectShader, edgeDetectMaterial);
                return edgeDetectMaterial;
            }
        }

        [Range(0.0f, 1.0f)]
        public float edgesOnly = 0.0f;
        public Color edgeColor = Color.black;
        public Color backgroundColor = Color.white;
        public float sampleDistance = 1.0f;
        public float sensitivityDepth = 1.0f;
        public float sensitivityNormals = 1.0f;


        void OnEnable()
        {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
        }
           
        [ImageEffectOpaque]//只对非透明物体起作用 渲染队列小于 2500的起作用
        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (material != null)
            {
                material.SetFloat("_EdgeOnly", edgesOnly);
                material.SetColor("_EdgeColor", edgeColor);
                material.SetColor("_BackgroundColor", backgroundColor);
                material.SetFloat("_SampleDistance", sampleDistance);
                material.SetVector("_Sensitivity", new Vector4(sensitivityNormals,sensitivityDepth,0.0f,0.0f)); 

                Graphics.Blit(src, dest, material);
            }
            else
            {
                Graphics.Blit(src, dest);
            }
        }
    }
}
