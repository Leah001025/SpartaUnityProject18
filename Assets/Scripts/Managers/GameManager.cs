using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Conan,
    Sera
}

[System.Serializable]

public class Character
{
    public CharacterType CharacterType;
    public Sprite CharacterSprite;
    public RuntimeAnimatorController animatorController;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Character> CharacterList = new List<Character>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
