using UnityEngine;

public abstract class Unit : MonoBehaviour
{
	public GameObject Prefab;
	public Renderer Renderer;
	public Material OriginalMaterial;

	public abstract void OnUnitSpawned();
	
	public GameObject GetModel()
	{
		return Prefab;
	}

	public void SetMaterial(Material material)
	{
		Renderer.material = material;
		Renderer.materials = new []{material};
		Renderer.sharedMaterial = material;
		Renderer.sharedMaterials = new []{material};
	}

	public void ResetMaterial()
	{
		Renderer.sharedMaterials[0] = OriginalMaterial;
	}
}
