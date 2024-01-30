using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    private bool isLive = true;
    [SerializeField] private Transform target;
    private Vector2 pos;
    private float power = 0;
    private CharacterStatHandler characterStatHandler = null;
    private float hp = 6;
    private bool isMoveAround = false;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private Boss bossScript;
    private bool isBossLive = true;

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
        //IsBossMonsterDie();
        Move();
    }

    // �����濡�� ������ �׾����� Ȯ��
    private void IsBossMonsterDie()
    {
        isBossLive = bossScript.isLive;

        // ������ �׾��ٸ�
        if (!isBossLive)
        {
            isLive = false;
            Die();
        }
    }

    // ���� ������
    private void Move()
    {
        float distance = Vector2.Distance(target.position, transform.position);

        // �÷��̾ ���Ͱ� �ν��� �� �ִ� ������ ���� ��
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

    // �÷��̾ �����Ѵ�.
    private void FollowTarget()
    {
        if (isLive)
        {
            // �÷��̾ �ִ� �������� ���� �̹��� ���� ��ȯ
            spriter.flipX = target.position.x > rigid.position.x;

            // �÷��̾ �ִ� ���� ���ؼ� ������
            Vector2 dirVec = target.position - transform.position;
            Vector2 nextVec = dirVec.normalized * speed * 2.0f * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }

    // ���Ͱ� ���ƴٴѴ�.
    private void MoveAround()
    {
        if (isLive)
        {
            if (!isMoveAround)
            {
                NextPos();
            }
            isMoveAround = true;

            // �����̴� �������� �̹��� ��ȯ
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

    // ���ݹ޾��� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            hp -= power;

            if (hp <= 0)
            {
                isLive = false;
                Die();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("Change hs");
            HealthSystem healthSystem = GameObject.Find("Player").GetComponent<HealthSystem>();
            healthSystem.ChangeHealth(-1);
        }
    }

    private void OnCollisionExit2D(Collision2D col) 
    {
        
    }

    // �׾��� ��
    private void Die()
    {
        Destroy(gameObject);
    }
}
