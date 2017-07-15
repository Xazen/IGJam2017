using UnityEngine;

public abstract class Unit : MonoBehaviour
{
	public GameObject Prefab;
	public Material OriginalMaterial;

	private MeshRenderer _renderer;
	
	public abstract void OnUnitSpawned();

	public GameObject GetModel()
	{
		return Prefab;
	}

	public void SetMaterial(Material material)
	{
		_renderer = GetComponentInChildren<MeshRenderer>();
		_renderer.material = material;
		_renderer.materials = new []{material};
		_renderer.sharedMaterial = material;
		_renderer.sharedMaterials = new []{material};
	}

	public void ResetMaterial()
	{
		_renderer.sharedMaterials[0] = OriginalMaterial;
	}
}
