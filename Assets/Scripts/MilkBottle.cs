using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottle : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] float _minVelocityAudio = 0.25f;
    [SerializeField] float _maxVelocityAudio = 2f;


    private VelocityEstimator _velocityEstimator;
    private Collider _collider;
    private SharedAudioEffects _sharedAudioEffects;
    private AudioSource _audioSource;



    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _velocityEstimator = GetComponent<VelocityEstimator>();
        _sharedAudioEffects = GetComponent<SharedAudioEffects>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void ResetBottle()
    {
        _velocityEstimator.FinishEstimatingVelocity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_velocityEstimator.Routine == null)
        {
            _velocityEstimator.BeginEstimatingVelocity();
        }

        if (collision.gameObject.GetComponent<CollisionSoundPriority>()) { return; }

        AudioClip clip = _sharedAudioEffects.HardImpact;
        if (collision.gameObject.TryGetComponent(out SharedAudioEffects effect))
        {
            //play specific sound
            if (effect.HardImpact != null) { clip = effect.HardImpact; }
        }

        PlaySound(clip, true);
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
