using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : SingletonObject<MeshManager>
{
    private CharacterMeshSO _characterMeshSO;
    public CharacterMeshSO ThisCharacterMeshSO => _characterMeshSO;

    protected override void Awake()
    {
        base.Awake();

        _characterMeshSO = AssetBundleManager.Instance.GetSOBundle().LoadAsset<CharacterMeshSO>(nameof(CharacterMeshSO));
    }
}
