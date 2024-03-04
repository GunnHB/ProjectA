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

    private const string PLAYER = "Player";
    private const string PLAYER_RENDER_TEXTURE = "RenderTexturePlayer";

    private GameMode _gameMode;

    private bool _gamePause = false;

    public GameMode CurrGameMode => _gameMode;
    public bool GamePause => _gamePause;

    public UnityAction InGameModeAction;
    public UnityAction UIModeAction;

    private GameObject _playerObj;
    public GameObject PlayerObj { get { return GetObject(PLAYER, ref _playerObj); } }

    private GameObject _renderPlayerObj;
    public GameObject RenderPlayerObj { get { return GetObject(PLAYER_RENDER_TEXTURE, ref _renderPlayerObj); } }

    protected override void Awake()
    {
        base.Awake();

        InitializeModel();
    }

    // 테이블 추가하면 반드시 해줘야 합니당
    private void InitializeModel()
    {
        ModelCategoryTab.Model.Initialize();
        ModelItem.Model.Initialize();
        ModelWeapon.Model.Initialize();
        ModelShield.Model.Initialize();
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

    private GameObject GetObject(string objName, ref GameObject obj)
    {
        if (obj == null)
        {
            var tempObj = GameObject.Find(objName);

            if (tempObj != null)
                obj = tempObj;
        }

        return obj;
    }
}
