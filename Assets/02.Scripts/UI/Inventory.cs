using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const string CATEGORY = "Category";
    private const string SLOTS = "Slots";

    [BoxGroup(CATEGORY), SerializeField]
    private ObjectPool _categoryPool;

    [BoxGroup(SLOTS), SerializeField]
    private ObjectPool _rowPool;

    public void Init()
    {
        InitCategory();
    }

    private void InitCategory()
    {
        for (int index = 0; index < ModelCategoryTab.GetModelList().Count; index++)
        {
            var item = ModelCategoryTab.GetModelList()[index];
            var catePrefab = _categoryPool.GetObject();

            if (catePrefab.TryGetComponent(out CategoryTab tab))
            {
                tab.Init(item);

                if (index == 0)
                {
                    ItemManager.Instance.SetCurrentCategoryTab(tab);
                    tab.SetSelect(true);
                }
            }
        }
    }

    private void InitSlots()
    {

    }
}
