using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private List<GemConfigSO> _gemConfigs;

    private void Start()
    {
        InvokeRepeating("SpawnGem", 2f, 1f);
    }

    void SpawnGem()
    {
        GameObject gem = ObjectPoolFactory.Spawn(_gemConfigs[0].PoolSettings).gameObject;
        gem.GetComponent<ItemPickUp>().SetItem(_gemConfigs[0].Item);
        //gem.GetComponentInChildren<SpriteRenderer>().material = _gemConfigs[0].Material;
        gem.GetComponent<TransformMover>().SetMoveSpeed(_gemConfigs[0].moveSpeed);

        float randomX = Random.Range(-2, 2);
        float randomY = Random.Range(4, 7);

        gem.transform.position = new Vector3(randomX, randomY, 0f);

    }
}
