using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottleTossGame : MonoBehaviour
{
    [SerializeField] List<PlaceableDetection> _placeables;


    public void ResetPlaceables()
    {
        foreach (var item in _placeables)
        {
            item.ResetPositionAndRotation();
        }
    }
}
