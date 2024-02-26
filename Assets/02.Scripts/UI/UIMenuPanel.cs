using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;

using TMPro;

using UnityEngine;
using UnityEngine.Events;

public class UIMenuPanel : UIPanelBase
{
    private const string TOP = "Top";
    private const string CONTENT = "Content";
    private const string BOTTOM = "Bottom";

    public UnityAction _inventoryAction;
    public UnityAction _settingAction;

    [BoxGroup(TOP), SerializeField]
    private TextMeshProUGUI _titleText;
    [BoxGroup(TOP), SerializeField]
    private TextMeshProUGUI _goldText;

    [BoxGroup(CONTENT), SerializeField]
    private GameObject _contentParent;
    [BoxGroup(CONTENT), SerializeField]
    private Inventory _inventory;

    public override void Init()
    {
        base.Init();

        _goldText.text = 0.ToString();

        _inventoryAction -= InventoryOpen;
        _settingAction -= SettingOpen;

        _inventoryAction += InventoryOpen;
        _settingAction += SettingOpen;
    }

    private void InventoryOpen()
    {
        _titleText.text = "INVENTORY";

        _inventory = ActiveContent<Inventory>();

        if (_inventory != null)
            _inventory.Init();
    }

    private void SettingOpen()
    {

    }

    private T ActiveContent<T>() where T : MonoBehaviour
    {
        T content = null;

        for (int index = 0; index < _contentParent.transform.childCount; index++)
        {
            var contentTransform = _contentParent.transform.GetChild(index);

            if (contentTransform.TryGetComponent(out T compo))
            {
                compo.gameObject.SetActive(true);
                content = compo;
            }
            else
                contentTransform.gameObject.SetActive(false);
        }

        return content;
    }
}
