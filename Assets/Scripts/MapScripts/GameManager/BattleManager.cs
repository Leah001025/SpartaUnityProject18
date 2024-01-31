using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public bool nowBattle = false;
    public bool bossDead = false;

    private HealthSystem playerhealthSystem;
    public RoomInfo playerWhereIs;

    public List<GameObject> BossCount;
    public List<GameObject> MonsterCountList;
    public Transform Player { get; private set; }
    [SerializeField] private string PlayerTag = "Player";

    public int currentMonsterCount;
    [Range(0, 1)]public float playerCurrentHealth;
    public bool IsGameOver = false;

    private void Awake()
    {
        instance = this;

        Player = GameObject.FindGameObjectWithTag(PlayerTag).transform;

        playerhealthSystem = Player.GetComponent<HealthSystem>();
        playerhealthSystem.OnDamage += UpdateHealthUI;
        playerhealthSystem.OnHeal += UpdateHealthUI;
        playerhealthSystem.OnDeath += GameOver;
    }

    private void GameOver()
    {
        IsGameOver = true;
    }

    private void UpdateHealthUI()
    {
        playerCurrentHealth = playerhealthSystem.CurrentHealth / playerhealthSystem.MaxHealth;// 현재 체력을 slider로 표시하기 위한 값
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMonsterCount = 0;

        RandomWalkGenerator.Instance.GenerateDungeon();
        UpdateHealthUI();

        var character = GameManager.Instance.CharacterList.Find(item => item.CharacterType == GameManager.Instance.nowCharacter);//GameManager에서 현재 캐릭터 정보를 받아옴

        Player.transform.Find("MainSprite").GetComponent<SpriteRenderer>().sprite = character.CharacterSprite;//캐릭터의 스프라이트를 변경
        Player.transform.Find("MainSprite").GetComponent<Animator>().runtimeAnimatorController = character.animatorController;//캐릭터의 애니메이션을 변경
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nowBattle)//전투중일 때
        {
            foreach(RoomInfo room in RoomList.DungeonRooms)
            {
                room.CloseDoor();//모든 문 닫음
            }
        }
        else//아닐 때
        {
            foreach (RoomInfo room in RoomList.DungeonRooms)
            {
                room.OpenDoor();//모든 문 열기
            }
        }
    }

    private void Update()
    {
        UpdateCount();

        if (BossCount.Count == 0)//List의 수가 0이면(보스 오브젝트가 없으면)
        {
            bossDead = true;
        }
        else
        {
            for(int i = 0; i < BossCount.Count; i++)
            {
                if (BossCount[i] == null)//만일 보스몬스터가 죽었다면(보스 오브젝트가 destroy되었다면),
                {
                    BossCount.RemoveAt(i);//List삭제
                }
            }
        }
    }

    private void UpdateCount()
    {
        currentMonsterCount = 0;
        foreach(GameObject count in MonsterCountList)//모든 방에 남아있는 몬스터 수 세기
        {
            currentMonsterCount += count.transform.childCount;
        }
    }
}
