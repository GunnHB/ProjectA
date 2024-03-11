using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    private Collider _collider;
    public Collider ThisCollider => _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();

        if (_collider != null)
            _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
