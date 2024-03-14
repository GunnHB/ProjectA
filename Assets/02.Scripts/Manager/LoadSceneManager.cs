using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
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

    public void DoFade(UnityAction action = null)
    {
        if (_fadeIn != null)
        {
            StopCoroutine(_fadeIn);
            _fadeIn = null;
        }

        _fadeIn = StartCoroutine(Cor_FadeIn(action));
    }

    public IEnumerator Cor_FadeIn(UnityAction action)
    {
        float _currFadeTime = 0f;

        // 레이타겟 끄기
        _uiFade.SetRayTarget(true);
        _isFadeInFin = false;

        while (_currFadeTime < _fadeTime)
        {
            _currFadeTime += Time.deltaTime / _fadeTime;

            _uiFade.SetFade(_currFadeTime);

            yield return null;
        }

        _isFadeInFin = true;

        action?.Invoke();

        if (_fadeOut != null)
        {
            StopCoroutine(_fadeOut);
            _fadeOut = null;
        }

        _fadeOut = StartCoroutine(nameof(Cor_FadeOut));
    }

    public IEnumerator Cor_FadeOut()
    {
        float _currFadeTime = 1f;

        while (_currFadeTime > 0)
        {
            if (_isFadeInFin)
            {
                _currFadeTime -= Time.deltaTime / _fadeTime;

                _uiFade.SetFade(_currFadeTime);
            }

            yield return null;
        }

        // 레이타겟 켜주기
        _uiFade.SetRayTarget(false);
        _isFadeInFin = false;
    }
}
