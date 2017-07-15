using UnityEngine;
using Zenject;

public class UnitSpawnView : MonoBehaviour
{
	public Camera MainCamera;

	private Vector3 _mouseOverMapPosition;
	private GameObject _selectedUnit;
	
	private UnitSpawnController _unitSpawnController;

	[Inject]
	public void Inject(UnitSpawnController unitSpawnController)
	{
		_unitSpawnController = unitSpawnController;
	}

	public void Update()
	{
		// No unit selected, just return
		if (_unitSpawnController == null || !_unitSpawnController.IsUnitSelected())
		{
			return;
		}

		if (IsMouseOverMap(out _mouseOverMapPosition))
		{
			if (_selectedUnit == null)
			{
				_selectedUnit = _unitSpawnController.GetSelectedUnitPrefab();	
			}
		}
	}

	private bool IsMouseOverMap(out Vector3 mousePosition)
	{
		Ray	ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			bool isMouseOverMap = hit.collider.gameObject.CompareTag(TagConstants.Map);
			mousePosition = hit.point;
			return isMouseOverMap;
		}
		return false;
	}
}
