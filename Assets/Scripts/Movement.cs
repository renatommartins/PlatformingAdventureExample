﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	enum PlayerStates
	{
		Idle,
		LowStaminaIdle,
		Running,
		Jumping,
		Falling
	}

	public float walkSpeed = 5;
	public float jumpSpeed = 15;
	public int maxJumps = 2;
	[Space(20)]

	public Collider2D groundHitbox;
	public LayerMask groundLayers;

	private Rigidbody2D _rigidbody2D;
	private bool _isGrounded = false;
	private int _remainingJumps = 0;
	private float _leaveGroundTime = 0;

	private PlayerStates _state = PlayerStates.Idle;
	private Animator _animator = null;

	public Vector2 Speed { get; private set; }
	public bool IsGrounded { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
		// Get Rigidbody2D instance from current GameObject.
		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		_animator = gameObject.GetComponent<Animator>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		CheckGrounded(collision);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		CheckGrounded(collision);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (_isGrounded)
			_isGrounded = false;
	}

	private void CheckGrounded(Collision2D collision)
	{
		bool collidedWithGround = false;
		List<ContactPoint2D> contacts = new List<ContactPoint2D>();
		collision.GetContacts(contacts);
		foreach (ContactPoint2D contact in contacts)
			if (Vector2.Dot(contact.normal, Vector2.up) > 0.6)
				collidedWithGround = true;

		if (collidedWithGround && (_leaveGroundTime + 0.2) - Time.time < 0)
		{
			_isGrounded = true;
			_remainingJumps = maxJumps;
		}
			
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
			_isGrounded = false;
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
			newVelocity.y < 0)
		{
			newVelocity.y = jumpSpeed;
			_remainingJumps--;
			newVelocity.y += Physics2D.gravity.y * Time.deltaTime;
		}
		// Apply gravity.
		else
			newVelocity.y += Physics2D.gravity.y * Time.deltaTime;

		IsGrounded = _isGrounded;
		Speed = newVelocity;

		// Apply calculated velocity to character's Rigidbody2D
		_rigidbody2D.velocity = newVelocity;

		if (newVelocity.y > 0.1)
			_state = PlayerStates.Jumping;
		else if (newVelocity.y < -0.1)
			_state = PlayerStates.Falling;
		else if (newVelocity.x < 0)
			_state = PlayerStates.Running;
		else if (newVelocity.x > 0)
			_state = PlayerStates.Running;
		else
			_state = PlayerStates.Idle;

		if (newVelocity.x < 0)
			transform.localScale = new Vector3(-6, 6, 1);
		else if (newVelocity.x > 0)
			transform.localScale = new Vector3(6, 6, 1);

		switch (_state)
		{
			case PlayerStates.Idle:
				_animator.SetBool("isIdle", true);
				_animator.SetBool("isRunning", false);
				_animator.SetBool("isJumping", false);
				_animator.SetBool("isFalling", false);
				_animator.SetBool("isLowStamina", false);
				break;
			case PlayerStates.LowStaminaIdle:
				_animator.SetBool("isIdle", true);
				_animator.SetBool("isRunning", false);
				_animator.SetBool("isJumping", false);
				_animator.SetBool("isFalling", false);
				_animator.SetBool("isLowStamina", true);
				break;
			case PlayerStates.Running:
				_animator.SetBool("isIdle", false);
				_animator.SetBool("isRunning", true);
				_animator.SetBool("isJumping", false);
				_animator.SetBool("isFalling", false);
				_animator.SetBool("isLowStamina", false);
				break;
			case PlayerStates.Jumping:
				_animator.SetBool("isIdle", false);
				_animator.SetBool("isRunning", false);
				_animator.SetBool("isJumping", true);
				_animator.SetBool("isFalling", false);
				_animator.SetBool("isLowStamina", false);
				break;
			case PlayerStates.Falling:
				_animator.SetBool("isIdle", false);
				_animator.SetBool("isRunning", false);
				_animator.SetBool("isJumping", false);
				_animator.SetBool("isFalling", true);
				_animator.SetBool("isLowStamina", false);
				break;
		}
	}
}
