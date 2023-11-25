using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _dartPrefab;
    [SerializeField] private Transform _instantiateLocation;
    private List<GameObject> _spawnables = new List<GameObject>();

    private void Awake()
    {
        if (_dartPrefab == null || _instantiateLocation == null)
        {
            throw new MissingReferenceException("Missing Dart prefab or instantiate location");
        }
    }

    public void SpawnDart()
    {
        var dart = Instantiate(_dartPrefab, _instantiateLocation.position, _instantiateLocation.rotation);
        _spawnables.Add(dart);
    }

    public void RemoveAllDarts()
    {
        foreach (var item in _spawnables)
        {
            Destroy(item);
        }
    }

}
