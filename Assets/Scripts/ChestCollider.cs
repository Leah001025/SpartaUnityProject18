using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;

public class ChestCollider : MonoBehaviour // ChestSprite
{
    Transform target = null;
    private Animator animator;
    [SerializeField] private GameObject EKey;
    [SerializeField] private Item item;
    [SerializeField] private GameObject ChestOpened;
    private ItemPopup itemPopup;
    
    void Awake()
    {
        itemPopup = GameObject.Find("ItemPopup").GetComponent<ItemPopup>();
        Debug.Log(itemPopup);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target != null)
        {
            if(Input.GetKeyDown(KeyCode.E)) 
            {
                OpenChestInvoke();
            }
            // popup E button and get key...
        }
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        Debug.Log("collide");
        var obj = col.gameObject;
        if (obj.tag == "Player")
        {
            target = col.gameObject.transform;
        }
        ShowEKey();
    }

    private void OnCollisionExit2D(Collision2D col) 
    {
        target = null;
        UnShowEKey();
    }

    private void ShowEKey()
    {
        EKey.SetActive(true);
    }

    private void UnShowEKey()
    {
        EKey.SetActive(false);
    }

    private void OpenChest()
    {
        Inventory.ins.AddItem(item);
        itemPopup.ShowItemPopup(item);
        ChestOpened.SetActive(true);
        Destroy(gameObject);
    }

    public void OpenChestInvoke()
    {
        animator.SetBool("IsOpen", true);
        Invoke("OpenChest", 0.8f);
    }
}