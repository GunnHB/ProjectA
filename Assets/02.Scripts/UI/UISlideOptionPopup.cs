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

    [BoxGroup(GROUP_TOP), SerializeField]
    private TextMeshProUGUI _titleText;

    [BoxGroup(GROUP_MID), SerializeField]
    private TextMeshProUGUI _descText;
    [BoxGroup(GROUP_MID), SerializeField]
    private Slider _slider;
    [BoxGroup(GROUP_MID), SerializeField]
    private InputField _inputField;

    [BoxGroup(GROUP_BOT), SerializeField]
    private UIButton _confirmButton;
    [BoxGroup(GROUP_BOT), SerializeField]
    private UIButton _cancelButton;

    public override void Init()
    {
        base.Init();

        _confirmButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
    }

    public void InitUI(string title, string desc, int maxValue,
                        UnityAction confirmCallback = null, UnityAction cancelCallback = null)
    {
        _titleText.text = title;
        _descText.text = desc;

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
}
