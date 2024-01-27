using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;

    private void OnValidate() {
        slots = slotParent.GetComponentsInChildren<Slot>();    
    }
}
