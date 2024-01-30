using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour //Inventory
{
    public static Inventory ins { get; private set; }
    public static bool inventoryActivated = false;
    public List<Item> items;
    private float time = 0f;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bag;
    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;
    private HealthSystem _healthSystem;

    private void OnValidate() // change slots if changed by editor
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    void Awake() 
    {
        ins = this;
        FreshSlot();
    }

    void Update()
    {
        TryOpenInventory();
    }

    void FixedUpdate()
    {
        if(Managers.Game.useWatch == true)
        {
            time += Time.deltaTime;
            if(time < 4f) BattleManager.instance.nowBattle = false;
            else 
            {
                time = 0;
                Managers.Game.useWatch = false;
            }
        }
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated) OpenInventory();
            else CloseInventory();
        }
    }

    private void OpenInventory()
    {
        bag.SetActive(true);
    }

    private void CloseInventory()
    {
        bag.SetActive(false);
    }

    public void FreshSlot() // reload slots & show items
    {
        int i = 0;
        for(; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for(; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _item)
    {
        Debug.Log("실행");
        if(items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot();
        }
        else
        {
            Debug.Log("슬롯이 가득 차 있습니다.");
        }
    }

    public void RemoveItem(Item _item)
    {
        items.Remove(_item);
        FreshSlot();
    }


    public void UseItem(Item item)
    {
        if(item.name == "Watch")
        {
            Managers.Game.useWatch = true;
        }
        else if(item.name == "Skateboard")
        {
            Debug.Log("이동속도 증가");
            // 이속증가
            CharacterStatHandler statHandler = player.GetComponent<CharacterStatHandler>();
            statHandler.AddStatModifier(item.statModifiers);
        }
        else if(item.name == "LemonPie")
        {
            Debug.Log("레몬파이사용");
            // hp 회복
            _healthSystem = player.GetComponent<HealthSystem>();
            _healthSystem.ChangeHealth(20);
        }
        else if(item.name == "RunningShoes")
        {
            Debug.Log("운동화사용");
            // 데미지 증가
            CharacterStatHandler statHandler = player.GetComponent<CharacterStatHandler>();
            statHandler.AddStatModifier(item.statModifiers);
        }
        else 
        {
            Debug.Log("NullItemException");
        }
        RemoveItem(item);
    }

}
