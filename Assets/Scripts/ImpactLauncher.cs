using UnityEngine;

public class ImpactLauncher : MonoBehaviour
{
    public Transform directionFocus; // Assign this in the inspector
    private float maxDistanceToColliderEdge;
    [SerializeField] MeshCollider meshCollider;

    // Define a public event for relaying collision information
    public delegate void ImpactEventHandler(Vector3 direction, float proximity);
    public event ImpactEventHandler OnImpact;

    void Start()
    {
        // Assuming the collider is a MeshCollider for a cylinder
        if (meshCollider != null)
        {
            // Calculate the radius of the cylinder (assuming the cylinder's local up-axis aligns with the world's y-axis)
            maxDistanceToColliderEdge = meshCollider.bounds.extents.x; // Radius of the cylinder
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the one we're interested in
        if (collision.gameObject.tag == "Hammer") // Replace with your game object's tag
        {
            // Get the point of impact
            Vector3 impactPoint = collision.contacts[0].point;

            // Calculate the direction from the impact point to the direction focus
            Vector3 directionFromImpactToFocus = (directionFocus.position - impactPoint).normalized;

            // Calculate the distance from the impact point to the direction focus (only along the top surface of the cylinder)
            float distanceFromImpactToFocus = Vector3.Distance(new Vector3(impactPoint.x, directionFocus.position.y, impactPoint.z), directionFocus.position);

            // Calculate the proximity as a value between 0 and 1
            float proximity =1-( distanceFromImpactToFocus / maxDistanceToColliderEdge);
            proximity = Mathf.Clamp(proximity, 0f, 1f);

            // Relay this information through the event
            OnImpact?.Invoke(directionFromImpactToFocus, proximity);
            print($"directionFromImpactToFocus:{directionFromImpactToFocus}, distanceFraction:{proximity}");
        }
    }
}