using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonObject<GameManager>
{
    public enum GameMode
    {
        None = -1,
        InGame,
        UI,
    }

    private GameMode _gameMode;

    private bool _gamePause = false;

    public GameMode CurrGameMode => _gameMode;
    public bool GamePause => _gamePause;

    public UnityAction InGameModeAction;
    public UnityAction UIModeAction;

    protected override void Awake()
    {
        base.Awake();

        CategoryTabModel.Initialize();
    }

    public void SetGameMode(GameMode mode)
    {
        _gameMode = mode;

        switch (_gameMode)
        {
            case GameMode.InGame:
                InGameModeAction?.Invoke();
                break;
            case GameMode.UI:
                UIModeAction?.Invoke();
                break;
        }
    }

    /// <summary>
    /// 일시정지 / 재개
    /// </summary>
    public void PauseGame(bool doPause)
    {
        _gamePause = doPause;

        Time.timeScale = _gamePause ? 0 : 1;
    }
}
