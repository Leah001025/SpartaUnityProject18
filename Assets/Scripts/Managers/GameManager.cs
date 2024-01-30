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
        // 초기화 로직: 플레이어, 몬스터, 맵 세팅
        //SceneManager.LoadScene // 게임씬 로드
        ChangeState(GameState.InGame);
    }

    void HandleInGame()
    {
        Debug.Log("Game is now in progress");
        // 게임 진행 중 필요한 로직: 게임 타이머, 플레이어 상태 체크 등
        // 게임 타이머 시작, 적의 스폰 로직 등
        // 게임 오버 조건을 체크하는 로직
    }

    void HandlePaused()
    {
        Debug.Log("Game Paused");
        // 게임 일시 정지 로직: 타이머 정지, 게임 상태 저장, 일시 정지 UI 표시
        Time.timeScale = 0f;
        // 일시 정지 관련 UI 활성화 등
    }

    public void ResumeGame()
    {
        ChangeState(GameState.InGame);
        Time.timeScale = 1f;
        // 일시 정지 관련 UI 비활성화
    }

    void HandleGameOver()
    {
        Debug.Log("Game Over");
        // 게임 종료 로직: 최종 점수 계산, 게임 오버 화면 표시, 데이터 정리
        // SceneManager.LoadScene("GameOver"); // 게임 오버 씬 로드 or UI 활성화
        // 추가적인 게임 오버 처리 작업...
    }

    #region Victory & Defeat
    void Victory()
    {
        // 승리 처리
    }

    void Defeat()
    {
        // 패배 처리
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