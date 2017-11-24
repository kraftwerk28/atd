using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraNavigation : MonoBehaviour
{
	public Slider MouseSensitivitySlider;
	public Slider KeySensitivitySlider;

	private float Deltakoef = 0.005f;
	private float Delta = 1;
	private float MoveSpeed = 10;

	private Camera cam;
	private Vector3 Target;
	private Vector3 Rtl, Rtr, Rdl, Rdr;
	private RaycastHit rh;

	// Use this for initialization
	void Start()
	{
		cam = GetComponent<Camera>();

		Rdl = new Vector3(0, 0);
		Rtl = new Vector3(0, cam.pixelHeight);
		Rdr = new Vector3(cam.pixelWidth, 0);
		Rtr = new Vector3(cam.pixelWidth, cam.pixelHeight);

		Target = transform.position;
	}

	// Update is called once per frame
	void Update()
	{

		transform.position = Vector3.Lerp(transform.position, Target, Time.deltaTime * MoveSpeed);

		//KeyNav
		Target -= Delta * KeySensitivitySlider.value * (Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical"));

		//zoom
		if (cam.orthographic)
		{
			cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 100 * Delta;
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 60);
			Target.y = 100;
		}
		else
			Target += transform.forward * Input.GetAxis("Mouse ScrollWheel") * 100;

		//if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(cam.pixelWidth, cam.pixelHeight)), out rh))
		//{
		//	Delta = rh.distance * Deltakoef;
		//}
		Delta = transform.position.y * Deltakoef;

		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 5, Mathf.Infinity), transform.position.z);

		//MouseNav
		if (Input.GetMouseButton(0))
		{
			Target += MouseSensitivitySlider.value * Delta * ((Vector3.forward + Vector3.right) * Input.GetAxis("Mouse X") + (Vector3.forward - Vector3.right) * Input.GetAxis("Mouse Y"));
		}

		if (Input.GetKey(KeyCode.KeypadPlus))
		{
			if (cam.orthographic)
			{
				cam.orthographicSize -= 10 * Delta;
				cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 60);
				Target.y = 100;
			}
			else
				Target += transform.forward * 10 * Delta;
		}
		else if (Input.GetKey(KeyCode.KeypadMinus))
		{
			if (cam.orthographic)
			{
				cam.orthographicSize += 10 * Delta;
				cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 80);
				Target.y = 100;
			}
			else
				Target -= transform.forward * 10 * Delta;
		}
	}

	private void LateUpdate()
	{
		float d = Delta * 10;
		if (transform.position.y > 3)
		{
			if (!Physics.Raycast(cam.ScreenPointToRay(Rdl))
			&& !Physics.Raycast(cam.ScreenPointToRay(Rtr))
			&& !Physics.Raycast(cam.ScreenPointToRay(Rtl))
			&& !Physics.Raycast(cam.ScreenPointToRay(Rdr)))
				Target += d  * transform.forward;
			else
			{
				if (!Physics.Raycast(cam.ScreenPointToRay(Rdl)))
					Target -= Vector3.forward * d;
				if (!Physics.Raycast(cam.ScreenPointToRay(Rtr)))
					Target += Vector3.forward * d;
				if (!Physics.Raycast(cam.ScreenPointToRay(Rtl)))
					Target -= Vector3.right * d;
				if (!Physics.Raycast(cam.ScreenPointToRay(Rdr)))
					Target += Vector3.right * d;
			}
		}
		else
			Target -= transform.forward;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!cam.orthographic)
		{
			//Target -= transform.forward * 50 * Delta;
			transform.position -= transform.forward * 50 * Delta;
			Target = transform.position;
		}
	}
}
