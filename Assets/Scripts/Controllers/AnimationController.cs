using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : Animations
{
    // Start is called before the first frame update
    void Start()
    {
        controller.OnMoveEvent += Animation;
    }

    public void Animation(Vector2 direction)
    {
        animator.SetBool("IsFrontRun", direction.y < 0f);
        animator.SetBool("IsBackRun", direction.y > 0f);
        animator.SetBool("IsLeftRun", direction.x < 0f);
        animator.SetBool("IsRightRun", direction.x > 0f);
    }
}
