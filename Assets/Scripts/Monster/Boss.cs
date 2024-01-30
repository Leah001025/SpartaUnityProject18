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

    // 보스 행동 결정
    private void Motion()
    {
        int random = Random.Range(0, 5);

        // 20%의 확률로 몬스터 생성
        if (random < 1)
        {
            anim.SetBool("IsAtk", true);
            MonsterCreate();
        }
        // 80%의 확률로 보스 움직임
        else
        {
            anim.SetBool("IsAtk", false);
            MoveAround();
        }

    }

    // 몬스터 생성
    private void MonsterCreate()
    {
        for(int idx = 0; idx < spawnPoints.Length; idx++)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnPoints[idx].position, Quaternion.identity);
            monster.transform.parent = GameObject.Find("Monsters").transform;
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
        if (collision.gameObject.tag == "Weapon")
        {
            hp--;

            if (hp <= 0)
            {
                isLive = false;
                Die();

                // 게임 오버 구현

            }
        }
    }

    // 죽었을 때
    private void Die()
    {
        Destroy(gameObject);
    }
}
