using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSoundPriority : MonoBehaviour
{
    public int Priority { get => _priority; }
    [SerializeField] private int _priority = 0;
}
