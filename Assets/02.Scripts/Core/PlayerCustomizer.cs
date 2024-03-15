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

    // private const string GROUP_HEAD = "Head";

    // [TitleGroup(GROUP_HEAD), SerializeField]
    // private Transform _rootHead;
    // [TitleGroup(GROUP_HEAD), SerializeField]
    // private Transform _rootHair;
    // [TitleGroup(GROUP_HEAD), SerializeField]
    // private Transform _rootEyebrows;
    // [TitleGroup(GROUP_HEAD), SerializeField]
    // private SkinnedMeshRenderer _targetHead;
    // [TitleGroup(GROUP_HEAD), SerializeField]
    // private SkinnedMeshRenderer _targetHair;
    // [TitleGroup(GROUP_HEAD), SerializeField]
    // private SkinnedMeshRenderer _targetEyebrows;

    // private List<SkinnedMeshRendererInfo> _headInfoList = new();
    // private List<SkinnedMeshRendererInfo> _hairInfoList = new();
    // private List<SkinnedMeshRendererInfo> _eyebrowsInfoList = new();

    // public List<SkinnedMeshRendererInfo> HeadInfoList => _headInfoList;
    // public List<SkinnedMeshRendererInfo> HairInfoList => _hairInfoList;
    // public List<SkinnedMeshRendererInfo> EyebrowsInfoList => _eyebrowsInfoList;

    // public SkinnedMeshRenderer TargetHead => _targetHead;
    // public SkinnedMeshRenderer TargetHair => _targetHair;
    // public SkinnedMeshRenderer TargetEyebrows => _targetHair;

    // private void Awake()
    // {
    //     Init();
    // }

    // private void Init()
    // {
    //     SetSkinnedMeshRendererInfoList(_rootHead, ref _headInfoList, _targetHead);
    //     SetSkinnedMeshRendererInfoList(_rootHair, ref _hairInfoList, _targetHair);
    //     SetSkinnedMeshRendererInfoList(_rootEyebrows, ref _eyebrowsInfoList, _targetEyebrows);
    // }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         SetRandom();
    //     }
    // }

    // private void SetSkinnedMeshRendererInfoList(Transform _rootTransform, ref List<SkinnedMeshRendererInfo> infoList, SkinnedMeshRenderer target)
    // {
    //     for (int index = 0; index < _rootTransform.childCount; index++)
    //     {
    //         var item = _rootTransform.GetChild(index);

    //         if (item.TryGetComponent(out SkinnedMeshRenderer skinn))
    //         {
    //             var tempInfo = new SkinnedMeshRendererInfo()
    //             {
    //                 _mesh = skinn.sharedMesh,
    //                 _bones = skinn.bones,
    //                 _rootBones = skinn.rootBone,
    //             };

    //             infoList.Add(tempInfo);
    //         }
    //     }

    //     SetSkinnedMeshRenderer(target, infoList[0]);
    // }

    // private void SetRandom()
    // {
    //     GetRandomValue(_targetHead, _headInfoList);
    //     GetRandomValue(_targetHair, _hairInfoList);
    //     GetRandomValue(_targetEyebrows, _eyebrowsInfoList);
    // }

    // private void GetRandomValue(SkinnedMeshRenderer target, List<SkinnedMeshRendererInfo> infoList)
    // {
    //     if (target != null && infoList.Count != 0)
    //     {
    //         var random = UnityEngine.Random.Range(0, infoList.Count);

    //         SetSkinnedMeshRenderer(target, infoList[random]);
    //     }
    // }

    // private void SetSkinnedMeshRenderer(SkinnedMeshRenderer target, SkinnedMeshRendererInfo info)
    // {
    //     target.sharedMesh = info._mesh;
    //     target.bones = info._bones;
    //     target.rootBone = info._rootBones;
    // }
}
