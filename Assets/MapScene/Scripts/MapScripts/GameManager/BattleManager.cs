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
        playerCurrentHealth = playerhealthSystem.CurrentHealth / playerhealthSystem.MaxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMonsterCount = 0;

        RandomWalkGenerator.Instance.GenerateDungeon();
        UpdateHealthUI();

        var character = GameManager.Instance.CharacterList.Find(item => item.CharacterType == GameManager.Instance.nowCharacter);

        Player.transform.Find("MainSprite").GetComponent<SpriteRenderer>().sprite = character.CharacterSprite;
        Player.transform.Find("MainSprite").GetComponent<Animator>().runtimeAnimatorController = character.animatorController;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nowBattle)
        {
            foreach(RoomInfo room in RoomList.DungeonRooms)
            {
                room.CloseDoor();
            }
        }
        else
        {
            foreach (RoomInfo room in RoomList.DungeonRooms)
            {
                room.OpenDoor();
            }
        }
    }

    private void Update()
    {
        UpdateCount();

        if (BossCount.Count == 0)
        {
            bossDead = true;
        }
        else
        {
            for(int i = 0; i < BossCount.Count; i++)
            {
                if (BossCount[i] == null)
                {
                    BossCount.RemoveAt(i);
                }
            }
        }
    }

    private void UpdateCount()
    {
        currentMonsterCount = 0;
        foreach(GameObject count in MonsterCountList)
        {
            currentMonsterCount += count.transform.childCount;
        }
    }
}
