using UnityEngine;

public class BillBoarding : MonoBehaviour
{
	public float Offset = 0;
	private Vector3 _origin;

	// Update is called once per frame
	private void Start()
	{
		_origin = gameObject.transform.position;
	}

	void Update ()
	{
		if (Offset == 0)
		{
			gameObject.transform.LookAt(Camera.main.transform);
		}
		else
		{
			gameObject.transform.position = Vector3.MoveTowards(_origin, Camera.main.transform.position, Offset);
			gameObject.transform.LookAt(Camera.main.transform);
		}
	}
}
