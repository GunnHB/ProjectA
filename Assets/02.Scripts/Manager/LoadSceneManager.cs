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

    private Coroutine _corFadeIn;
    private Coroutine _corFadeOut;
    private Coroutine _corLoadScene;

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
        if (_corFadeIn != null)
        {
            StopCoroutine(_corFadeIn);
            _corFadeIn = null;
        }

        _corFadeIn = StartCoroutine(Cor_FadeIn(action));
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

        if (_corFadeOut != null)
        {
            StopCoroutine(_corFadeOut);
            _corFadeOut = null;
        }

        _corFadeOut = StartCoroutine(nameof(Cor_FadeOut));
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

    public void LoadScene(SceneType sceneType)
    {
        if (_corLoadScene != null)
        {

        }
    }
}
