using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class UIPanelBase : UIBase
{
    protected Sequence _startSequence;
    protected Sequence _destorySequence;

    public override void Init()
    {
        transform.SetParent(UIManager.Instance.PanelCanvas.transform);
        UIManager.Instance.PanelCanvas.GetComponent<CanvasGroup>().alpha = 0f;

        base.Init();

        _uiType = UIType.PANEL;
    }

    private void Start()
    {
        _startSequence = DOTween.Sequence()
                                .Append(StartSeq());
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
        _destorySequence = DOTween.Sequence()
                                .Append(DestroySeq());
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
