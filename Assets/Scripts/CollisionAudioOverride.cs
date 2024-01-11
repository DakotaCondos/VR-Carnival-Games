using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudioOverride : MonoBehaviour
{
    [SerializeField] private bool doesIgnoreAudio = true;

    public bool DoesIgnoreAudio => doesIgnoreAudio;
}
