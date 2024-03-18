using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class SkinnedMeshRendererInfo
{
    public Mesh _mesh;
    public Transform[] _bones;
    public Transform _rootBones;
}

[Serializable]
public class PartsData
{
    public Transform _rootTransform;
    public SkinnedMeshRenderer _targetSkinned;
    [ListDrawerSettings(NumberOfItemsPerPage = 5)]
    public List<SkinnedMeshRendererInfo> _skinnedInfoList = new();

    private SkinnedMeshRendererInfo _currentSkinnInfo;
    public SkinnedMeshRendererInfo CurrentSkinnInfo => _currentSkinnInfo;

    public void SwtichParts(int index)
    {
        if (_skinnedInfoList.Count == 0)
        {
            _targetSkinned.sharedMesh = null;
            _currentSkinnInfo = null;

            return;
        }

        _currentSkinnInfo = _skinnedInfoList[index];

        _targetSkinned.sharedMesh = _skinnedInfoList[index]._mesh;
        _targetSkinned.bones = _skinnedInfoList[index]._bones;
        _targetSkinned.rootBone = _skinnedInfoList[index]._rootBones;
    }

    public int GetCurrentInfoIndex()
    {
        if (_currentSkinnInfo == null)
            return 0;
        else
            return _skinnedInfoList.IndexOf(_currentSkinnInfo);
    }
}

public class PlayerCustomizer : SerializedMonoBehaviour
{
    [SerializeField, InlineButton("@UpdateSkinnedInfoList(_maleDataDic)", label: "update")]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    private Dictionary<GameValue.PartsKey, PartsData> _maleDataDic = new();
    [SerializeField, InlineButton("@UpdateSkinnedInfoList(_femaleDataDic)", label: "update")]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    private Dictionary<GameValue.PartsKey, PartsData> _femaleDataDic = new();
    [SerializeField, InlineButton("@UpdateSkinnedInfoList(_commonDataDic)", label: "update")]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    private Dictionary<GameValue.PartsKey, PartsData> _commonDataDic;

    public Dictionary<GameValue.PartsKey, PartsData> MaleDataDic => _maleDataDic;
    public Dictionary<GameValue.PartsKey, PartsData> FemaleDataDic => _femaleDataDic;
    public Dictionary<GameValue.PartsKey, PartsData> CommonDataDic => _commonDataDic;

    private Dictionary<GameValue.GenderType, Dictionary<GameValue.PartsKey, PartsData>> _partDataDic;
    public Dictionary<GameValue.GenderType, Dictionary<GameValue.PartsKey, PartsData>> PartDataDic
    {
        get
        {
            if (_partDataDic == null)
            {
                _partDataDic = new Dictionary<GameValue.GenderType, Dictionary<GameValue.PartsKey, PartsData>>()
                {
                    {GameValue.GenderType.Male, _maleDataDic},
                    {GameValue.GenderType.Female, _femaleDataDic},
                    {GameValue.GenderType.COMMON, _commonDataDic},
                };
            }

            return _partDataDic;
        }
    }

    private void UpdateSkinnedInfoList(Dictionary<GameValue.PartsKey, PartsData> partsDic)
    {
        if (partsDic == null || partsDic.Count == 0)
            return;

        foreach (var key in partsDic.Keys)
        {
            if (partsDic[key] == null || partsDic[key]._rootTransform == null)
                continue;

            partsDic[key]._skinnedInfoList.Clear();

            for (int index = 0; index < partsDic[key]._rootTransform.childCount; index++)
            {
                var item = partsDic[key]._rootTransform.GetChild(index);

                if (item.TryGetComponent(out SkinnedMeshRenderer renderer))
                {
                    var tempInfo = new SkinnedMeshRendererInfo()
                    {
                        _bones = renderer.bones,
                        _mesh = renderer.sharedMesh,
                        _rootBones = renderer.rootBone,
                    };

                    partsDic[key]._skinnedInfoList.Add(tempInfo);
                }
            }
        }
    }

    /// <summary>
    /// 플레이어를 성별에 따라 기본 값으로 세팅
    /// </summary>
    /// <param name="genderType"></param>
    public void PresetByGender(GameValue.GenderType genderType)
    {
        switch (genderType)
        {
            case GameValue.GenderType.Male:
                {
                    SwtichPartsAllByIndex(_maleDataDic, 0);
                }
                break;
            case GameValue.GenderType.Female:
                {
                    SwtichPartsAllByIndex(_femaleDataDic, 0);
                }
                break;
        }

        _commonDataDic[GameValue.PartsKey.Hair].SwtichParts(0);
    }

    private void SwtichPartsAllByIndex(Dictionary<GameValue.PartsKey, PartsData> dataDic, int index)
    {
        foreach (var item in dataDic.Keys)
            dataDic[item].SwtichParts(index);
    }

    public void SwitchAllParts(GameValue.GenderType genderType)
    {
        if (PlayerPartsManager.Instance.GeneralDataDic == null || PlayerPartsManager.Instance.CommonDataDic == null)
            return;

        foreach (var key in PlayerPartsManager.Instance.GeneralDataDic.Keys)
        {
            int index = PlayerPartsManager.Instance.GeneralDataDic[key].GetCurrentInfoIndex();

            if (genderType == GameValue.GenderType.Male)
                _maleDataDic[key].SwtichParts(index);
            else
                _femaleDataDic[key].SwtichParts(index);
        }

        foreach (var key in PlayerPartsManager.Instance.CommonDataDic.Keys)
        {
            int index = PlayerPartsManager.Instance.CommonDataDic[key].GetCurrentInfoIndex();

            _commonDataDic[key].SwtichParts(index);
        }
    }
}
