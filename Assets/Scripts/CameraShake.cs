using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	private Vector3 originalPos;

	private void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent<Transform>();
		}
	}

	public void Shake()
    {
		shakeDuration = 0.1f;
    }

	private void Update()
	{
		if (shakeDuration > 0)
		{
			camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
		}
	}
}