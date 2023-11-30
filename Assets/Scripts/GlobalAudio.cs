using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GlobalAudio : MonoBehaviour
{
    // Singleton instance
    public static GlobalAudio Instance { get; private set; }

    [SerializeField]
    private AudioClip UIClick;

    private AudioSource audioSource;

    void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional: To persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy if another instance exists
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    // Method to play the UIClick sound
    public void PlayUIClick()
    {
        if (UIClick != null)
        {
            audioSource.PlayOneShot(UIClick);
        }
    }
}
