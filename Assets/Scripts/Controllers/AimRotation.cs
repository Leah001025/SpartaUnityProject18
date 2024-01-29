using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer;
    
    private PlayerController _controller;
    // Start is called before the first frame update

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }
    void Start()
    {
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
