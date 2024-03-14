using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using Sirenix.OdinInspector;

public class UIPanelBase : UIBase
{
    private const string GROUP_OPTION_PANEL = "Panel option";

    protected Sequence _startSequence;
    protected Sequence _destorySequence;

    [BoxGroup(GROUP_OPTION_PANEL), SerializeField]
    protected bool _doTween = true;

    public override void Init()
    {
        transform.SetParent(UIManager.Instance.PanelCanvas.transform);

        if (_doTween)
            UIManager.Instance.PanelCanvas.GetComponent<CanvasGroup>().alpha = 0f;

        base.Init();

        _uiType = UIType.PANEL;
    }

    private void Start()
    {
        if (_doTween)
        {
            _startSequence = DOTween.Sequence()
                                    .Append(StartSeq());
        }
    }

    private Sequence StartSeq()
    {
        RectTransform rect = (RectTransform)transform;

        return DOTween.Sequence()
                    .OnStart(() => { rect.localScale = new Vector3(1.1f, 1.1f, 1.1f); })
                    .Append(rect.DOScale(1f, .1f).SetEase(Ease.InQuad))
                    .Join(UIManager.Instance.PanelCanvas.GetComponent<CanvasGroup>().DOFade(1f, .1f));
    }

    public override void Close()
    {
        if (_doTween)
        {
            _destorySequence = DOTween.Sequence()
                                    .Append(DestroySeq());
        }
        else
            base.Close();
    }

    private Sequence DestroySeq()
    {
        RectTransform rect = (RectTransform)transform;

        return DOTween.Sequence()
                    .Append(rect.DOScale(1.1f, .1f).SetEase(Ease.OutQuad))
                    .Join(UIManager.Instance.PanelCanvas.GetComponent<CanvasGroup>().DOFade(1f, .1f))
                    .OnComplete(() =>
                    {
                        UIManager.Instance.PanelCanvas.GetComponent<CanvasGroup>().alpha = 1f;
                        base.Close();
                    });
    }
}
