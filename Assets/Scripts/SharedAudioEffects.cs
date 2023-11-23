using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedAudioEffects : MonoBehaviour
{
    [SerializeField] private AudioClip _impactSound;
    public AudioClip ImpactSound { get => _impactSound; }
}