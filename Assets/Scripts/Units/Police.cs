using UnityEngine;

public class Police : MonoBehaviour, IUnit
{
	public GameObject PoliceModel;
	
	public GameObject GetModel()
	{
		return PoliceModel;
	}
}
