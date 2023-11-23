using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    // Define an event delegate that takes a GameObject as a parameter
    public delegate void TriggerEvent(GameObject gameObject);

    [SerializeField] private Collider _triggerCollider;
    [SerializeField] private string triggerTag = "";
    public string TriggerTag { get => triggerTag; }


    // Create an event based on the delegate
    public event TriggerEvent OnTriggerEvent;

    // Method to trigger the event and pass a GameObject as a parameter
    public void NotifyGameObjectEvent(GameObject gameObject)
    {
        // Check if there are any subscribers to the event
        if (OnTriggerEvent != null)
        {
            // Invoke the event and pass the GameObject
            OnTriggerEvent(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        NotifyGameObjectEvent(other.gameObject);
    }
}
