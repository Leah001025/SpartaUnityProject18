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
        playerCurrentHealth = playerhealthSystem.CurrentHealth / playerhealthSystem.MaxHealth;// ���� ü���� slider�� ǥ���ϱ� ���� ��
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMonsterCount = 0;

        RandomWalkGenerator.Instance.GenerateDungeon();
        UpdateHealthUI();

        var character = GameManager.Instance.CharacterList.Find(item => item.CharacterType == GameManager.Instance.nowCharacter);//GameManager���� ���� ĳ���� ������ �޾ƿ�

        Player.transform.Find("MainSprite").GetComponent<SpriteRenderer>().sprite = character.CharacterSprite;//ĳ������ ��������Ʈ�� ����
        Player.transform.Find("MainSprite").GetComponent<Animator>().runtimeAnimatorController = character.animatorController;//ĳ������ �ִϸ��̼��� ����
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nowBattle)//�������� ��
        {
            foreach(RoomInfo room in RoomList.DungeonRooms)
            {
                room.CloseDoor();//��� �� ����
            }
        }
        else//�ƴ� ��
        {
            foreach (RoomInfo room in RoomList.DungeonRooms)
            {
                room.OpenDoor();//��� �� ����
            }
        }
    }

    private void Update()
    {
        UpdateCount();

        if (BossCount.Count == 0)//List�� ���� 0�̸�(���� ������Ʈ�� ������)
        {
            bossDead = true;
        }
        else
        {
            for(int i = 0; i < BossCount.Count; i++)
            {
                if (BossCount[i] == null)//���� �������Ͱ� �׾��ٸ�(���� ������Ʈ�� destroy�Ǿ��ٸ�),
                {
                    BossCount.RemoveAt(i);//List����
                }
            }
        }
    }

    private void UpdateCount()
    {
        currentMonsterCount = 0;
        foreach(GameObject count in MonsterCountList)//��� �濡 �����ִ� ���� �� ����
        {
            currentMonsterCount += count.transform.childCount;
        }
    }
}
