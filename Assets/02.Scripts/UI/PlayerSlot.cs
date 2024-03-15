using System.Collections;
using System.Collections.Generic;

using TMPro;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    [SerializeField]
    private UIButton _button;
    [SerializeField]
    private Image _frame;
    [SerializeField]
    private RawImage _rawImage;
    [SerializeField]
    private TextMeshProUGUI _genderText;

    private UICreateCharacterPanel _createPanel;
    private GameValue.GenderType _genderType;

    private Sequence _slotSeq;
    public Sequence SlotSeq => _slotSeq;

    private void Awake()
    {
        _button.onClick.RemoveAllListeners();

        _button.SetEnterAndExit(EnterButton, ExitButton);
        _button.onClick.AddListener(OnClickButton);
    }

    public void Init(UICreateCharacterPanel createPanel, GameValue.GenderType gender, Sequence seq)
    {
        _createPanel = createPanel;
        _genderType = gender;

        _genderText.text = _genderType.ToString();

        _frame.gameObject.SetActive(false);

        _slotSeq = seq;
        _slotSeq.Pause();       // 등록 후 바로 실행되지 않도록
    }

    private void OnClickButton()
    {
        if (_createPanel == null)
            return;

        if (_createPanel.SelectedSlot == this)
            _createPanel.SwitchSelectSlot(null);
        else
            _createPanel.SwitchSelectSlot(this);
    }

    private void EnterButton()
    {
        if (_createPanel.SelectedSlot == this)
            return;

        SetEnterColor();
    }

    private void ExitButton()
    {
        if (_createPanel.SelectedSlot == this)
            return;

        SetExitColor();
    }

    public void SetSelect(bool isSelect)
    {
        if (isSelect)
            SetSelectColor();
        else
            SetExitColor();
    }

    public void SetTexture(RenderTexture renderTexture)
    {
        if (_rawImage != null)
            _rawImage.texture = renderTexture;
    }

    private void SetSelectColor()
    {
        _frame.gameObject.SetActive(true);

        _frame.color = new Color(_frame.color.r, _frame.color.g, _frame.color.b, 1f);
    }

    private void SetEnterColor()
    {
        _frame.gameObject.SetActive(true);

        _frame.color = new Color(_frame.color.r, _frame.color.g, _frame.color.b, .5f);
    }

    private void SetExitColor()
    {
        _frame.color = new Color(_frame.color.r, _frame.color.g, _frame.color.b, 1f);

        _frame.gameObject.SetActive(false);
    }

    public void StartSequence()
    {
        if (_slotSeq != null)
            _slotSeq.Restart();
    }

    public void SetBtnInteractable(bool active)
    {
        _button.interactable = active;
    }
}
