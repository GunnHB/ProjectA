using System;
using System.Collections.Generic;
using System.Linq;

public class EquipmentData
{
    public InventoryItemData _itemWeaponData;
    public InventoryItemData _itemShieldData;

    public EquipmentData()
    {
        _itemWeaponData = new InventoryItemData();
        _itemShieldData = new InventoryItemData();
    }

    public void EquipWeapon(ItemSlot newItemSlot)
    {
        switch (newItemSlot.InvenItemData._itemData.type)
        {
            case GameValue.ItemType.Weapon:
                RefreshEquipment(newItemSlot, ref _itemWeaponData);
                break;
            case GameValue.ItemType.Shield:
                RefreshEquipment(newItemSlot, ref _itemShieldData);
                break;
        }
    }

    public void RemoveWeapon(InventoryItemData itemData)
    {
        switch (itemData._itemData.type)
        {
            case GameValue.ItemType.Weapon:
                RemoveEquipment(ref _itemWeaponData);
                break;
            case GameValue.ItemType.Shield:
                RemoveEquipment(ref _itemShieldData);
                break;
        }
    }

    private void RefreshEquipment(ItemSlot newItemSlot, ref InventoryItemData prevItemData)
    {
        RemoveEquipment(ref prevItemData);                  // 해제
        SetEquipment(newItemSlot, ref prevItemData);        // 장착
    }

    private void RemoveEquipment(ref InventoryItemData prevItemData)
    {
        if (prevItemData == null || prevItemData._itemData.id == 0)
            return;

        prevItemData._isEquip = false;

        // 오브젝트 비활성화

        // 슬롯 갱신
        ItemManager.Instance.RefreshSlot(prevItemData);
    }

    private void SetEquipment(ItemSlot newItemSlot, ref InventoryItemData prevItemSlot)
    {
        if (newItemSlot == null || newItemSlot.InvenItemData._itemData.id == 0)
            return;

        newItemSlot.InvenItemData._isEquip = true;
        prevItemSlot = newItemSlot.InvenItemData;

        // 오브젝트 활성화

        // 슬롯 갱신
        ItemManager.Instance.RefreshSlot(newItemSlot);
    }
}
