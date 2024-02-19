using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

public class CategoryTab : MonoBehaviour
{
    private const string BOX_GROUP_NORMAL = "Normal";
    private const string BOX_GROUP_SELECT = "Select";

    [BoxGroup(BOX_GROUP_NORMAL), SerializeField]
    private Image _normalImage;
    [BoxGroup(BOX_GROUP_NORMAL), SerializeField]
    private UIButton _normalButton;

    [BoxGroup(BOX_GROUP_SELECT), SerializeField]
    private Image _selectImage;

    private ModelCategoryTab _model = null;

    private void Awake()
    {
        _normalButton.SetEnterAndExit(EnterAction, ExitAction);
    }

    public void Init(ModelCategoryTab model)
    {
        _model = model;

        if (_model == null)
            return;

        _normalImage.sprite = AtlasManager.Instance.InventoryAtlas.GetSprite(model.normal_sprite);
        _selectImage.sprite = AtlasManager.Instance.InventoryAtlas.GetSprite(model.select_sprite);

        SetSelect(false);

        // 반투명하게
        _normalImage.color = new Color(1f, 1f, 1f, .5f);
    }

    public void SetSelect(bool active = true)
    {
        _normalImage.gameObject.SetActive(!active);
        _selectImage.gameObject.SetActive(active);
    }

    private void SelectAction()
    {
        ItemManager.Instance.ChangeCurrentCategoryTab(this);
    }

    private void EnterAction()
    {

    }

    private void ExitAction()
    {

    }
}
