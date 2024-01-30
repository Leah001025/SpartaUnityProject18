using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    public bool isLive = true;
    private Vector2 pos;
    private int hp = 30;
    private bool isMoveAround = false;

    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform[] spawnPoints;

    Rigidbody2D rigid;
    public Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        StartCoroutine(MotionCheck());
    }

    bool isMotion = true;

    IEnumerator MotionCheck()
    {
        
        while (isMotion)
        {
            Motion();
            yield return new WaitForSeconds(3.0f);
        }
        yield break;
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
        if (random < 1)
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
            monster.transform.parent = GameObject.Find("Monsters").transform;
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
            hp--;

            if (hp <= 0)
            {
                isLive = false;
                Die();

                // ���� ���� ����

            }
        }
    }

    // �׾��� ��
    private void Die()
    {
        Destroy(gameObject);
    }
}
