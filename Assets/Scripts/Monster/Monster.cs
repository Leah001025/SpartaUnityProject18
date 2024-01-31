using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 몬스터 기본 스텟
    [SerializeField] private float speed = 2.0f;
    private float hp = 6;
    private Vector2 pos;
    private bool isLive = true;
    private bool isMoveAround = false;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    // 몬스터가 추적할 타겟
    [SerializeField] private Transform target;

    private float power = 0;
    private CharacterStatHandler characterStatHandler = null;

    private void Awake()
    {
        characterStatHandler = GameObject.Find("Player").GetComponent<CharacterStatHandler>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        power = characterStatHandler.CurrentStats.attackSO.power;
        Move();
    }

    // 몬스터 행동 결정
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

    // 플레이어를 추적
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

    // 몬스터 움직임
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

    // 몬스터 다음 움직임 결정
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
        if (collision.gameObject.tag == "Weapon")
        {
            hp -= power;

            if (hp <= 0)
            {
                isLive = false;
                Die();
            }
        }
    }

    // 플레이어 피격
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            HealthSystem healthSystem = GameObject.Find("Player").GetComponent<HealthSystem>();
            healthSystem.ChangeHealth(-1);
            if (characterStatHandler.CurrentStats.attackSO.isOnKnockback)
            {
                Debug.Log("knockbackon");
                Movement movement = col.gameObject.GetComponent<Movement>();
                if (movement != null)
                {
                    movement.ApplyKnockback(transform, characterStatHandler.CurrentStats.attackSO.knockbackPower, characterStatHandler.CurrentStats.attackSO.knockbackTime);
                }
            }
        }
    }

    // 죽었을 때
    private void Die()
    {
        Destroy(gameObject);
    }
}
