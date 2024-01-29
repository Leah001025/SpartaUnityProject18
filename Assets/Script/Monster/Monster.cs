using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private bool isLive = true;
    [SerializeField] private Rigidbody2D target;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (isLive)
        {
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }

    private void LateUpdate()
    {
        if (isLive)
        {
            spriter.flipX = target.position.x > rigid.position.x;
        }
    }
}
