using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupStartMenu : MonoBehaviour
{
    [SerializeField] private Image characterSprite;
    [SerializeField] private GameObject SelectCharacter;
    [SerializeField] private GameObject ConanInfo;
    [SerializeField] private GameObject SeraInfo;

    private CharacterType characterType;

    public void OnClickCharacter()
    {
        SelectCharacter.SetActive(true);
        ConanInfo.SetActive(false);
        SeraInfo.SetActive(false);
    }

    public void OnClickSelectCharacter(int index)
    {
        characterType = (CharacterType)index;
        var character = Managers.Game.CharacterList.Find(item => item.CharacterType == characterType);

        switch (index)
        {
            case 0:
                SelectCharacter.SetActive(false);
                ConanInfo.SetActive(true);
                SeraInfo.SetActive(false);

                break;
            case 1:
                SelectCharacter.SetActive(false);
                ConanInfo.SetActive(false);
                SeraInfo.SetActive(true);
                break;
        }

    }
}