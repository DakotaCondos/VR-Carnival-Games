using UnityEngine;

public enum SharedAudioEffectsType
{
    MetalHit,
    SoftImpact,
    HardImpact
}

public class SharedAudioEffects : MonoBehaviour
{
    public AudioClip MetalHit { get => _metalHit; }
    [SerializeField] private AudioClip _metalHit;
    public AudioClip SoftImpact { get => _softImpact; }
    [SerializeField] private AudioClip _softImpact;
    public AudioClip HardImpact { get => _hardImpact; }
    [SerializeField] private AudioClip _hardImpact;

    public AudioClip GetAudioEffectType(SharedAudioEffectsType effectType)
    {
        return effectType switch
        {
            SharedAudioEffectsType.MetalHit => MetalHit,
            SharedAudioEffectsType.SoftImpact => SoftImpact,
            SharedAudioEffectsType.HardImpact => HardImpact,
            _ => null,
        };
    }


}