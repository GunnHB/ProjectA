using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonObject<GameManager>
{

    private bool _gamePause = false;

    public bool GamePause => _gamePause;

    /// <summary>
    /// 일시정지 / 재개
    /// </summary>
    public void PauseGame()
    {
        _gamePause = !_gamePause;

        Time.timeScale = _gamePause ? 0 : 1;
    }
}
