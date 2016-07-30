using UnityEngine;
using System.Collections;

public class FllowTarget : MonoBehaviour
{
	public Transform character;
	public float smootTime = 0.01f;
	private Vector3 cameraVelocity = Vector3.zero;
	private Camera mainCamera;
	private Vector3 Gap;
	// Use this for initialization
	void Start()
	{
		mainCamera = Camera.main;
		if (character) {
			Gap = mainCamera.transform.position - character.position;
		}

	}
	
	// Update is called once per frame
	void Update()
	{
		if (character) {
			transform.position = Vector3.SmoothDamp(transform.position, character.position + Gap, ref cameraVelocity, smootTime);
		}
	}
}

