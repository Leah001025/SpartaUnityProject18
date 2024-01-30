using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int maxHealth = 100; // 최대 체력
    public int currentHealth; // 현재 체력
    public int atk = 10; // 공격력
    public int def = 5; // 방어력

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) // 몬스터 -> 플레이어
    {
        int actualDamage = Mathf.Max(0, damage - def);
        currentHealth -= actualDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //public void DamageToMonster(int damage)
    //{
    //    int DamageToMonster = Mathf.Max(0, damage - Monster.def);

    //    // 몬스터의 체력 감소
    //    Monster.DamageToMonster(DamageToMonster);
    //}

    void Die() // 사망
    {
        Debug.Log("Player has died!");
        // 추가 예정
    }
}
