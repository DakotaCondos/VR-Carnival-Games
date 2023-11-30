using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    [Tooltip("Empty GameObject that defines the allowed entry direction.")]
    public Transform entryDirectionMarker;

    private void OnCollisionEnter(Collision collision)
    {
        if (entryDirectionMarker == null)
        {
            Debug.LogError("EntryDirectionMarker is not set.");
            return;
        }

        // Calculate the direction from the entry marker to the collision point
        Vector3 collisionDirection = collision.transform.position - entryDirectionMarker.position;
        // Calculate the direction from the entry marker to the wall
        Vector3 wallDirection = transform.position - entryDirectionMarker.position;

        // Normalize directions to compare them
        collisionDirection.Normalize();
        wallDirection.Normalize();

        // Check if the object is approaching from the allowed direction
        if (Vector3.Dot(collisionDirection, wallDirection) > 0)
        {
            // Disable the collider temporarily
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Re-enable the collider when the object exits the collision
        GetComponent<Collider>().enabled = true;
    }
}
