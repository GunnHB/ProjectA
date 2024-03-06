using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private bool _doAction = false;
    public bool DoAction => _doAction;

    public void DrawWeapon()
    {
        ItemManager.Instance.DrawWeapon();
    }

    public void SheathWeapon()
    {
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

    // [SerializeField] private GameObject _handHolder;
    // [SerializeField] private GameObject _sheathHolder;
    // [SerializeField] private GameObject _weapon;

    // private GameObject _currWeaponInHand;
    // private GameObject _currWeaponInSheath;

    // private bool _doAction = false;

    // public bool DoAction => _doAction;

    // public void DrawWeapon()
    // {
    //     _currWeaponInHand = Instantiate(_weapon, _handHolder.transform);
    //     Destroy(_currWeaponInSheath);
    // }

    // public void SheathWeapon()
    // {
    //     _currWeaponInSheath = Instantiate(_weapon, _sheathHolder.transform);
    //     Destroy(_currWeaponInHand);
    // }

    // public void StartAction()
    // {
    //     _doAction = true;
    // }

    // public void EndAction()
    // {
    //     _doAction = false;
    // }
}
