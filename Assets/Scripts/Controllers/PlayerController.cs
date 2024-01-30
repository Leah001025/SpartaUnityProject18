using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // event 외부에서는 호출하지 못하게 막는다
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<AttackSO> OnAttackEvent;

    private float _timeSinseLastAttack = float.MaxValue;
    protected bool IsAttacking { get; set; }
    protected CharacterStatHandler Stats { get; private set; }
    protected virtual void Awake()
    {
        Stats = GetComponent<CharacterStatHandler>();
    }

    protected virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStats.attackSO == null)
            return;
        if (_timeSinseLastAttack <= Stats.CurrentStats.attackSO.delay)
        {
            _timeSinseLastAttack += Time.deltaTime;
        }

        if (IsAttacking && _timeSinseLastAttack > Stats.CurrentStats.attackSO.delay)
        {
            _timeSinseLastAttack = 0;
            callAttackEvent(Stats.CurrentStats.attackSO);
        }
    }
    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void callAttackEvent(AttackSO attackSO)
    {
        OnAttackEvent?.Invoke(attackSO);
    }
}
// 이동 처리 연습
//[SerializeField] private float speed = 5f;
//// Start is called before the first frame update
//void Start()
//{

//}

//// Update is called once per frame
//void Update()
//{
//    //float x = Input.GetAxis("Horizontal");
//    //float y = Input.GetAxis("Vertical");

//    //transform.position += new Vector3(x, y) * speed * Time.deltaTime;
//}
