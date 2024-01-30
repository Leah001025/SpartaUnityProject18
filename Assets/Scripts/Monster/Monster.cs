using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    private bool isLive = true;
    [SerializeField] private Transform target;
    private Vector2 pos;
    private int hp = 3;
    private bool isMoveAround = false;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private Boss bossScript;
    private bool isBossLive = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        //IsBossMonsterDie();
        Move();
    }

    // 보스방에서 보스가 죽었는지 확인
    private void IsBossMonsterDie()
    {
        isBossLive = bossScript.isLive;

        // 보스가 죽었다면
        if (!isBossLive)
        {
            isLive = false;
            Die();
        }
    }

    // 몬스터 움직임
    private void Move()
    {
        float distance = Vector2.Distance(target.position, transform.position);

        // 플레이어가 몬스터가 인식할 수 있는 범위에 있을 때
        if (distance < 16)
        {
            isMoveAround = false;
            FollowTarget();
        }
        else
        {
            MoveAround();
        }
    }

    // 플레이어를 추적한다.
    private void FollowTarget()
    {
        if (isLive)
        {
            // 플레이어가 있는 방향으로 몬스터 이미지 방향 전환
            spriter.flipX = target.position.x > rigid.position.x;

            // 플레이어가 있는 곳을 향해서 움직임
            Vector2 dirVec = target.position - transform.position;
            Vector2 nextVec = dirVec.normalized * speed * 2.0f * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }

    // 몬스터가 돌아다닌다.
    private void MoveAround()
    {
        if (isLive)
        {
            if (!isMoveAround)
            {
                NextPos();
            }
            isMoveAround = true;

            // 움직이는 방향으로 이미지 전환
            spriter.flipX = rigid.position.x + pos.x > rigid.position.x;

            rigid.velocity = pos * speed;

        }
    }

    private void NextPos()
    {
        pos.x = Random.Range(-1, 2);
        pos.y = Random.Range(-1, 2);
        Vector2 nextVec = pos.normalized * speed * Time.fixedDeltaTime;

        Invoke("NextPos", 2f);
    }

    // 공격받았을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            hp--;

            if (hp <= 0)
            {
                isLive = false;
                Die();
            }
        }
    }

    // 죽었을 때
    private void Die()
    {
        Destroy(gameObject);
    }
}
