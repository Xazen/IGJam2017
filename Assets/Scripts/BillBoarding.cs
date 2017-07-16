using UnityEngine;

public class BillBoarding : MonoBehaviour
{
	public float Offset = 0;
	private Vector3 _origin;
	private Vector3 _localOriginPosition;

	// Update is called once per frame
	private void Start()
	{
		_localOriginPosition = gameObject.transform.localPosition;
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
			gameObject.transform.LookAt(Camera.main.transform);
			gameObject.transform.localPosition = _localOriginPosition;
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Camera.main.transform.position, Offset);
		}
	}
}
