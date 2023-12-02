using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedAudioEffects : MonoBehaviour
{
    public AudioClip MetalHit { get => _metalHit; }
    [SerializeField] private AudioClip _metalHit;
    public AudioClip SoftImpact { get => _softImpact; }
    [SerializeField] private AudioClip _softImpact;
    public AudioClip HardImpact { get => _hardImpact; }
    [SerializeField] private AudioClip _hardImpact;

}