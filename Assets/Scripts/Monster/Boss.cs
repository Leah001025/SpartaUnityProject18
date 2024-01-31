using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Boss 기본 스텟
    [SerializeField] private float speed = 2.0f;
    private float hp = 60;
    private Vector2 pos;
    public bool isLive = true;
    private bool isMoveAround = false;

    Rigidbody2D rigid;
    public Animator anim;

    bool isMotion = false;

    // 소환할 몬스터 할당
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private float power = 0;
    private CharacterStatHandler characterStatHandler = null;

    private void Awake()
    {
        characterStatHandler = GameObject.Find("Player").GetComponent<CharacterStatHandler>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        power = characterStatHandler.CurrentStats.attackSO.power;
    }

    // Boss 움직임 코루틴 함수
    private void OnEnable()
    {
        StartCoroutine(MotionCheck());
        isMotion = true;
    }

    // Boss 움직임 딜레이 함수
    IEnumerator MotionCheck()
    {
        while (isMotion)
        {
            Motion();
            yield return new WaitForSeconds(3.0f);
        }
        yield break;
    }

    // Boss 행동 결정
    private void Motion()
    {
        int random = Random.Range(0, 5);

        // 40%의 확률로 몬스터 생성
        if (random < 2)
        {
            anim.SetBool("IsAtk", true);
            MonsterCreate();
        }
        // 60%의 확률로 보스 움직임
        else
        {
            anim.SetBool("IsAtk", false);
            MoveAround();
        }
    }

    // 몬스터 생성
    private void MonsterCreate()
    {
        for (int idx = 0; idx < spawnPoints.Length; idx++)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnPoints[idx].position, Quaternion.identity);
            monster.transform.parent = GameObject.Find("MonsterCount").transform;
        }
    }

    // Boss 움직임
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

    // Boss 다음 움직임 결정
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