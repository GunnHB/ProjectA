using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Sirenix.OdinInspector;

using TMPro;

public class UISlideOptionPopup : UIPopupBase
{
    private const string GROUP_TOP = "Top";
    private const string GROUP_MID = "MID";
    private const string GROUP_BOT = "BOT";

    private const string TEXT_CONFIRM = "Confirm";
    private const string TEXT_CANCEL = "Cancel";

    [BoxGroup(GROUP_TOP), SerializeField]
    private TextMeshProUGUI _titleText;

    [BoxGroup(GROUP_MID), SerializeField]
    private TextMeshProUGUI _descText;
    [BoxGroup(GROUP_MID), SerializeField]
    private Slider _slider;
    [BoxGroup(GROUP_MID), SerializeField]
    private TMP_InputField _inputField;

    [BoxGroup(GROUP_BOT), SerializeField]
    private UIButton _confirmButton;
    [BoxGroup(GROUP_BOT), SerializeField]
    private UIButton _cancelButton;

    public override void Init()
    {
        base.Init();

        _confirmButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        _slider.onValueChanged.RemoveAllListeners();
    }

    public void InitUI(string title, string desc, int maxValue,
                        UnityAction confirmCallback = null, UnityAction cancelCallback = null,
                        string confirmText = "", string cancelText = "")
    {
        _titleText.text = title;
        _descText.text = desc;

        _slider.maxValue = maxValue;
        _inputField.text = 1.ToString();

        _slider.onValueChanged.AddListener(SetInputField);

        _confirmButton.ButtonText.text = confirmText == string.Empty ? TEXT_CONFIRM : confirmText;
        _cancelButton.ButtonText.text = cancelText == string.Empty ? TEXT_CANCEL : cancelText;

        _confirmButton.onClick.AddListener(() =>
        {
            confirmCallback?.Invoke();
            UIManager.Instance.CloseUI(this);
        });
        _cancelButton.onClick.AddListener(() =>
        {
            cancelCallback?.Invoke();
            UIManager.Instance.CloseUI(this);
        });
    }

    private void SetInputField(float sliderValue)
    {
        _inputField.text = sliderValue.ToString();
    }
}
