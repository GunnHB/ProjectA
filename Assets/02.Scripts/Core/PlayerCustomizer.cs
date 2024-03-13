using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class SkinnedMeshRendererInfo
{
    public Mesh _mesh;
    public Transform[] _bones;
    public Transform _rootBones;
}

public class PlayerCustomizer : MonoBehaviour
{
    private const string GROUP_HEAD = "Head";

    [TitleGroup(GROUP_HEAD), SerializeField]
    private Transform _rootHead;
    [TitleGroup(GROUP_HEAD), SerializeField]
    private Transform _rootHair;
    [TitleGroup(GROUP_HEAD), SerializeField]
    private Transform _rootEyebrows;
    [TitleGroup(GROUP_HEAD), SerializeField]
    private SkinnedMeshRenderer _targetHead;
    [TitleGroup(GROUP_HEAD), SerializeField]
    private SkinnedMeshRenderer _targetHair;
    [TitleGroup(GROUP_HEAD), SerializeField]
    private SkinnedMeshRenderer _targetEyebrows;

    private List<SkinnedMeshRendererInfo> _headInfoList = new();
    private List<SkinnedMeshRendererInfo> _hairInfoList = new();
    private List<SkinnedMeshRendererInfo> _eyebrowsInfoList = new();

    public List<SkinnedMeshRendererInfo> HeadInfoList => _headInfoList;
    public List<SkinnedMeshRendererInfo> HairInfoList => _hairInfoList;
    public List<SkinnedMeshRendererInfo> EyebrowsInfoList => _eyebrowsInfoList;

    public SkinnedMeshRenderer TargetHead => _targetHead;
    public SkinnedMeshRenderer TargetHair => _targetHair;
    public SkinnedMeshRenderer TargetEyebrows => _targetHair;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SetSkinnedMeshRendererInfoList(_rootHead, ref _headInfoList, _targetHead);
        SetSkinnedMeshRendererInfoList(_rootHair, ref _hairInfoList, _targetHair);
        SetSkinnedMeshRendererInfoList(_rootEyebrows, ref _eyebrowsInfoList, _targetEyebrows);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRandom();
        }
    }

    private void SetSkinnedMeshRendererInfoList(Transform _rootTransform, ref List<SkinnedMeshRendererInfo> infoList, SkinnedMeshRenderer target)
    {
        for (int index = 0; index < _rootTransform.childCount; index++)
        {
            var item = _rootTransform.GetChild(index);

            if (item.TryGetComponent(out SkinnedMeshRenderer skinn))
            {
                var tempInfo = new SkinnedMeshRendererInfo()
                {
                    _mesh = skinn.sharedMesh,
                    _bones = skinn.bones,
                    _rootBones = skinn.rootBone,
                };

                infoList.Add(tempInfo);
            }
        }

        SetSkinnedMeshRenderer(target, infoList[0]);
    }

    private void SetRandom()
    {
        GetRandomValue(_targetHead, _headInfoList);
        GetRandomValue(_targetHair, _hairInfoList);
        GetRandomValue(_targetEyebrows, _eyebrowsInfoList);
    }

    private void GetRandomValue(SkinnedMeshRenderer target, List<SkinnedMeshRendererInfo> infoList)
    {
        if (target != null && infoList.Count != 0)
        {
            var random = UnityEngine.Random.Range(0, infoList.Count);

            SetSkinnedMeshRenderer(target, infoList[random]);
        }
    }

    private void SetSkinnedMeshRenderer(SkinnedMeshRenderer target, SkinnedMeshRendererInfo info)
    {
        target.sharedMesh = info._mesh;
        target.bones = info._bones;
        target.rootBone = info._rootBones;
    }
}
