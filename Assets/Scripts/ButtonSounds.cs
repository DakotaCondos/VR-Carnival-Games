using UnityEngine;
using UnityEngine.UI; // Required for UI components

[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour
{
    private Button button;

    void Start()
    {
        // Get the Button component
        button = GetComponent<Button>();

        // Check if GlobalAudio instance exists
        if (GlobalAudio.Instance != null)
        {
            // Add PlayUIClick to the button's onClick event
            button.onClick.AddListener(GlobalAudio.Instance.PlayUIClick);
        }
        else
        {
            Debug.LogWarning("GlobalAudio instance not found");
        }
    }
}
