using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class FrogLauncher : MonoBehaviour
{
    public CollisionDetector collisionDetector;
    public Rigidbody targetRigidbody; // The Rigidbody to which the force will be applied
    public AnimationCurve directionalIntensityCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public AnimationCurve heightIntensityCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    [SerializeField] float _maxImpartedForceHeight = 18f;
    [SerializeField] float _launchDirectionScalar = 7.5f;
    [SerializeField] float _launchHeightScalar = 10;

    void Start()
    {
        if (collisionDetector != null)
        {
            // Subscribe to the OnImpact event
            collisionDetector.OnImpact += LaunchFrog;
        }
    }

    private void LaunchFrog(CollisionData collisionData)
    {
        // Apply the impulse as a force to the target Rigidbody
        if (targetRigidbody == null) { return; }

        // use velovity tracking for force magnitude
        float forceHeight = 0;
        if (collisionData.Collider.gameObject.TryGetComponent(out VelocityEstimator velocityEstimator))
        {
            float forceMagnitude = velocityEstimator.GetVelocityEstimate().magnitude;
            forceHeight = heightIntensityCurve.Evaluate(Mathf.InverseLerp(0, _maxImpartedForceHeight, forceMagnitude)) * _launchHeightScalar;
        }

        //guard check before other logic
        if (forceHeight == 0) { return; }

        Vector3 contactVector = -collisionData.DirectionToContactPoint; //get direction FROM contact point
        Vector3 launchDirection = directionalIntensityCurve.Evaluate(contactVector.magnitude) * contactVector;

        // apply scaler to create actual vector for directional force
        Vector3 launchDirectionVector = launchDirection * _launchDirectionScalar;

        targetRigidbody.AddForce(launchDirectionVector, ForceMode.Impulse);
        targetRigidbody.AddForce(new Vector3(0, forceHeight, 0), ForceMode.Impulse);

    }


    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (collisionDetector != null)
        {
            collisionDetector.OnImpact -= LaunchFrog;
        }
    }
}
