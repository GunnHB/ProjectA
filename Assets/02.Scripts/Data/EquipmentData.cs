using System.Collections.Generic;

public class EquipmentData
{
    public InventoryItemData _itemWeaponData;
    public InventoryItemData _itemShieldData;

    public EquipmentData()
    {
        _itemWeaponData = new InventoryItemData();
        _itemShieldData = new InventoryItemData();
    }

    public void EquipWeapon(InventoryItemData newItemData)
    {
        switch (newItemData._itemData.type)
        {
            case GameValue.ItemType.Weapon:
                SetEquip(newItemData, ref _itemWeaponData);
                break;
            case GameValue.ItemType.Shield:
                SetEquip(newItemData, ref _itemShieldData);
                break;
        }
    }

    private void SetEquip(InventoryItemData newItemData, ref InventoryItemData prevItemData)
    {
        if (prevItemData != null && prevItemData._itemData.id != 0)
        {
            // 기존에 있던 장비 빼기
            prevItemData._isEquip = false;
            // 오브젝트 비활성화
        }
        else
        {
            newItemData._isEquip = true;
            prevItemData = newItemData;

            // 오브젝트 활성화
        }
    }
}