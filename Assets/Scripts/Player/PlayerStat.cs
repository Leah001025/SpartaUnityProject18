using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsChangeType
{
    Add,
    Multiple,
    Override,
}

[Serializable]
public class PlayerStat
{
    public StatsChangeType statsChangeType;
    [Range(1, 100)] public float maxHealth;
    [Range(1, 100)] public float currentHealth;
    [Range(1f, 20f)] public float speed;
    public AttackSO attackSO;
}