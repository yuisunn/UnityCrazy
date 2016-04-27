using UnityEngine;
using System.Collections;
using SPSGame.Tools;
public class LoadShaderAgain : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{		
		Renderer[] renderers = this.gameObject.GetComponents<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{		
			for (int j = 0; j < renderers[i].sharedMaterials.Length; j++)
			{
				string name = renderers[i].sharedMaterials[j].shader.name;
				renderers[i].sharedMaterials[j].shader = Shader.Find(name);
                if (renderers[i].sharedMaterials[j].shader ==null)
                DebugMod.LogWarning(renderers[i].sharedMaterials[j].name+" shader not found!");
			}
		}    
	}
}