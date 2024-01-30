using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    Start,
    InGame,
    Paused,
    GameOver
}

public enum CharacterType
{
    Conan,
    Sera
}

[Serializable]
public class Character
{
    public CharacterType CharacterType;
    public Sprite CharacterSprite;
    public RuntimeAnimatorController animatorController;
}

public class Stat
{
    [SerializeField] private int baseStat;

    public int GetStat()
    {
        return baseStat;
    }
}

[Serializable]
public class GameData
{

}

public class GameManager
{
	GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

    public List<Character> CharacterList = new List<Character>();
    public GameState CurrentState { get; private set; }

    public void Init()
    {
        ChangeState(GameState.Start);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.InGame:
                HandleInGame();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    void HandleStart()
    {
        Debug.Log("Game Started");
        // �ʱ�ȭ ����: �÷��̾�, ����, �� ����
        //SceneManager.LoadScene // ���Ӿ� �ε�
        ChangeState(GameState.InGame);
    }

    void HandleInGame()
    {
        Debug.Log("Game is now in progress");
        // ���� ���� �� �ʿ��� ����: ���� Ÿ�̸�, �÷��̾� ���� üũ ��
        // ���� Ÿ�̸� ����, ���� ���� ���� ��
        // ���� ���� ������ üũ�ϴ� ����
    }

    void HandlePaused()
    {
        Debug.Log("Game Paused");
        // ���� �Ͻ� ���� ����: Ÿ�̸� ����, ���� ���� ����, �Ͻ� ���� UI ǥ��
        Time.timeScale = 0f;
        // �Ͻ� ���� ���� UI Ȱ��ȭ ��
    }

    public void ResumeGame()
    {
        ChangeState(GameState.InGame);
        Time.timeScale = 1f;
        // �Ͻ� ���� ���� UI ��Ȱ��ȭ
    }

    void HandleGameOver()
    {
        Debug.Log("Game Over");
        // ���� ���� ����: ���� ���� ���, ���� ���� ȭ�� ǥ��, ������ ����
        // SceneManager.LoadScene("GameOver"); // ���� ���� �� �ε� or UI Ȱ��ȭ
        // �߰����� ���� ���� ó�� �۾�...
    }

    #region Victory & Defeat
    void Victory()
    {
        // �¸� ó��
    }

    void Defeat()
    {
        // �й� ó��
    }
    #endregion

    #region Save & Load
    public string _path = Application.persistentDataPath + "/SaveData.json";
    
    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        File.WriteAllText(_path, jsonStr);
        Debug.Log($"Save Game Completed : {_path}");
    }

    public bool LoadGame()
    {
        if (File.Exists(_path) == false)
            return false;

        string fileStr = File.ReadAllText(_path);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data != null)
        {
            Managers.Game.SaveData = data;
        }

        Debug.Log($"Save Game Loaded : {_path}");
        return true;
    }
    
    #endregion
}