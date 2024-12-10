using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object Pool")]
public class ObjectPoolSettingsSO : ScriptableObject
{
    [SerializeField] private ObjectPoolType _type;
    [SerializeField] private GameObject _prefab;

    private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxPoolSize = 100;
    [SerializeField] private bool _prewarm = false;
    private bool _hasBeenPreWarmed = false;

    public ObjectPoolType Type => _type;
    public GameObject Prefab => _prefab;

    internal bool CollectionCheck => _collectionCheck;
    internal int DefaultCapacity => _defaultCapacity;
    internal int MaxPoolSize => _maxPoolSize;

    internal bool Prewarm => _prewarm;
    public bool HasBeenPreWarmed { get => _hasBeenPreWarmed; internal set => _hasBeenPreWarmed = value; }

    internal ObjectPooler Create()
    {
        var go = Instantiate(Prefab, ObjectPoolFactory.Instance.GetChildTransformPosition(_type));
        go.SetActive(false);
        go.name = Prefab.name;

        var objectPooler = go.GetOrAdd<ObjectPooler>();
        objectPooler.SetSettings(this);

        return objectPooler;
    }

    internal void OnGet(ObjectPooler o) => o.gameObject.SetActive(true);
    internal void OnRelease(ObjectPooler o) => o.gameObject.SetActive(false);
    internal void OnDestroyPoolObject(ObjectPooler o) => Destroy(o.gameObject);

    private void OnEnable()
    {
        _hasBeenPreWarmed = false;
    }
}
