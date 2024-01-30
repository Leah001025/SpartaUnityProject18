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

    public void OnGoEndingButton()//게임 클리어 버튼 눌렀을 때
    {

    }

    public void OnGameOverButton()//게임 오버 버튼 눌렀을 때
    {
        SceneManager.LoadScene("StartScene");
    }
}
