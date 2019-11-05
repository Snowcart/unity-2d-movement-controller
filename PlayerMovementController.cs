using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	public UnityEvent OnLandEvent;
	public bool jumping = false;
	public bool isRight = true;
	[SerializeField] private float jumpForce = 100f;
	[SerializeField] private float runSpeed = 5f;
	[SerializeField] private float movmentSmoothing = 0.2f;
	[SerializeField] private float jumpTime = 0.3f;
	[SerializeField] private LayerMask ground;
	[SerializeField] private Transform groundCheck;
	private bool isGamepad;
	PlayerControls _controls;
	Rigidbody2D _rb;
	private bool jumpPressed = false;
	private float horizontalMove = 0f;
	private bool onGround;
	private Vector2 velocity = Vector2.zero;
	private const float onGroundRadius = .2f;

	void Awake()
	{
		_controls = new PlayerControls();
		_rb = GetComponent<Rigidbody2D>();
		if (OnLandEvent == null) OnLandEvent = new UnityEvent();
		IsGamepadAwake();
		HandleMovementInput();
	}

	void Update()
	{
		GetIsGamepad();
		if (!isGamepad)
		{
			HandleKeyboardMovement();
		}
	}

	void FixedUpdate()
	{
		Move(horizontalMove * Time.fixedDeltaTime);
		StartCoroutine(CheckIfGrounded());

		if (jumpPressed && !jumping && onGround)
		{
			jumping = true;
			StartCoroutine(Jump());
		}
	}

	IEnumerator Jump()
	{
		_rb.velocity = Vector2.zero;
		float timer = 0;
		Vector2 jumpVector = new Vector2(0f, jumpForce);

		while (jumpPressed && timer < jumpTime)
		{
			float propCompleted = timer / jumpTime;
			Vector2 thisFrameJumpVector = Vector2.Lerp(jumpVector, Vector2.zero, propCompleted);
			_rb.AddForce(thisFrameJumpVector);
			timer += Time.deltaTime;
			yield return null;
		}

		jumpPressed = false;
		jumping = false;
	}

	IEnumerator CheckIfGrounded()
	{
		bool wasGrounded = onGround;
		onGround = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, onGroundRadius, ground);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				onGround = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
			yield return null;
		}
	}

	void Move(float movmentSpeed)
	{
		Vector2 targetVelocity = new Vector2(movmentSpeed * 10f, _rb.velocity.y);
		_rb.velocity = Vector2.SmoothDamp(_rb.velocity, targetVelocity, ref velocity, movmentSmoothing);
		if (movmentSpeed > 0 && !isRight) FlipDirection();
		if (movmentSpeed < 0 && isRight) FlipDirection();
	}

	void FlipDirection()
	{
		isRight = !isRight;
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	void OnEnable()
	{
		_controls.Gameplay.Enable();
	}

	void onDisable()
	{
		_controls.Gameplay.Disable();
	}

	void HandleMovementInput()
	{
		if (isGamepad)
		{
			HandleHorizontalInput();
		}
		HandleJumpInput();
	}

	private void HandleKeyboardMovement()
	{
		var kb = Keyboard.current;
		if (kb.rightArrowKey.isPressed)
		{
			if (kb.leftArrowKey.isPressed) horizontalMove = 0;
			else horizontalMove = 1 * runSpeed;
		}

		if (kb.leftArrowKey.isPressed)
		{
			if (kb.rightArrowKey.isPressed) horizontalMove = 0;
			else horizontalMove = -1 * runSpeed;
		}

		if (kb.leftArrowKey.wasReleasedThisFrame || kb.rightArrowKey.wasReleasedThisFrame) horizontalMove = 0;
	}

	void HandleHorizontalInput()
	{
		_controls.Gameplay.Horizontal.performed += c => horizontalMove = c.ReadValue<float>() * runSpeed;
	}

	void HandleJumpInput()
	{
		_controls.Gameplay.Jump.performed += j => jumpPressed = true;
		_controls.Gameplay.Jump.canceled += c => jumpPressed = false;
	}

	void GetIsGamepad()
	{
		InputSystem.onDeviceChange += (device, change) =>
		{
			if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected && device is Gamepad)
			{
				isGamepad = true;
			};

			if (change == InputDeviceChange.Removed || change == InputDeviceChange.Disconnected)
			{
				if (!InputSystem.devices.Any(d => d is Gamepad))
				{
					isGamepad = false;
				};
			}

		};
	}

	void IsGamepadAwake()
	{
		var devices = InputSystem.devices;
		for (var i = 0; i < devices.Count; ++i)
		{
			var device = devices[i];
			if (device is Gamepad) isGamepad = true;
		}
	}
}
