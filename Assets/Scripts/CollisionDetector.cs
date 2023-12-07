using UnityEngine;
using System;

public class CollisionDetector : MonoBehaviour
{
    // Define the OnImpact event with CollisionData
    public event Action<CollisionData> OnImpact;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint firstContact = collision.contacts[0];
            Vector3 firstContactPoint = firstContact.point;
            Vector3 impulse = firstContact.impulse;
            Vector3 directionToContactPoint = (firstContactPoint - transform.position).normalized;

            // Create a new CollisionData object
            CollisionData collisionData = new CollisionData(firstContactPoint, directionToContactPoint, impulse, collision.collider);

            // Invoke the OnImpact event with the CollisionData object
            OnImpact?.Invoke(collisionData);
        }
    }
}
