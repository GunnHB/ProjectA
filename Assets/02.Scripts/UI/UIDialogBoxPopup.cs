using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;

public class UIDialogBoxPopup : UIPopupBase
{
    [SerializeField]
    private Transform _dialogBoxTr;
    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private TextMeshProUGUI _descText;
    [SerializeField]
    private UIButton _confirmButton;
    [SerializeField]
    private UIButton _cancelButton;

    private Sequence _openSeq;
    private Sequence _closeSeq;

    public override void Init()
    {
        base.Init();

        _openSeq = OpenSequence();
    }

    public void Open(string title, string desc,
                    string confirm, UnityAction confirmAction,
                    string cancel, UnityAction cancelAction)
    {
        _titleText.text = title;
        _descText.text = desc;

        _confirmButton.ButtonText.text = confirm == string.Empty ? "CONFIRM" : confirm;
        _cancelButton.ButtonText.text = cancel == string.Empty ? "CANCEL" : cancel;

        _confirmButton.AddListener(() =>
        {
            _closeSeq = CloseSequence(confirmAction);
        });
        _cancelButton.AddListener(() =>
        {
            _closeSeq = CloseSequence(cancelAction);
        });
    }

    private Sequence OpenSequence()
    {
        return DOTween.Sequence()
                    .OnStart(() =>
                    {
                        _dialogBoxTr.GetComponent<CanvasGroup>().alpha = 0f;
                        _dialogBoxTr.localScale = Vector3.zero;

                        ActiveButton(false);
                    })
                    .Append(_dialogBoxTr.DOScale(1f, .3f).SetEase(Ease.OutBounce))
                    .Join(_dialogBoxTr.GetComponent<CanvasGroup>().DOFade(1f, .3f))
                    .OnComplete(() =>
                    {
                        ActiveButton(true);
                    });
    }

    private Sequence CloseSequence(UnityAction callback)
    {
        return DOTween.Sequence()
                    .OnStart(() =>
                    {
                        _dialogBoxTr.GetComponent<CanvasGroup>().alpha = 1f;
                        _dialogBoxTr.localScale = Vector3.one;

                        ActiveButton(false);
                    })
                    .Append(_dialogBoxTr.DOScale(0f, .3f).SetEase(Ease.InBounce))
                    .Join(_dialogBoxTr.GetComponent<CanvasGroup>().DOFade(0f, .3f))
                    .OnComplete(() =>
                    {
                        UIManager.Instance.CloseUI(this, callback);
                    });
    }

    private void ActiveButton(bool active)
    {
        _cancelButton.interactable = active;
        _confirmButton.interactable = active;
    }
}
