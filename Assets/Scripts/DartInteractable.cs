using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DartInteractable : XRGrabInteractable
{
    [SerializeField] private TriggerNotifier _triggerNotifier;
    private bool _isGrabbed = false;
    public bool IsGrabbed { get => _isGrabbed; }
    private Rigidbody _grabbedRigidbody;

    [Header("Audio")]
    [SerializeField] private AudioClip _grabSound; // Sound when the dart is grabbed
    [SerializeField] private AudioClip _releaseSound; // Sound when the dart is released
    [SerializeField] private AudioClip _hitSound; // Sound when the dart hits a target
    [SerializeField] float _minVelocityAudio = 0.25f;
    [SerializeField] float _maxVelocityAudio = 2f;
    [SerializeField] float _grabSoundVolume = 0.1f;
    private AudioSource _audioSource;
    private VelocityEstimator _velocityEstimator;


    protected override void Awake()
    {
        base.Awake();
        if (!TryGetComponent(out _grabbedRigidbody))
        {
            Debug.LogWarning("Rigidbody not found");
        }
        if (!TryGetComponent(out _audioSource))
        {
            Debug.LogWarning("Audio source not found");
        }
        if (!TryGetComponent(out _velocityEstimator))
        {
            Debug.LogWarning("Velocity Estimator not found");
        }
    }

    private void Update()
    {
        if (IsGrabbed) { return; }
        if (!_grabbedRigidbody.useGravity && _grabbedRigidbody.velocity.magnitude > Mathf.Epsilon)
        {
            _grabbedRigidbody.useGravity = true;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_triggerNotifier != null)
        {
            _triggerNotifier.OnTriggerEvent += HandleTriggerEvent;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_triggerNotifier != null)
        {
            _triggerNotifier.OnTriggerEvent -= HandleTriggerEvent;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _isGrabbed = true;
        _grabbedRigidbody.useGravity = true;
        PlaySound(_grabSound, _grabSoundVolume);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _isGrabbed = false;
        _grabbedRigidbody.useGravity = true;
        PlaySound(_releaseSound, true);
    }
    private void HandleTriggerEvent(GameObject gameObject)
    {
        if (_isGrabbed) { return; }

        if (gameObject.TryGetComponent(out Collider collider))
        {
            //ignore trigger colliders
            if (collider.isTrigger) { return; }
        }

        AudioClip clip = _hitSound;
        if (gameObject.TryGetComponent(out SharedAudioEffects effect))
        {
            //play specific sound
            if (effect.MetalHit != null) { clip = effect.MetalHit; }
        }

        PlaySound(clip, true);

        _grabbedRigidbody.velocity = Vector3.zero;
        _grabbedRigidbody.angularVelocity = Vector3.zero;
        _grabbedRigidbody.useGravity = false;
    }
    private void PlaySound(AudioClip clip, bool useVelocity, float setVolume)
    {
        if (clip == null)
        {
            Debug.LogWarning("Missing Audio Clip");
        }

        if (useVelocity)
        {
            float velocity = _velocityEstimator.GetVelocityEstimate().magnitude;
            float calculatedVolume = Mathf.InverseLerp(_minVelocityAudio, _maxVelocityAudio, velocity);
            _audioSource.PlayOneShot(clip, calculatedVolume);
            return;
        }

        if (setVolume != 0)
        {
            _audioSource.PlayOneShot(clip, setVolume);
            return;
        }

        _audioSource.PlayOneShot(clip);
    }
    public void PlaySound(AudioClip clip, bool useVelocity)
    {
        PlaySound(clip, useVelocity, 0);
    }
    public void PlaySound(AudioClip clip, float setVolume)
    {
        PlaySound(clip, false, setVolume);
    }
}
