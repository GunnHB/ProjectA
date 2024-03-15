using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartsSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _partName;
    [SerializeField]
    private TextMeshProUGUI _indexText;
    [SerializeField]
    private UIButton _leftButton;
    [SerializeField]
    private UIButton _rightButton;

    public void InitSlot(string partName)
    {
        _partName.text = partName;

        InitButtons();
    }

    private void InitButtons()
    {
        _leftButton.AddListener(OnClickLeftButton);
        _rightButton.AddListener(OnClickRightButton);
    }

    private void OnClickLeftButton()
    {

    }

    private void OnClickRightButton()
    {

    }
}
