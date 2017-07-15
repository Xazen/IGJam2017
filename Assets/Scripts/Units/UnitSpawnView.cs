using UnityEngine;
using Zenject;

public class UnitSpawnView : MonoBehaviour
{
	public Camera MainCamera;

	private Vector3 _targetUnitSpawnPreviewPosition;
	private GameObject _unitSpawnPreview;
	
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

		if (IsMouseOverMap(out _targetUnitSpawnPreviewPosition))
		{
			_targetUnitSpawnPreviewPosition = new Vector3(
				Mathf.RoundToInt(_targetUnitSpawnPreviewPosition.x),
				0,
				Mathf.RoundToInt(_targetUnitSpawnPreviewPosition.z));
			UpdateSpawnPreview();
		}
	}

	private bool IsMouseOverMap(out Vector3 mousePosition)
	{
		Ray	ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			mousePosition = hit.point;
			return true;
		}
		
		// Just move the preview somewhere outside the visible area
		mousePosition = new Vector3(-1000, -1000, -1000);
		return false;
	}

	private void UpdateSpawnPreview()
	{
		// 
		if (!_unitSpawnPreview)
		{
			_unitSpawnPreview = Instantiate(_unitSpawnController.GetSelectedUnitPrefab(), _targetUnitSpawnPreviewPosition,
				Quaternion.identity);
		}
		else if (_unitSpawnPreview.transform.position != _targetUnitSpawnPreviewPosition)
		{
			_unitSpawnPreview.transform.position = _targetUnitSpawnPreviewPosition;
		}
	}
}
