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
	}

	public void ResetMaterial()
	{
		Renderer.material = OriginalMaterial;
	}
}
