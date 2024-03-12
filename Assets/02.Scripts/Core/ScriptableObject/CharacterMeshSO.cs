using Sirenix.OdinInspector;

using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New Character Mesh SO", menuName = "Scriptable Object/New Character Mesh SO", order = int.MaxValue)]
public class CharacterMeshSO : SerializedScriptableObject
{
    private const string GROUP_HEAD = "Head";
    private const string GROUP_TORSO = "Torso";
    private const string GROUP_ARM = "Arm";
    private const string GROUP_HIP = "Hip";
    private const string GROUP_LEG = "Leg";

    public bool _showMale = false;

    [BoxGroup(GROUP_HEAD), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleHeadArray;
    [BoxGroup(GROUP_HEAD), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleHeadArray;
    [BoxGroup(GROUP_HEAD), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleEyebrowsArray;
    [BoxGroup(GROUP_HEAD), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleEyebrowsArray;
    [BoxGroup(GROUP_HEAD), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleFacialHairArray;

    [BoxGroup(GROUP_TORSO), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleTorsoArray;
    [BoxGroup(GROUP_TORSO), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleTorsoArray;

    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleUpperArmRightArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleUpperArmRightArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleUpperArmLeftArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleUpperArmLeftArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleLowerArmRightArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleLowerArmRightArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleLowerArmLeftArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleLowerArmLeftArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleHandRightArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleHandRightArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleHandLeftArray;
    [BoxGroup(GROUP_ARM), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleHandLeftArray;

    [BoxGroup(GROUP_HIP), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleHipArray;
    [BoxGroup(GROUP_HIP), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleHipArray;

    [BoxGroup(GROUP_LEG), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleLegRightArray;
    [BoxGroup(GROUP_LEG), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleLegRightArray;
    [BoxGroup(GROUP_LEG), ListDrawerSettings(NumberOfItemsPerPage = 5), ShowIf(nameof(_showMale)), SerializeField]
    private Mesh[] _maleLegLeftArray;
    [BoxGroup(GROUP_LEG), ListDrawerSettings(NumberOfItemsPerPage = 5), HideIf(nameof(_showMale)), SerializeField]
    private Mesh[] _femaleLegLeftArray;

    // public Mesh[] MaleHeadArray => _maleHeadArray;
    // public Mesh[] FemaleHeadArray => _femaleHeadArray;
    private Dictionary<GameValue.MeshKey, Mesh[]> _maleMeshDic;
    public Dictionary<GameValue.MeshKey, Mesh[]> MaleMeshDic
    {
        get
        {
            if (_maleMeshDic == null)
            {
                _maleMeshDic = new Dictionary<GameValue.MeshKey, Mesh[]>()
                {
                    {GameValue.MeshKey.Head, _maleHeadArray},
                    {GameValue.MeshKey.Eyebrows, _maleEyebrowsArray},
                    {GameValue.MeshKey.FacialHair, _maleFacialHairArray},
                    {GameValue.MeshKey.Torso, _maleTorsoArray},
                    {GameValue.MeshKey.UpperArmRight, _maleUpperArmRightArray},
                    {GameValue.MeshKey.UpperArmLeft, _maleUpperArmLeftArray},
                    {GameValue.MeshKey.LowerArmRight, _maleLowerArmRightArray},
                    {GameValue.MeshKey.LowerArmLeft, _maleLowerArmLeftArray},
                    {GameValue.MeshKey.HandRight, _maleHandRightArray},
                    {GameValue.MeshKey.HandLeft, _maleHandLeftArray},
                    {GameValue.MeshKey.Hip, _maleHipArray},
                    {GameValue.MeshKey.LegRight, _maleLegRightArray},
                    {GameValue.MeshKey.LegLeft, _maleLegLeftArray},
                };
            }

            return _maleMeshDic;
        }
    }

    private Dictionary<GameValue.MeshKey, Mesh[]> _femaleMeshDic;
    public Dictionary<GameValue.MeshKey, Mesh[]> FemaleMeshDic
    {
        get
        {
            if (_femaleMeshDic == null)
            {
                _femaleMeshDic = new Dictionary<GameValue.MeshKey, Mesh[]>()
                {
                    {GameValue.MeshKey.Head, _femaleHeadArray},
                    {GameValue.MeshKey.Eyebrows, _femaleEyebrowsArray},
                    {GameValue.MeshKey.Torso, _femaleTorsoArray},
                    {GameValue.MeshKey.UpperArmRight, _femaleUpperArmRightArray},
                    {GameValue.MeshKey.UpperArmLeft, _femaleUpperArmLeftArray},
                    {GameValue.MeshKey.LowerArmRight, _femaleLowerArmRightArray},
                    {GameValue.MeshKey.LowerArmLeft, _femaleLowerArmLeftArray},
                    {GameValue.MeshKey.HandRight, _femaleHandRightArray},
                    {GameValue.MeshKey.HandLeft, _femaleHandLeftArray},
                    {GameValue.MeshKey.Hip, _femaleHipArray},
                    {GameValue.MeshKey.LegRight, _femaleLegRightArray},
                    {GameValue.MeshKey.LegLeft, _femaleLegLeftArray},
                };
            }

            return _femaleMeshDic;
        }
    }
}
