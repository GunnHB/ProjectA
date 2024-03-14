using UnityEngine;

using Sirenix.OdinInspector;

public class UIStartPanel : UIPanelBase
{
    private const string GROUP_BUTTONS = "Buttons";

    [BoxGroup(GROUP_BUTTONS), SerializeField]
    private UIButton _newGameButton;
    [BoxGroup(GROUP_BUTTONS), SerializeField]
    private UIButton _loadGameButtno;
    [BoxGroup(GROUP_BUTTONS), SerializeField]
    private UIButton _optionsButtno;
    [BoxGroup(GROUP_BUTTONS), SerializeField]
    private UIButton _quitButtno;

    public override void Init()
    {
        base.Init();

        InitButtons();
    }

    private void InitButtons()
    {
        // 새로 시작하는 게임이라면 저장된 데이터가 없어서 LoadGame 버튼이 나타나지 않음

        _newGameButton.onClick.RemoveAllListeners();
        _loadGameButtno.onClick.RemoveAllListeners();
        _optionsButtno.onClick.RemoveAllListeners();
        _quitButtno.onClick.RemoveAllListeners();

        _newGameButton.onClick.AddListener(OnClickNewGame);
        _loadGameButtno.onClick.AddListener(OnClickLoadGame);
        _optionsButtno.onClick.AddListener(OnClickOptions);
        _quitButtno.onClick.AddListener(OnClickQuit);

        // 개발하고 있는 지금은 일단 무조건 숨김 처리
        _loadGameButtno.gameObject.SetActive(false);
    }

    private void OnClickNewGame()
    {
        LoadSceneManager.Instance.DoFade();
    }

    private void OnClickLoadGame()
    {

    }

    private void OnClickOptions()
    {

    }

    private void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
