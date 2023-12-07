using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrogLauncher : MonoBehaviour
{
    public CollisionDetector collisionDetector;
    public Rigidbody targetRigidbody; // The Rigidbody to which the force will be applied

    [SerializeField] float _minImpartedForce = 1.0f;
    [SerializeField] float _maxImpartedForce = 8.0f;

    void Start()
    {
        if (collisionDetector != null)
        {
            // Subscribe to the OnImpact event
            collisionDetector.OnImpact += ApplyImpactToTarget;
        }
    }

    private void ApplyImpactToTarget(CollisionData collisionData)
    {
        // Apply the impulse as a force to the target Rigidbody
        if (targetRigidbody != null)
        {
            //get direction FROM contact point
            Vector3 launchDirection = -collisionData.DirectionToContactPoint;
            //Ensure always launching upwards
            if (launchDirection.y < 0) { launchDirection.y *= -1; }

            //use VelocityEstimator to impart force
            if (collisionData.Collider.gameObject.TryGetComponent(out VelocityEstimator velocityEstimator))
            {
                var force = velocityEstimator.GetVelocityEstimate().magnitude;
                force = Mathf.Clamp(force, _minImpartedForce, _maxImpartedForce);
                targetRigidbody.AddForce(launchDirection * force, ForceMode.Impulse);
            }

        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (collisionDetector != null)
        {
            collisionDetector.OnImpact -= ApplyImpactToTarget;
        }
    }
}
