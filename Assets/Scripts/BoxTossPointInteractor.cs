using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxTossPointInteractor : MonoBehaviour
{
    [SerializeField] public int Points;
    [SerializeField] TextMeshProUGUI _pointText;
    [SerializeField] private BoxCollider _boxCollider;

    private readonly Collider[] _overlapResults = new Collider[10]; // Adjust the size as needed
    public void SetPointValue(int pointValue)
    {
        Points = pointValue;
        string modifier = (pointValue > 0) ? "+" : "";
        _pointText.text = $"{modifier}{pointValue}";
    }

    public bool IsColliding()
    {
        // Calculate the center and size of the box collider in world space
        Vector3 worldCenter = _boxCollider.transform.TransformPoint(_boxCollider.center);
        Vector3 worldSize = _boxCollider.transform.lossyScale;
        worldSize.x *= _boxCollider.size.x;
        worldSize.y *= _boxCollider.size.y;
        worldSize.z *= _boxCollider.size.z;

        // Get the number of colliders overlapping with this box
        int numColliders = Physics.OverlapBoxNonAlloc(worldCenter, worldSize * 0.5f, _overlapResults, _boxCollider.transform.rotation);

        for (int i = 0; i < numColliders; i++)
        {
            if (_overlapResults[i] != _boxCollider)
            {
                return true; // There is at least one collider overlapping
            }
        }

        return false; // No colliders are overlapping
    }
}
