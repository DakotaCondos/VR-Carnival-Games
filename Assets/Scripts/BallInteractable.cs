using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallInteractable : XRGrabInteractable
{
    private bool _isGrabbed = false;
    public bool IsGrabbed { get => _isGrabbed; }
    private Rigidbody _grabbedRigidbody;

    [Header("Audio")]
    [SerializeField] private AudioClip _grabSound;
    [SerializeField] private AudioClip _releaseSound;
    [SerializeField] private AudioClip _hitSound;
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

    private void OnCollisionEnter(Collision collision)
    {
        AudioClip clip = _hitSound;

        PlaySound(clip, true);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _isGrabbed = true;
        PlaySound(_grabSound, _grabSoundVolume);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _isGrabbed = false;
        PlaySound(_releaseSound, true);
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
