using UnityEngine;
using UnityEngine.InputSystem;

public class Player_InputRecive : MonoBehaviour
{
    public Vector2 lookInput {  get; private set; }
    public Vector2 moveInput { get; private set; }
    public bool hasJumped {  get; private set; }
    public bool hasInteracted {  get; private set; }
    public bool hasDroped { get; private set; }

    private void LateUpdate()
    {
        hasJumped = false;
        hasInteracted = false;
        hasDroped = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) hasJumped = true;
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started) hasInteracted = true;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.started) hasDroped = true;
    }
}
