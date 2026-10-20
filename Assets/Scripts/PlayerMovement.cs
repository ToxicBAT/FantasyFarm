
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private VirtualJoystick joystick;
    [SerializeField] private bool useKeyboardInEditor = true;

    private CharacterController controller;
    private float verticalVelocity;
    private Vector3 lastMoveDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector2 input = joystick != null
            ? joystick.Direction
            : Vector2.zero;

#if UNITY_EDITOR
        if (useKeyboardInEditor &&
            Keyboard.current != null)
        {
            Vector2 keyboard = Vector2.zero;

            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
                keyboard.x -= 1f;

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
                keyboard.x += 1f;

            if (Keyboard.current.sKey.isPressed ||
                Keyboard.current.downArrowKey.isPressed)
                keyboard.y -= 1f;

            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.upArrowKey.isPressed)
                keyboard.y += 1f;

            if (keyboard.sqrMagnitude > 0f)
                input = Vector2.ClampMagnitude(keyboard, 1f);
        }
#endif

        Vector3 moveDirection =
            new(input.x, 0f, input.y);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            moveDirection.Normalize();
            lastMoveDirection = moveDirection;

            // Face the direction of travel.
            transform.forward = moveDirection;
        }

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    public Vector3 FacingDirection => lastMoveDirection;
}
