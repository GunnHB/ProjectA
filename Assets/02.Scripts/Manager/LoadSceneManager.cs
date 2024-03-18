using System;
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

    private float _fadeTime = 1f;               // 전환 시간

    private Coroutine _corFadeIn;
    private Coroutine _corFadeOut;
    private Coroutine _corLoadScene;

    private bool _isFadeInFin;                  // 페이드 인 끝났는지

    private SceneType _sceneType = SceneType.None;
    public SceneType ThisSceneType => _sceneType;

    private UnityAction _callback = null;

    private bool _callbackInvoked = false;
    private bool _isDoneChangeScene = true;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (_uiFade == null)
        {
            FindFadeUI();

            if (_uiFade != null)
                _uiFade.Open();
        }
    }

    private void FindFadeUI()
    {
        _uiFade = UIManager.Instance.OpenUI<UILoadSceneFade>();

        _uiFade.SetRayTarget(false);
        _uiFade.SetFade(0f);
    }

    public void DoFade(UnityAction action = null, SceneType type = SceneType.None)
    {
        if (_uiFade == null)
            FindFadeUI();

        _uiFade.SetRayTarget(true);

        ResetCoroutineDatas();

        _callback = action;
        _sceneType = type;

        _corFadeIn = StartCoroutine(nameof(Cor_FadeIn));
        _corFadeOut = StartCoroutine(nameof(Cor_FadeOut));
    }

    private void ResetCoroutineDatas()
    {
        CoroutineStop(_corFadeIn);
        CoroutineStop(_corFadeOut);
    }

    private void CoroutineStop(Coroutine cor)
    {
        if (cor != null)
        {
            StopCoroutine(nameof(cor));
            cor = null;
        }
    }

    public IEnumerator Cor_FadeIn()
    {
        float _currFadeTime = 0f;

        _isFadeInFin = false;

        while (_currFadeTime < _fadeTime)
        {
            _currFadeTime += Time.deltaTime / _fadeTime;
            _uiFade.SetFade(_currFadeTime);

            yield return null;
        }

        _isFadeInFin = true;
    }

    public IEnumerator Cor_FadeOut()
    {
        yield return new WaitUntil(() => _isFadeInFin);
        float _currFadeTime = 1f;

        while (_currFadeTime > 0f)
        {
            if (!_callbackInvoked)
            {
                if (_sceneType != SceneType.None)
                    LoadScene(_sceneType);

                yield return new WaitUntil(() => _isDoneChangeScene);

                _callback?.Invoke();
                _callbackInvoked = true;
            }

            // if (_uiFade == null)
            //     FindFadeUI();

            _currFadeTime -= Time.deltaTime / _fadeTime;
            _uiFade.SetFade(_currFadeTime);

            yield return null;
        }

        ResetData();
    }

    private void ResetData()
    {
        _isFadeInFin = false;
        _callbackInvoked = false;
        _callback = null;
        _sceneType = SceneType.None;
        _isDoneChangeScene = true;

        if (_uiFade != null)
            _uiFade.SetRayTarget(false);
    }

    public void LoadScene(SceneType sceneType)
    {
        if (_corLoadScene != null)
        {
            StopCoroutine(_corLoadScene);
            _corLoadScene = null;
        }

        _corLoadScene = StartCoroutine(nameof(Cor_LoadScene), (int)sceneType);
    }

    private IEnumerator Cor_LoadScene(int sceneIndex)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            _isDoneChangeScene = false;
            yield return null;
        }

        if (_uiFade == null)
            FindFadeUI();

        _isDoneChangeScene = true;
    }
}
