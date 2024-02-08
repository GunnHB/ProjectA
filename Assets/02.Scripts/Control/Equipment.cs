using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private GameObject _handHolder;
    [SerializeField] private GameObject _sheathHolder;
    [SerializeField] private GameObject _weapon;

    private GameObject _currWeaponInHand;
    private GameObject _currWeaponInSheath;

    private void Start()
    {
        if (_sheathHolder != null)
            _currWeaponInSheath = Instantiate(_weapon, _sheathHolder.transform);
    }

    public void DrawWeapon()
    {
        _currWeaponInHand = Instantiate(_weapon, _handHolder.transform);
        Destroy(_currWeaponInSheath);
    }

    public void SheathWeapon()
    {
        _currWeaponInSheath = Instantiate(_weapon, _sheathHolder.transform);
        Destroy(_currWeaponInHand);
    }
}
