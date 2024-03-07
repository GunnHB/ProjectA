using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    public int _attackAnimHash;
}

public class Attack : MonoBehaviour
{
    private Dictionary<GameValue.AttakcType, List<AttackData>> _attackDic;
    public Dictionary<GameValue.AttakcType, List<AttackData>> AttackDic => _attackDic;

    public void RegistData(GameValue.AttakcType attackType, List<AttackData> attackDataList)
    {
        if (!_attackDic.ContainsKey(attackType))
            _attackDic.Add(attackType, attackDataList);
        else
            _attackDic[attackType].AddRange(attackDataList);
    }
}
