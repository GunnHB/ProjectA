using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

    private PlayerCustomizer _customizer = null;
    private Dictionary<GameValue.PartsKey, PartsData> _cacheDic;

    private int _currIndex;
    private int _maxValue;

    public void InitSlot(PlayerCustomizer customizer, GameValue.GenderType genderType, GameValue.PartsKey key)
    {
        _customizer = customizer;

        _partName.text = key.ToString();

        InitDic(genderType);
        SetIndexText(key);

        // 이전 버튼
        _leftButton.AddListener(() =>
        {
            OnClickButton(key, false);
        });

        // 다음 버튼
        _rightButton.AddListener(() =>
        {
            OnClickButton(key, true);
        });
    }

    private void OnClickButton(GameValue.PartsKey key, bool isNext)
    {
        if (_customizer == null || _cacheDic == null || _cacheDic.Count == 0)
            return;

        if (isNext)
            _currIndex++;
        else
            _currIndex--;

        if (_currIndex < 0)
            _currIndex = _maxValue - 1;
        else if (_currIndex >= _maxValue)
            _currIndex = 0;

        _cacheDic[key].SwtichParts(_currIndex);

        SetIndexText(key);
    }

    private void InitDic(GameValue.GenderType gendertype)
    {
        if (_customizer == null)
            return;

        if (gendertype == GameValue.GenderType.Male)
            _cacheDic = _customizer.MaleDataDic;
        else if (gendertype == GameValue.GenderType.Female)
            _cacheDic = _customizer.FemaleDataDic;
        else
            _cacheDic = _customizer.CommonDataDic;
    }

    private void SetIndexText(GameValue.PartsKey key)
    {
        if (_customizer == null || _cacheDic == null || _cacheDic.Count == 0 || !_cacheDic.ContainsKey(key))
            return;

        _maxValue = _cacheDic[key]._skinnedInfoList.Count;
        _currIndex = _cacheDic[key].GetCurrentInfoIndex();

        _indexText.text = $"{_currIndex + 1} / {_maxValue}";
    }
}
