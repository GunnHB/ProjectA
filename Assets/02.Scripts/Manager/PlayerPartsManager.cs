using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartsManager : SingletonObject<PlayerPartsManager>
{
    private Dictionary<GameValue.PartsKey, PartsData> _generalDataDic;
    public Dictionary<GameValue.PartsKey, PartsData> GeneralDataDic => _generalDataDic;

    private Dictionary<GameValue.PartsKey, PartsData> _commonDataDic;
    public Dictionary<GameValue.PartsKey, PartsData> CommonDataDic => _commonDataDic;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetPlayerDataDic(Dictionary<GameValue.GenderType, Dictionary<GameValue.PartsKey, PartsData>> dataDic,
                                GameValue.GenderType genderType)
    {
        _generalDataDic = dataDic[genderType];
        _commonDataDic = dataDic[GameValue.GenderType.COMMON];

        if (GameManager.Instance.PlayerObj != null)
        {
            if (GameManager.Instance.PlayerObj.TryGetComponent(out PlayerCustomizer customizer))
                customizer.SwitchAllParts(genderType);
        }

        if (GameManager.Instance.RenderPlayerObj != null)
        {
            if (GameManager.Instance.RenderPlayerObj.TryGetComponent(out PlayerCustomizer customizer))
                customizer.SwitchAllParts(genderType);
        }
    }
}
