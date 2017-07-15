using UnityEngine;

public abstract class Unit : MonoBehaviour
{
	public GameObject Prefab;
	public Color OriginalColor;
	public Color OriginalEmissionColor;

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
		_renderer.material.SetColor("_EmissionColor", materialColor);
	}

	public void ResetMaterial()
	{
		_renderer.material.color = OriginalColor;
		_renderer.material.SetColor("_EmissionColor", OriginalEmissionColor);
	}
}
