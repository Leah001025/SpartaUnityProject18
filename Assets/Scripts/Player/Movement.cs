using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private PlayerController _controller;
    private CharacterStatHandler _stats;
    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody2D _rigidbody2D;


    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _stats = GetComponent<CharacterStatHandler>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementDirection);
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * _stats.CurrentStats.speed;

        _rigidbody2D.velocity = direction;
    }
}
