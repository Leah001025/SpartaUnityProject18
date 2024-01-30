using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterStatHandler : MonoBehaviour
{
    private const float MinAttackDelay = 0.03f;
    private const float MinAttackPower = 0.5f;
    private const float MinAttackSize = 0.4f;
    private const float MinAttackSpeed = .1f;

    private const float MinSpeed = 0.8f;

    private const int MinMaxHealth = 5;
    [SerializeField] private PlayerStat baseStats;
    public PlayerStat CurrentStats { get; private set; }
    public List<PlayerStat> statsModifiers = new List<PlayerStat>();

    private void Awake()
    {
        UpdateCharacterStats();
    }

    public void AddStatModifier(PlayerStat statModifier)
    {
        statsModifiers.Add(statModifier);
        UpdateCharacterStats();
    }

    public void RemoveStatModifier(PlayerStat statModifier)
    {
        statsModifiers.Remove(statModifier);
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
        CurrentStats.speed = baseStats.speed;
        UpdateStats((a, b) => b, baseStats);
        if (CurrentStats.attackSO != null)
        {
            CurrentStats.attackSO.target = baseStats.attackSO.target;
        }

        foreach (PlayerStat modifier in statsModifiers.OrderBy(o => o.statsChangeType))
        {
            if (modifier.statsChangeType == StatsChangeType.Override)
            {
                UpdateStats((o, o1) => o1, modifier);
            }
            else if (modifier.statsChangeType == StatsChangeType.Add)
            {
                UpdateStats((o, o1) => o + o1, modifier);
            }
            else if (modifier.statsChangeType == StatsChangeType.Multiple)
            {
                UpdateStats((o, o1) => o * o1, modifier);
            }
        }

        LimitAllStats();
    }

    private void UpdateStats(Func<float, float, float> operation, PlayerStat newModifier)
    {
        CurrentStats.maxHealth = (int)operation(CurrentStats.maxHealth, newModifier.maxHealth);
        CurrentStats.speed = operation(CurrentStats.speed, newModifier.speed);

        if (CurrentStats.attackSO== null || newModifier.attackSO == null)
           return;

        UpdateAttackStats(operation, CurrentStats.attackSO, newModifier.attackSO);

        if (CurrentStats.attackSO.GetType() != newModifier.attackSO.GetType())
        {
            return;
        }
    }

    private void UpdateAttackStats(Func<float, float, float> operation, AttackSO currentAttack, AttackSO newAttack)
    {
        if (currentAttack == null || newAttack == null)
        {
            return;
        }

        currentAttack.delay = operation(currentAttack.delay, newAttack.delay);
        currentAttack.power = operation(currentAttack.power, newAttack.power);
        currentAttack.size = operation(currentAttack.size, newAttack.size);
        currentAttack.speed = operation(currentAttack.speed, newAttack.speed);
    }

    private void LimitStats(ref float stat, float minVal)
    {
        stat = Mathf.Max(stat, minVal);
    }

    private void LimitAllStats()
    {
        if (CurrentStats == null || CurrentStats.attackSO == null)
        {
            return;
        }

        LimitStats(ref CurrentStats.attackSO.delay, MinAttackDelay);
        LimitStats(ref CurrentStats.attackSO.power, MinAttackPower);
        LimitStats(ref CurrentStats.attackSO.size, MinAttackSize);
        LimitStats(ref CurrentStats.attackSO.speed, MinAttackSpeed);
        LimitStats(ref CurrentStats.speed, MinSpeed);
        CurrentStats.maxHealth = Mathf.Max(CurrentStats.maxHealth, MinMaxHealth);
    }

}