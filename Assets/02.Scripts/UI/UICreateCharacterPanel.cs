using Sirenix.OdinInspector;
using TMPro;

using UnityEngine;

public class UICreateCharacterPanel : UIPanelBase
{
    private const string GROUP_SELECT_GENDER = "Select Gender";
    private const string GROUP_CUSTOMIZE = "Customize";
    private const string GROUP_COMMON = "Common";

    private const string CAMERA_MALE = "MaleCamera";
    private const string CAMERA_FEMALE = "FemaleCamera";

    [BoxGroup(GROUP_SELECT_GENDER), SerializeField]
    private PlayerSlot _maleSlot;
    [BoxGroup(GROUP_SELECT_GENDER), SerializeField]
    private PlayerSlot _femaleSlot;

    [BoxGroup(GROUP_COMMON), SerializeField]
    private TextMeshProUGUI _subtitleText;
    [BoxGroup(GROUP_COMMON), SerializeField]
    private UIButton _backButton;
    [BoxGroup(GROUP_COMMON), SerializeField]
    private UIButton _nextButton;

    // 현재 성별 선택 창인지
    private bool _isGenderSelect = true;

    private Camera _maleCam;
    private Camera _femaleCam;

    private PlayerSlot _selectedSlot;
    public PlayerSlot SelectedSlot => _selectedSlot;

    public override void Init()
    {
        base.Init();

        _maleSlot.Init(this, GameValue.GenderType.Male);
        _femaleSlot.Init(this, GameValue.GenderType.Female);

        InitBackAndNext();
        InitRenderTexture();
    }

    private void InitRenderTexture()
    {
        if (_maleCam == null)
            _maleCam = GetCamera(CAMERA_MALE);

        if (_femaleCam == null)
            _femaleCam = GetCamera(CAMERA_FEMALE);

        if (_maleCam == null || _femaleCam == null)
            return;

        if (_maleCam.targetTexture != null && _femaleCam.targetTexture != null)
        {
            _maleSlot.SetTexture(_maleCam.targetTexture);
            _femaleSlot.SetTexture(_femaleCam.targetTexture);

            return;
        }

        var maleRenderTexture = new RenderTexture(256, 256, 24);
        var femaleRenderTextrue = new RenderTexture(256, 256, 24);

        _maleCam.targetTexture = maleRenderTexture;
        _femaleCam.targetTexture = femaleRenderTextrue;

        _maleSlot.SetTexture(maleRenderTexture);
        _femaleSlot.SetTexture(femaleRenderTextrue);
    }

    private Camera GetCamera(string camName)
    {
        var camObj = GameObject.Find(camName);

        if (camObj != null)
            return camObj.GetComponent<Camera>();

        return null;
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

    public void SwitchSelectSlot(PlayerSlot slot)
    {
        if (_selectedSlot != null)
            _selectedSlot.SetSelect(false);

        _selectedSlot = slot;

        if (_selectedSlot != null)
        {
            _selectedSlot.SetSelect(true);
            _nextButton.gameObject.SetActive(true);
        }
        else
            _nextButton.gameObject.SetActive(false);
    }

    private void OnClickBack()
    {
        // if (!_isGenderSelect)
        // {
        //     _isGenderSelect = true;
        // }
        LoadSceneManager.Instance.DoFade(() =>
        {
            UIManager.Instance.CloseUI(this);
        });
    }

    private void OnClickNext()
    {
        // if (_isGenderSelect)
        // {
        //     _isGenderSelect = false;
        // }
    }
}
