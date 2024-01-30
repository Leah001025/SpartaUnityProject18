using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : PlayerController 
{

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        CallMoveEvent(moveInput);
    }
    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(newAim);

        newAim = (worldPos - (Vector2)transform.position).normalized;
        CallLookEvent(newAim);
    }

    public void OnFire(InputValue value)
    {
        IsAttacking = value.isPressed;
    }
}
