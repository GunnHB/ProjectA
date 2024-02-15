using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    protected enum UIType
    {
        NONE = -1,
        HUD,
        PANEL,
        POPUP,
    }

    [BoxGroup("[Common]"), SerializeField]
    protected bool _needBlur;
    [BoxGroup("[Common]"), SerializeField]
    protected UIType _uiType;

    private Image _blurImage;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        SetBlurMaterial();

        if (_blurImage != null && !_needBlur)
            _blurImage.gameObject.SetActive(false);
    }

    private void SetBlurMaterial()
    {
        _blurImage = transform.Find("Blur").GetComponent<Image>();

        if (_blurImage != null)
        {
            var materialObj = AssetBundleManager.Instance.MaterialBundle.LoadAsset<GameObject>("UIBlurMaterial");

            if (materialObj != null && materialObj.TryGetComponent(out Material mat))
                _blurImage.material = mat;
        }
    }
}
