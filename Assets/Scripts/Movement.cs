using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public LayerMask _groundLayerMask;
	public float walkSpeed = 5;
	public float jumpSpeed = 15;

	public bool isGrounded = false;

	private Rigidbody2D _rigidbody2D;

	// Start is called before the first frame update
	void Start()
	{
		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		bool isLeftPressed = Input.GetKey(KeyCode.A);
		bool isRightPressed = Input.GetKey(KeyCode.D);
		bool isJumping = Input.GetKey(KeyCode.Space);

		Vector2 velocityUpdate = _rigidbody2D.velocity;

		if (_rigidbody2D.IsTouchingLayers(_groundLayerMask))
			isGrounded = true;
		else
			isGrounded = false;

		if (isLeftPressed && !isRightPressed)
			velocityUpdate.x = -walkSpeed;
		else if (isRightPressed && !isLeftPressed)
			velocityUpdate.x = walkSpeed;
		else
			velocityUpdate.x = 0;

		if (!isJumping && isGrounded)
			velocityUpdate.y = 0;
		else if (isJumping && isGrounded)
			velocityUpdate.y = jumpSpeed;
		else
			velocityUpdate.y += Physics2D.gravity.y * Time.deltaTime;

		_rigidbody2D.velocity = velocityUpdate;
	}
}
