using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _instantiateLocation;
    private List<GameObject> _spawnables = new List<GameObject>();

    private void Awake()
    {
        if (_itemPrefab == null || _instantiateLocation == null)
        {
            throw new MissingReferenceException("Missing item prefab or instantiate location");
        }
    }

    public void SpawnItem()
    {
        var item = Instantiate(_itemPrefab, _instantiateLocation.position, _instantiateLocation.rotation);
        _spawnables.Add(item);
    }

    public void RemoveAllSpawnedItems()
    {
        foreach (var item in _spawnables)
        {
            Destroy(item);
        }
    }

}
