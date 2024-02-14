using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonObject<GameManager>
{

    private bool _gamePause = false;

    public bool GamePause => _gamePause;

    /// <summary>
    /// 일시정지
    /// </summary>
    public void PauseGame()
    {
        _gamePause = true;

        Time.timeScale = 0;
    }

    /// <summary>
    /// 재개
    /// </summary>
    public void PlayGame()
    {
        _gamePause = false;

        Time.timeScale = 1;
    }
}
