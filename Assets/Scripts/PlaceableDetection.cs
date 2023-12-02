using UnityEngine;
using DG.Tweening;
using System;

public class PlaceableDetection : MonoBehaviour
{
    public enum DetectionMode
    {
        Movement,
        Rotation,
        Both,
        Either
    }

    [Header("Detection Settings")]
    [SerializeField] private DetectionMode detectionMode = DetectionMode.Both;
    [SerializeField] private float movementThreshold = 0.1f;
    [SerializeField] private float rotationThreshold = 10.0f;

    [Header("Reset Settings")]
    [SerializeField] private float resetDuration = 1.0f;
    [SerializeField] private Ease resetEase = Ease.InOutQuad;
    [SerializeField] private Rigidbody optionalRigidbody; // Serializable field for Rigidbody

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public bool isDisplaced = false;

    public float ResetDuration { get => resetDuration; }

    public event Action OnDisplaced;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!isDisplaced)
        {
            bool positionDisplaced = Vector3.Distance(transform.position, initialPosition) > movementThreshold;
            bool rotationDisplaced = Quaternion.Angle(transform.rotation, initialRotation) > rotationThreshold;

            switch (detectionMode)
            {
                case DetectionMode.Movement:
                    isDisplaced = positionDisplaced;
                    break;
                case DetectionMode.Rotation:
                    isDisplaced = rotationDisplaced;
                    break;
                case DetectionMode.Both:
                    isDisplaced = positionDisplaced && rotationDisplaced;
                    break;
                case DetectionMode.Either:
                    isDisplaced = positionDisplaced || rotationDisplaced;
                    break;
            }

            if (isDisplaced)
            {
                OnDisplaced?.Invoke();
            }
        }
    }

    public void ResetPositionAndRotation()
    {
        if (optionalRigidbody)
        {
            optionalRigidbody.isKinematic = true; // Disable Rigidbody physics
        }

        transform.DOMove(initialPosition, resetDuration).SetEase(resetEase).OnComplete(() =>
        {
            if (optionalRigidbody)
            {
                optionalRigidbody.isKinematic = false; // Re-enable Rigidbody physics
            }
            isDisplaced = false;
        });

        transform.DORotateQuaternion(initialRotation, resetDuration).SetEase(resetEase);
    }
}
