using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    [SerializeField] private GameObject _gameManager;
    public void StartToCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelectScene");
        DontDestroyOnLoad(_gameManager);
    }

    public void CharacterSelectToIntro()
    {
        SceneManager.LoadScene("IntroScene");
    }
}
