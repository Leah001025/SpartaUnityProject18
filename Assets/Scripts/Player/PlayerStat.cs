using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int maxHealth = 100; // �ִ� ü��
    public int currentHealth; // ���� ü��
    public int atk = 10; // ���ݷ�
    public int def = 5; // ����

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) // ���� -> �÷��̾�
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

    //    // ������ ü�� ����
    //    Monster.DamageToMonster(DamageToMonster);
    //}

    void Die() // ���
    {
        Debug.Log("Player has died!");
        // �߰� ����
    }
}
