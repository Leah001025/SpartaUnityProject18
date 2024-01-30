using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : UIBase // 추후 ItemPopup.cs와 겹치는 내용 UIBase로 병합
{
    [SerializeField] private Image useImage;
    [SerializeField] private GameObject useBackground;
    [SerializeField] private GameObject bag;
    [SerializeField] private GameObject pop;
    [SerializeField] private Text iname;
    [SerializeField] private Text idetail;
    [SerializeField] private Image iimg;
    private Item thisItem = null;

    public void ShowItemPopup(Item item)
    {
        thisItem = item;
        bag.SetActive(false);
        iname.text = item.itemName;
        idetail.text = item.itemDetail;
        iimg.sprite = item.itemImage;
        pop.SetActive(true);
    }
    
    public void OnClickUseBtn()
    {
        pop.SetActive(false);
        ItemUseEffect();
        Inventory.ins.UseItem(thisItem);
    }

    public void OnClickExitBtn()
    {
        pop.SetActive(false);
        bag.SetActive(true);
    }

    public void ItemUseEffect()
    {
        useImage.sprite = thisItem.useImage;
        Animator anim = useBackground.GetComponent<Animator>();
        useBackground.SetActive(true);
        anim.SetBool("IsUse", true);
        Invoke("SetActiveFalse", 1.1f);
    }

    public void SetActiveFalse()
    {
        useBackground.SetActive(false);
    }
}
