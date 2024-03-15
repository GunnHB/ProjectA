using Sirenix.OdinInspector;

using TMPro;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UICreateCharacterPanel : UIPanelBase
{
    private const string GROUP_SELECT_GENDER = "Select Gender";
    private const string GROUP_CUSTOMIZE = "Customize";
    private const string GROUP_COMMON = "Common";

    private const string CAMERA_MALE = "MaleCamera";
    private const string CAMERA_FEMALE = "FemaleCamera";
    private const string CAMERA_CUSTOMIZE = "CustomizeCamera";

    private const string OBJECT_CUSTOMIZE_PLAYER = "CustomizePlayer";

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
    private Camera _customizeCam;

    private PlayerSlot _selectedSlot;
    public PlayerSlot SelectedSlot => _selectedSlot;

    private Sequence _panelSequence;
    private Sequence _partsGroupSeq;

    // 이거 말고 더 좋은 방법이 있을건디...
    private Vector3 _partsGroupOriginLocalPos;
    private Vector3 _customizeRawImageOriginLocalPos;

    private PlayerCustomizer _customizer;

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

        if (_customizeCam == null)
            _customizeCam = GetCamera(CAMERA_CUSTOMIZE);

        if (_maleCam == null || _femaleCam == null || _customizeCam == null)
            return;

        if (_maleCam.targetTexture != null && _femaleCam.targetTexture != null && _customizeCam.targetTexture != null)
        {
            _maleSlot.SetTexture(_maleCam.targetTexture);
            _femaleSlot.SetTexture(_femaleCam.targetTexture);

            _customizeRawImage.texture = _customizeCam.targetTexture;

            return;
        }

        var maleRenderTexture = new RenderTexture(256, 256, 24);
        var femaleRenderTextrue = new RenderTexture(256, 256, 24);
        var customizeRenderTexture = new RenderTexture(256, 256, 24);

        _maleCam.targetTexture = maleRenderTexture;
        _femaleCam.targetTexture = femaleRenderTextrue;
        _customizeCam.targetTexture = customizeRenderTexture;

        _maleSlot.SetTexture(maleRenderTexture);
        _femaleSlot.SetTexture(femaleRenderTextrue);

        _customizeRawImage.texture = customizeRenderTexture;
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
        _partsGroupOriginLocalPos = _partsGroup.transform.localPosition;
        _customizeRawImageOriginLocalPos = _customizeRawImage.transform.localPosition;

        if (_customizer == null)
        {
            var playerObj = GameObject.Find(OBJECT_CUSTOMIZE_PLAYER);

            if (playerObj == null)
            {
                Debug.Log("~~~ no player :( ~~~)");
                return;
            }

            if (playerObj.TryGetComponent(out PlayerCustomizer customizer))
                _customizer = customizer;
        }
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
            // 알림창 하나 띄우기
        }
    }

    private Sequence GoToCustomize()
    {
        return DOTween.Sequence()
                    .OnStart(() =>
                    {
                        ButtonInteratable(false);
                    })
                    .AppendCallback(() =>
                    {
                        _maleSlot.StartSequence();
                        _femaleSlot.StartSequence();
                    })
                    .InsertCallback(.5f, () =>
                    {
                        _genderSelectGroup.SetActive(false);
                        _customizeGroup.SetActive(true);
                    })  // .5초 후에 콜백 실행
                    .Append(CustomizeGroupSeq())
                    .OnComplete(() =>
                    {
                        ButtonInteratable(true);
                    });
    }

    private Sequence CustomizeGroupSeq()
    {
        _partsGroupSeq = null;

        _partsGroupSeq = DOTween.Sequence()
                                .OnStart(() =>
                                {
                                    _partsGroup.transform.DOLocalMoveX(_partsGroupOriginLocalPos.x - 50f, 0f);
                                    _partsGroup.GetComponent<CanvasGroup>().alpha = 0f;

                                    _customizeRawImage.color = new Color(1f, 1f, 1f, 0f);
                                    _customizeRawImage.transform.DOLocalMoveX(_customizeRawImageOriginLocalPos.x + 50f, 0f);
                                })
                                .AppendCallback(() =>
                                {
                                    _partsGroup.transform.DOLocalMoveX(_partsGroupOriginLocalPos.x + 50f, .3f);
                                    _partsGroup.GetComponent<CanvasGroup>().DOFade(1f, .3f);

                                    _customizeRawImage.transform.DOLocalMoveX(_customizeRawImageOriginLocalPos.x - 50f, .3f);
                                    _customizeRawImage.DOFade(1f, .3f);
                                });

        return _partsGroupSeq;
    }

    private Sequence GoToGenderSelect()
    {
        return DOTween.Sequence()
                    .OnStart(() =>
                    {
                        ButtonInteratable(false);
                    })
                    .AppendCallback(() =>
                    {
                        _partsGroup.transform.DOLocalMoveX(_partsGroupOriginLocalPos.x - 50f, .3f);
                        _partsGroup.GetComponent<CanvasGroup>().DOFade(0f, .3f);

                        _customizeRawImage.transform.DOLocalMoveX(_customizeRawImageOriginLocalPos.x + 50f, .3f);
                        _customizeRawImage.DOFade(0f, .3f);
                    })
                    .InsertCallback(.3f, () =>
                    {
                        _customizeGroup.SetActive(false);
                        _genderSelectGroup.SetActive(true);
                    })
                    .InsertCallback(.5f, () =>
                    {
                        _maleSlot.SlotSeq.PlayBackwards();
                        _femaleSlot.SlotSeq.PlayBackwards();
                    })
                    .OnComplete(() =>
                    {
                        ButtonInteratable(true);
                    });
    }

    private void ButtonInteratable(bool active)
    {
        _backButton.interactable = active;
        _nextButton.interactable = active;

        _maleSlot.SetBtnInteractable(active);
        _femaleSlot.SetBtnInteractable(active);
    }
}
