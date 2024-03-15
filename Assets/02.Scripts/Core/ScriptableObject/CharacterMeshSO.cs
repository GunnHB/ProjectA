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
    private Dictionary<GameValue.PartsKey, Mesh[]> _maleMeshDic;
    public Dictionary<GameValue.PartsKey, Mesh[]> MaleMeshDic
    {
        get
        {
            if (_maleMeshDic == null)
            {
                _maleMeshDic = new Dictionary<GameValue.PartsKey, Mesh[]>()
                {
                    {GameValue.PartsKey.Face, _maleHeadArray},
                    {GameValue.PartsKey.Eyebrows, _maleEyebrowsArray},
                    {GameValue.PartsKey.FacialHair, _maleFacialHairArray},
                    {GameValue.PartsKey.Torso, _maleTorsoArray},
                    {GameValue.PartsKey.UpperArmRight, _maleUpperArmRightArray},
                    {GameValue.PartsKey.UpperArmLeft, _maleUpperArmLeftArray},
                    {GameValue.PartsKey.LowerArmRight, _maleLowerArmRightArray},
                    {GameValue.PartsKey.LowerArmLeft, _maleLowerArmLeftArray},
                    {GameValue.PartsKey.HandRight, _maleHandRightArray},
                    {GameValue.PartsKey.HandLeft, _maleHandLeftArray},
                    {GameValue.PartsKey.Hip, _maleHipArray},
                    {GameValue.PartsKey.LegRight, _maleLegRightArray},
                    {GameValue.PartsKey.LegLeft, _maleLegLeftArray},
                };
            }

            return _maleMeshDic;
        }
    }

    private Dictionary<GameValue.PartsKey, Mesh[]> _femaleMeshDic;
    public Dictionary<GameValue.PartsKey, Mesh[]> FemaleMeshDic
    {
        get
        {
            if (_femaleMeshDic == null)
            {
                _femaleMeshDic = new Dictionary<GameValue.PartsKey, Mesh[]>()
                {
                    {GameValue.PartsKey.Face, _femaleHeadArray},
                    {GameValue.PartsKey.Eyebrows, _femaleEyebrowsArray},
                    {GameValue.PartsKey.Torso, _femaleTorsoArray},
                    {GameValue.PartsKey.UpperArmRight, _femaleUpperArmRightArray},
                    {GameValue.PartsKey.UpperArmLeft, _femaleUpperArmLeftArray},
                    {GameValue.PartsKey.LowerArmRight, _femaleLowerArmRightArray},
                    {GameValue.PartsKey.LowerArmLeft, _femaleLowerArmLeftArray},
                    {GameValue.PartsKey.HandRight, _femaleHandRightArray},
                    {GameValue.PartsKey.HandLeft, _femaleHandLeftArray},
                    {GameValue.PartsKey.Hip, _femaleHipArray},
                    {GameValue.PartsKey.LegRight, _femaleLegRightArray},
                    {GameValue.PartsKey.LegLeft, _femaleLegLeftArray},
                };
            }

            return _femaleMeshDic;
        }
    }
}
