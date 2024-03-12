using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    public int _attackAnimHash;
    public float _transitionDuration;
}

public class Attack : MonoBehaviour
{
    // 무기 타입에 따라 공격 애니가 다르도록 설정
    // 나중에 문제 생기면 AttackType으로 따로 빼자...
    private Dictionary<GameValue.WeaponType, List<AttackData>> _attackDic = new();
    public Dictionary<GameValue.WeaponType, List<AttackData>> AttackDic => _attackDic;

    public void RegistData(GameValue.WeaponType attackType, List<AttackData> attackDataList)
    {
        if (!_attackDic.ContainsKey(attackType))
            _attackDic.Add(attackType, attackDataList);
        else
            _attackDic[attackType].AddRange(attackDataList);
    }

    // animation events
    public void StartCheckHitCollider()
    {
        if (ItemManager.Instance.EquipWeaponData._itemPrefab == null)
            return;

        // 시작되는 동안은 콜라이더 기능 켜기
        if (ItemManager.Instance.EquipWeaponData._itemPrefab.TryGetComponent(out WeaponItem weaponItem))
            weaponItem.EnableCollider();
    }

    // animation events
    public void EndCheckHitCollider()
    {
        if (ItemManager.Instance.EquipWeaponData._itemPrefab == null)
            return;

        // 끝나면 콜라이더 기능 끄기
        if (ItemManager.Instance.EquipWeaponData._itemPrefab.TryGetComponent(out WeaponItem weaponItem))
            weaponItem.DisableCollider();
    }
}
