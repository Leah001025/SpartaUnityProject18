using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : UIBase // 추후 ItemPopup.cs와 겹치는 내용 UIBase로 병합
{
    [SerializeField] private GameObject chestSprite;
    [SerializeField] private GameObject chestOpened;
    [SerializeField] private GameObject pop;
    [SerializeField] private GameObject popBack;
    [SerializeField] private Text iname;
    [SerializeField] private Text idetail;
    [SerializeField] private Image iimg;

    public void ShowItemPopup(Item item)
    {
        iname.text = item.itemName;
        idetail.text = item.itemDetail;
        iimg.sprite = item.itemImage;
        popBack.SetActive(true);
        pop.SetActive(true);
    }

    public void ClosePopup()
    {
        pop.SetActive(false);
        popBack.SetActive(false);
        Destroy(chestSprite);
        chestOpened.SetActive(true);
    }

}
