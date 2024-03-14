using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonObject<LoadSceneManager>
{
    public enum SceneType
    {
        None = -1,
        StartScene,
        InGameScene,
    }

    private UILoadSceneFade _uiFade;

    private float _prevAlpha = 0f;              // 직전의 알파 값
    private float _fadeTime = 1f;               // 전환 시간

    private Coroutine _fadeIn;
    private Coroutine _fadeOut;

    private bool _isFadeInFin;                  // 페이드 인 끝났는지

    private SceneType _currentScene = SceneType.None;

    public SceneType CurrentScene => _currentScene;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (_uiFade == null)
        {
            _uiFade = UIManager.Instance.OpenUI<UILoadSceneFade>();

            if (_uiFade != null)
                _uiFade.Open(_prevAlpha);
        }
    }

    public void DoFade()
    {
        if (_fadeIn != null)
        {
            StopCoroutine(_fadeIn);
            _fadeIn = null;
        }

        _fadeIn = StartCoroutine(Cor_FadeIn());
    }

    public IEnumerator Cor_FadeIn()
    {
        float _currFadeTime = 0f;

        // 후방의 ui 동작 막기
        _uiFade.SetRayTarget(true);

        while (_currFadeTime < _fadeTime)
        {
            _currFadeTime += Time.deltaTime / _fadeTime;

            _uiFade.SetFade(_currFadeTime);

            yield return null;
        }
    }

    public IEnumerator Cor_FadeOut()
    {
        yield break;
    }
}
