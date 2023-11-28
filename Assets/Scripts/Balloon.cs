using System;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private AudioClip _popSound;
    [SerializeField] private Material[] _PossibleMaterials;
    [SerializeField] private Material _balloonMaterial;
    [SerializeField] private bool _enablePopWhileGrabbed = true;
    [SerializeField] float _balloonPopVolume = 0.75f;

    public event Action OnBalloonPop;

    private Collider _collider;
    private ParticleSystem _particleSystem;
    private MeshRenderer _meshRenderer;
    private bool _isPopped = false;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _particleSystem = GetComponent<ParticleSystem>();
        _meshRenderer = GetComponent<MeshRenderer>();
        if (_popSound == null) { Debug.LogWarning("Pop sound missing"); }
        if (_PossibleMaterials.Length == 0) { Debug.LogWarning("Missing balloon colors"); }
        RandomizeColor();
    }

    private void RandomizeColor()
    {
        _balloonMaterial = _PossibleMaterials[UnityEngine.Random.Range(0, _PossibleMaterials.Length)];
        _meshRenderer.material = _balloonMaterial;
        _particleSystem.GetComponent<ParticleSystemRenderer>().material = _balloonMaterial;
    }

    public void ResetBalloon()
    {
        _isPopped = false;
        _meshRenderer.enabled = true;
        _collider.enabled = true;
        RandomizeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isPopped) { return; }

        if (other.gameObject.TryGetComponent(out TriggerNotifier triggerNotifier))
        {
            if (triggerNotifier.TriggerTag == "DartTip")
            {
                DartInteractable dart = other.gameObject.GetComponentInParent<DartInteractable>();
                if (dart.IsGrabbed && !_enablePopWhileGrabbed) { return; }
                dart.PlaySound(_popSound, _balloonPopVolume);
                _particleSystem.Play();
                _isPopped = true;
                _meshRenderer.enabled = false;
                _collider.enabled = false;

                OnBalloonPop?.Invoke();
            }
        }
    }
}
