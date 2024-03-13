using Sirenix.OdinInspector;
using TMPro;

using UnityEngine;

public class UICreateCharacterPanel : UIPanelBase
{
    private const string GROUP_SELECT_GENDER = "Select Gender";
    private const string GROUP_CUSTOMIZE = "Customize";
    private const string GROUP_COMMON = "Common";

    [BoxGroup(GROUP_SELECT_GENDER), SerializeField]
    private UIButtonRaw _maleRawButton;
    [BoxGroup(GROUP_SELECT_GENDER), SerializeField]
    private UIButtonRaw _femaleRawButton;

    [BoxGroup(GROUP_COMMON), SerializeField]
    private TextMeshProUGUI _subtitleText;
    [BoxGroup(GROUP_COMMON), SerializeField]
    private UIButton _backButton;
    [BoxGroup(GROUP_COMMON), SerializeField]
    private UIButton _nextButton;

    // 현재 성별 선택 창인지
    private bool _isGenderSelect = true;

    public override void Init()
    {
        base.Init();

        InitBackAndNext();
        InitRenderTexture();
    }

    private void InitRenderTexture()
    {

    }

    private void InitBackAndNext()
    {
        _backButton.onClick.RemoveAllListeners();
        _nextButton.onClick.RemoveAllListeners();

        _backButton.onClick.AddListener(OnClickBack);
        _nextButton.onClick.AddListener(OnClickNext);

        // 처음엔 숨김
        _nextButton.gameObject.SetActive(false);
    }

    private void OnClickBack()
    {
        if (!_isGenderSelect)
        {
            _isGenderSelect = true;
        }
    }

    private void OnClickNext()
    {
        if (_isGenderSelect)
        {
            _isGenderSelect = false;
        }
    }
}
