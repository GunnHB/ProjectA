using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonObject<ItemManager>
{
    private CategoryTab _currentCategoryTab;

    public CategoryTab CurrentCategoryTab;

    public void ChangeCurrentCategoryTab(CategoryTab tab)
    {
        _currentCategoryTab.SetSelect(false);

        _currentCategoryTab = tab;

        _currentCategoryTab.SetSelect(true);
    }
}
