using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource), typeof(VelocityEstimator), typeof(Rigidbody))]
public class CollisionAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private float _minVelocityAudio = 0.25f;
    [SerializeField] private float _maxVelocityAudio = 2f;
    [SerializeField][Range(0f, 1f)] private float _defaultAudioVolume = 0.5f;


    [Header("Object Sound Settings")]
    [SerializeField] private SharedAudioEffectsType _effectType;
    [SerializeField] private bool _alwaysPlayHitSound = false;
    [SerializeField] private Collider _specificCollider = null;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private VelocityEstimator _velocityEstimator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _velocityEstimator = GetComponent<VelocityEstimator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // specific collider guard statement
        if (_specificCollider != null)
        {
            bool validContact = false;
            foreach (var contact in collision.contacts)
            {
                if (contact.thisCollider == _specificCollider)
                {
                    validContact = true;
                }
            }
            if (!validContact) { return; }
        }

        AudioClip clip = _hitSound;
        bool usedHitSound = true;

        // use specific sound if available
        if (collision.gameObject.TryGetComponent(out SharedAudioEffects effect))
        {
            AudioClip sharedAudioEffectsClip = effect.GetAudioEffectType(_effectType);
            if (sharedAudioEffectsClip != null)
            {
                clip = sharedAudioEffectsClip;
                usedHitSound = false;
            }
        }

        PlaySoundBasedOnRigidbody(clip);

        if (_alwaysPlayHitSound && !usedHitSound)
        {
            PlaySoundBasedOnRigidbody(_hitSound);
        }

        void PlaySoundBasedOnRigidbody(AudioClip clip)
        {
            // Determin playsound method
            if (!_rigidbody.IsSleeping())
            {
                PlaySound(clip, true);
            }
            else
            {
                PlaySound(clip, _defaultAudioVolume);
            }
        }
    }

    public void PlaySound(AudioClip clip, bool useVelocity)
    {
        PlaySound(clip, useVelocity, 0);
    }

    public void PlaySound(AudioClip clip, float setVolume)
    {
        PlaySound(clip, false, setVolume);
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

}
