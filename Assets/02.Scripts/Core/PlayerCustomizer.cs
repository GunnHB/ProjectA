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
    public List<SkinnedMeshRendererInfo> _skinnedInfoList = new();

    public void SwtichParts(int index)
    {
        _targetSkinned.sharedMesh = _skinnedInfoList[index]._mesh;
        _targetSkinned.bones = _skinnedInfoList[index]._bones;
        _targetSkinned.rootBone = _skinnedInfoList[index]._rootBones;
    }
}

public class PlayerCustomizer : SerializedMonoBehaviour
{
    [SerializeField, InlineButton("@UpdateSkinnedInfoList(_maleDataDic)", label: "update")]
    private Dictionary<GameValue.PartsKey, PartsData> _maleDataDic = new();
    [SerializeField, InlineButton("@UpdateSkinnedInfoList(_femaleDataDic)", label: "update")]
    private Dictionary<GameValue.PartsKey, PartsData> _femaleDataDic = new();
    [SerializeField, InlineButton("@UpdateSkinnedInfoList(_commonDataDic)", label: "update")]
    private Dictionary<GameValue.PartsKey, PartsData> _commonDataDic;

    public Dictionary<GameValue.PartsKey, PartsData> MaleDataDic => _maleDataDic;
    public Dictionary<GameValue.PartsKey, PartsData> FemaleDataDic => _femaleDataDic;
    public Dictionary<GameValue.PartsKey, PartsData> CommonDataDic => _commonDataDic;

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
}
