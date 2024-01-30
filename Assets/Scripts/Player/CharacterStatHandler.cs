using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatHandler : MonoBehaviour
{
    [SerializeField] private PlayerStat baseStats;
    public PlayerStat CurrentStats { get; private set; }
    public List<PlayerStat> statsModifiers = new List<PlayerStat>();

    private void Awake()
    {
        UpdateCharacterStats();
    }

    private void UpdateCharacterStats()
    {
        AttackSO attackSO = null;
        if (baseStats.attackSO != null)
        {
            attackSO = Instantiate(baseStats.attackSO);
        }

        CurrentStats = new PlayerStat { attackSO = attackSO };
        // TODO
        CurrentStats.statsChangeType = baseStats.statsChangeType;
        CurrentStats.maxHealth = baseStats.maxHealth;
        CurrentStats.currentHealth = baseStats.maxHealth;
        CurrentStats.speed = baseStats.speed;

    }
}