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

    public void CharacterSelectToIntro(int type)
    {
        SceneManager.LoadScene("IntroScene");
        GameManager.Instance.nowCharacter = (CharacterType)type;
    }

    public void OnGoEndingButton()//���� Ŭ���� ��ư ������ ��
    {
        SceneManager.LoadScene("EndingScene");
    }

    public void OnGameOverButton()//���� ���� ��ư ������ ��
    {
        SceneManager.LoadScene("StartScene");
    }
}
