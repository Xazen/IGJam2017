using UnityEngine;

public abstract class Unit : MonoBehaviour
{
	public GameObject Prefab;
	public Color OriginalColor;

	private MeshRenderer _renderer;
	
	public abstract void OnUnitSpawned();

	public void Start()
	{
		_renderer = GetComponentInChildren<MeshRenderer>();
	}
	
	public GameObject GetModel()
	{
		return Prefab;
	}

	public void SetMaterialColor(Color materialColor)
	{
		_renderer.material.color = materialColor;
	}

	public void ResetMaterial()
	{
		_renderer.material.color = OriginalColor;
	}
}
