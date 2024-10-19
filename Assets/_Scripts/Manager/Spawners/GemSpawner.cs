using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private List<GemConfigSO> _gemConfigs;

    private void Start()
    {
        InvokeRepeating("SpawnGem", 2f, 0.3f);
    }

    void SpawnGem()
    {
        GameObject gem = ObjectPoolFactory.Spawn(_gemConfigs[0].PoolSettings).gameObject;
        gem.GetComponent<ItemPickUp>().SetItem(_gemConfigs[0].Item);
        gem.GetComponentInChildren<SpriteRenderer>().material = _gemConfigs[0].Material;
        gem.GetComponent<TransformMover>().SetMoveSpeed(_gemConfigs[0].moveSpeed);

        gem.transform.position = new Vector3(0f, 4, 0f);

    }
}
