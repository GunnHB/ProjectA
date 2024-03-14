using Sirenix.OdinInspector;

using TMPro;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

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
    [BoxGroup(GROUP_SELECT_GENDER), SerializeField]
    private GameObject _genderSelectGroup;

    [BoxGroup(GROUP_CUSTOMIZE), SerializeField]
    private GameObject _customizeGroup;
    [BoxGroup(GROUP_CUSTOMIZE), SerializeField]
    private GameObject _partsGroup;
    [BoxGroup(GROUP_CUSTOMIZE), SerializeField]
    private ObjectPool _partsSlotPool;
    [BoxGroup(GROUP_CUSTOMIZE), SerializeField]
    private RawImage _customizeRawImage;

    [BoxGroup(GROUP_COMMON), SerializeField]
    private TextMeshProUGUI _subtitleText;
    [BoxGroup(GROUP_COMMON), SerializeField]
    private UIButton _backButton;
    [BoxGroup(GROUP_COMMON), SerializeField]
    private UIButton _nextButton;

    // 성별했는지 (선택 창에서 넘어감)
    private bool _isGenderSelected = false;

    private Camera _maleCam;
    private Camera _femaleCam;

    private PlayerSlot _selectedSlot;
    public PlayerSlot SelectedSlot => _selectedSlot;

    private Sequence _panelSequence;

    private Sequence _partsGroupSeq;

    public override void Init()
    {
        base.Init();

        _maleSlot.Init(this, GameValue.GenderType.Male, PlayerSlotSequence(_maleSlot.transform, 5f));
        _femaleSlot.Init(this, GameValue.GenderType.Female, PlayerSlotSequence(_femaleSlot.transform, -5f));

        InitBackAndNext();

        InitSelectGenderGroup();
        InitCustomizeGroup();
    }

    private void InitSelectGenderGroup()
    {
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

    private void InitCustomizeGroup()
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

    private Sequence PlayerSlotSequence(Transform slotTr, float moveX)
    {
        return DOTween.Sequence()
                    .SetAutoKill(false)                                     // 삭제되면 안됨
                    .Append(slotTr.DOLocalMoveX(moveX, .3f))
                    .Join(slotTr.GetComponent<CanvasGroup>().DOFade(0f, .3f));
    }

    private void OnClickBack()
    {
        if (!_isGenderSelected)
        {
            LoadSceneManager.Instance.DoFade(() =>
            {
                UIManager.Instance.CloseUI(this);
            });
        }
        else
        {
            _isGenderSelected = false;
            _panelSequence = null;

            _panelSequence = DOTween.Sequence().Append(GoToGenderSelect());
        }
    }

    private void OnClickNext()
    {
        _panelSequence = null;

        if (!_isGenderSelected)
        {
            _isGenderSelected = true;

            _panelSequence = DOTween.Sequence().Append(GoToCustomize());
        }
        else
        {
            // 인게임 씬으로 전환
        }
    }

    private Sequence GoToCustomize()
    {
        return DOTween.Sequence()
                    .AppendCallback(() =>
                    {
                        _maleSlot.StartSequence();
                        _femaleSlot.StartSequence();
                    })
                    .InsertCallback(.5f, () =>
                    {
                        _genderSelectGroup.SetActive(false);
                        _customizeGroup.SetActive(true);

                        // 비디오 돌리기
                        _maleSlot.SlotSeq.Rewind();
                        _femaleSlot.SlotSeq.Rewind();
                    })  // .5초 후에 콜백 실행
                    .Append(CustomizeGroupSeq());
    }

    private Sequence CustomizeGroupSeq()
    {
        _partsGroupSeq = DOTween.Sequence()
                                .SetAutoKill(false)
                                .OnStart(() =>
                                {
                                    _partsGroup.GetComponent<CanvasGroup>().alpha = 0f;
                                })
                                .SetDelay(.5f)
                                .Append(_partsGroup.transform.DOLocalMoveX(-100f, .3f).From(true))
                                .Join(_partsGroup.GetComponent<CanvasGroup>().DOFade(1f, 3f).From(true));

        return _partsGroupSeq;
    }

    private Sequence GoToGenderSelect()
    {
        return DOTween.Sequence();
    }
}
