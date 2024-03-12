using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    [SerializeField]
    private Collider _collider;

    private void Start()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();

        DisableCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != (int)GameManager.Instance.PlayerMask)
        {
            Debug.Log(other.name);
        }
    }

    public void EnableCollider()
    {
        if (_collider != null)
            _collider.enabled = true;
    }

    public void DisableCollider()
    {
        if (_collider != null)
            _collider.enabled = false;
    }
}
