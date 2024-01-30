using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    public bool isLive = true;
    private Vector2 pos;
    private float power = 0;
    private CharacterStatHandler characterStatHandler = null;
    private float hp = 30;
    private bool isMoveAround = false;

    bool isMotion = false;

    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform[] spawnPoints;

    Rigidbody2D rigid;
    public Animator anim;

    private void Awake()
    {
        characterStatHandler = GameObject.Find("Player").GetComponent<CharacterStatHandler>();
        rigid = GetComponent<Rigidbody2D>();

    }

    private void OnEnable()
    {
        StartCoroutine(MotionCheck());
        isMotion = true;
    }

    IEnumerator MotionCheck()
    {
        
        while (isMotion)
        {
            Motion();
            yield return new WaitForSeconds(3.0f);
        }
        yield break;
    }

    void FixedUpdate()
    {
        power = characterStatHandler.CurrentStats.attackSO.power;
    }
    //void FixedUpdate()
    //{
    //    //MoveAround();
    //    InvokeRepeating("Motion", 0f, 3f);
    //    //MonsterCreate();
    //}


    // ���� �ൿ ����
    private void Motion()
    {
        int random = Random.Range(0, 5);

        // 20%�� Ȯ���� ���� ����
        if (random < 3)
        {
            anim.SetBool("IsAtk", true);
            MonsterCreate();
        }
        // 80%�� Ȯ���� ���� ������
        else
        {
            anim.SetBool("IsAtk", false);
            MoveAround();
        }

    }

    // ���� ����
    private void MonsterCreate()
    {
        for(int idx = 0; idx < spawnPoints.Length; idx++)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnPoints[idx].position, Quaternion.identity);
            monster.transform.parent = GameObject.Find("MonsterCount").transform;
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
        if (collision.gameObject.tag == "Weapon")
        {
            hp -= power;

            if (hp <= 0)
            {
                isLive = false;
                Die();

                // ���� ���� ����

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        if(col.gameObject.tag == "Player")
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

    private void OnCollisionExit2D(Collision2D col) 
    {
        
    }

    // �׾��� ��
    private void Die()
    {
        Destroy(gameObject);
    }
}
