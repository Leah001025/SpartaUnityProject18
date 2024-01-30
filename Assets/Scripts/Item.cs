using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Item Type 추가

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDetail;
    public Sprite itemImage;
    public Sprite useImage;
    public PlayerStat statModifiers;
}
