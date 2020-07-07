using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public float walkSpeed = 5;
	public float jumpSpeed = 15;

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
		//bool isJumping = Input.GetKey(KeyCode.Space);

		if (isLeftPressed)
			_rigidbody2D.velocity = new Vector2(-walkSpeed, _rigidbody2D.velocity.y);
		else if (isRightPressed)
			_rigidbody2D.velocity = new Vector2(walkSpeed, _rigidbody2D.velocity.y);
		else
			_rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

		/*if (isJumping && _rigidbody2D.IsTouchingLayers(LayerMask.GetMask(new string[] { "Ground" })))
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);

		if (!_rigidbody2D.IsTouchingLayers(LayerMask.GetMask(new string[] { "Ground" })))
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y + Physics2D.gravity.y);
		else
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);*/
	}
}
