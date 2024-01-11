using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderReader : MonoBehaviour, IColliderReader
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] List<Collider> _ignoreableColliders = new();
    private readonly Collider[] _overlapResults = new Collider[50]; // Adjust the size as needed
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
            if (_overlapResults[i] != _boxCollider && !_ignoreableColliders.Contains(_overlapResults[i]))
            {
                return true; // There is at least one collider overlapping
            }
        }

        return false; // No colliders are overlapping
    }

    public int TotalColliding()
    {
        // Calculate the center and size of the box collider in world space
        Vector3 worldCenter = _boxCollider.transform.TransformPoint(_boxCollider.center);
        Vector3 worldSize = _boxCollider.transform.lossyScale;
        worldSize.x *= _boxCollider.size.x;
        worldSize.y *= _boxCollider.size.y;
        worldSize.z *= _boxCollider.size.z;

        // Get the number of colliders overlapping with this box
        int numColliders = Physics.OverlapBoxNonAlloc(worldCenter, worldSize * 0.5f, _overlapResults, _boxCollider.transform.rotation);


        int totalColliding = 0;
        for (int i = 0; i < numColliders; i++)
        {
            if (_overlapResults[i] != _boxCollider)
            {
                totalColliding++;
            }
        }

        return totalColliding;
    }
}
