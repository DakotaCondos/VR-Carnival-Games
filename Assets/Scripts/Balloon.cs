using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private AudioClip _popSound;
    [SerializeField] private Material[] _PossibleMaterials;
    [SerializeField] private Material _balloonMaterial;
    private Collider _collider;
    private ParticleSystem _particleSystem;
    private MeshRenderer _meshRenderer;

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
        _balloonMaterial = _PossibleMaterials[Random.Range(0, _PossibleMaterials.Length)];
        _meshRenderer.material = _balloonMaterial;
        _particleSystem.GetComponent<ParticleSystemRenderer>().material = _balloonMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out DartInteractable dart))
        {
            print("DART HIT BALLOON");
            dart.PlaySound(_popSound, true);
            _particleSystem.Play();

            RandomizeColor();
        }
    }
}
