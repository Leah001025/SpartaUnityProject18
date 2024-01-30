using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : Animations
{
    private HealthSystem _healthSystem;
    private static readonly int IsHit = Animator.StringToHash("IsHit");
    protected override void Awake()
    {
        base.Awake();
        _healthSystem = GetComponent<HealthSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        controller.OnMoveEvent += Animation;

        if (_healthSystem != null)
        {
            _healthSystem.OnDamage += Hit;
            _healthSystem.OnInvincibilityEnd += InvincibilityEnd;
        }

    }

    

    public void Animation(Vector2 direction)
    {
        animator.SetBool("IsFrontRun", direction.y < 0f);
        animator.SetBool("IsBackRun", direction.y > 0f);
        animator.SetBool("IsLeftRun", direction.x < 0f);
        animator.SetBool("IsRightRun", direction.x > 0f);
    }

    private void Hit()
    {
        animator.SetBool(IsHit, true);
    }

    private void InvincibilityEnd()
    {
        animator.SetBool(IsHit, false);
    }
}
