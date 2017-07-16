using UnityEngine;

public class BillBoarding : MonoBehaviour 
{
	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.LookAt(Camera.main.transform);
	}
}
