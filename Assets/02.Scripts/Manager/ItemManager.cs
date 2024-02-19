using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonObject<ItemManager>
{
    private CategoryTab _currentCategoryTab;

    public CategoryTab CurrentCategoryTab => _currentCategoryTab;

    public void SetCurrentCategoryTab(CategoryTab tab)
    {
        _currentCategoryTab = tab;
    }

    public void ChangeCurrentCategoryTab(CategoryTab tab)
    {
        _currentCategoryTab.SetSelect(false);

        SetCurrentCategoryTab(tab);

        _currentCategoryTab.SetSelect(true);
    }
}
