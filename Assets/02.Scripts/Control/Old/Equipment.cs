using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private bool _doAction = false;
    public bool DoAction => _doAction;

    // 무기 든 상태인지
    private bool _isDraw = false;
    public bool IsDraw => _isDraw;

    // 애니메이션 이벤트
    public void DrawWeapon()
    {
        _isDraw = true;
        ItemManager.Instance.DrawWeapon();
    }

    // 애니메이션 이벤트
    public void SheathWeapon()
    {
        _isDraw = false;
        ItemManager.Instance.SheathWeapon();
    }

    public void StartAction()
    {
        _doAction = true;
    }

    public void EndAction()
    {
        _doAction = false;
    }
}
