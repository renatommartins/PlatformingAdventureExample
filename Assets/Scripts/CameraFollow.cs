using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject target;
	public float smoothingFactor = 0.1f;
	public float movementOffset = 2.0f;
	public float movementThreshold = 0.5f;
	public float graceTime = 0.5f;

	[Space(5)]
	public float xMinimum;
	public float xMaximum;
	public float yMinimum;
	public float yMaximum;

	private Vector3 _originalPositon;
	private Vector3 _targetLastPosition;
	private bool _isTriggered = false;
	private float _triggerTime;

	private void Start()
	{
		_originalPositon = transform.position;
	}

	private void FixedUpdate()
	{
		Vector3 cameraTargetPosition;
		Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, _originalPositon.z);

		float differenceMagnitude = targetPosition.x - _targetLastPosition.x;
		if (Mathf.Abs(differenceMagnitude) > movementThreshold * Time.fixedDeltaTime && !_isTriggered)
		{
			_isTriggered = true;
			_triggerTime = Time.time;
		}
		else if (Mathf.Abs(differenceMagnitude) < movementThreshold * Time.fixedDeltaTime)
		{
			_isTriggered = false;
		}
			

		if (_triggerTime + graceTime < Time.time)
			cameraTargetPosition = new Vector3(Math.Sign(differenceMagnitude) * movementOffset, 0, 0) + targetPosition;
		else
			cameraTargetPosition = targetPosition;

		if (cameraTargetPosition.x < xMinimum)
			cameraTargetPosition.x = xMinimum;

		if (cameraTargetPosition.x > xMaximum)
			cameraTargetPosition.x = xMaximum;

		if (cameraTargetPosition.y < yMinimum)
			cameraTargetPosition.y = yMinimum;

		if (cameraTargetPosition.y > yMaximum)
			cameraTargetPosition.y = yMaximum;


		Vector3 newPosition = Vector3.Lerp(transform.position, cameraTargetPosition, smoothingFactor);
		transform.position = newPosition;

		_targetLastPosition = targetPosition;
	}
}