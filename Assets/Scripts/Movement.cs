using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public LayerMask _groundLayerMask;
	public float walkSpeed = 5;
	public float jumpSpeed = 15;
	public int maxJumps = 2;
	public LayerMask groundLayers;

	public bool isGrounded = false;

	private Rigidbody2D _rigidbody2D;
	private bool _isGrounded = false;
	private int _remainingJumps = 0;
	private float _leaveGroundTime = 0;

    // Start is called before the first frame update
    void Start()
    {
		// Get Rigidbody2D instance from current GameObject.
		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
		// Read input from player (ONLY READS INPUT FROM ONE PLAYER).
		bool isLeftPressed = Input.GetKey(KeyCode.A);
		bool isRightPressed = Input.GetKey(KeyCode.D);
		bool isJumping = Input.GetKeyDown(KeyCode.Space);

		// New velocity temporary vector.
		Vector2 newVelocity = _rigidbody2D.velocity;

		// Checks if character is grounded accounting for grace time when just jumped
		if (_rigidbody2D.IsTouchingLayers(groundLayers) && 
			(_leaveGroundTime + 0.2f) - Time.time < 0)
		{
			_isGrounded = true;
			_remainingJumps = maxJumps;
		}
		else
			_isGrounded = false;

		// Calculates horizontal movement.
		if (isLeftPressed && !isRightPressed)
			newVelocity.x = -walkSpeed;
		else if (isRightPressed && !isLeftPressed)
			newVelocity.x = walkSpeed;
		else
			newVelocity.x = 0;

		// Set vertical velocity to zero if player is grounded
		if (!isJumping && _isGrounded)
			newVelocity.y = 0;
		// When player press jump and on ground ,
		// Set vertical velocity to jump speed,
		// set time to check grace time and
		// and decrement jumps remaining.
		else
		if (isJumping && _isGrounded)
		{
			newVelocity.y = jumpSpeed;
			_remainingJumps--;

			_leaveGroundTime = Time.time;
		}
		// When player press jump and not on ground and
		// still has jumps remaining and starting to fall,
		// Set vertical velocity to jump speed
		// and decrement jumps remaining
		// and apply gravity.
		else
		if (isJumping && 
			!_isGrounded && 
			_remainingJumps > 0 && 
			newVelocity.y < -2.5f)
		{
			newVelocity.y = jumpSpeed;
			_remainingJumps--;
			newVelocity.y += Physics2D.gravity.y * Time.deltaTime;
		}
		// Apply gravity.
		else
			newVelocity.y += Physics2D.gravity.y * Time.deltaTime;

		// Apply calculated velocity to character's Rigidbody2D
		_rigidbody2D.velocity = newVelocity;
	}
}
