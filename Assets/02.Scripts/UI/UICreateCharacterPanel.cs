using Sirenix.OdinInspector;
using TMPro;

using UnityEngine;

public class UICreateCharacterPanel : UIPanelBase
{
    private const string GROUP_SELECT_GENDER = "Select Gender";
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

    public override void Init()
    {
        base.Init();
    }

    private void InitRenderTexture()
    {

    }
}
