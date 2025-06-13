using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleEventSystem : MonoBehaviour
{
    private EventSystem eventSystem;
    private StandaloneInputModule inputModule;

    void Awake()
    {
        // Check if an EventSystem already exists
        eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
        {
            // Create a new GameObject to hold the EventSystem
            GameObject eventSystemObj = new GameObject("EventSystem");

            // Add EventSystem component
            eventSystem = eventSystemObj.AddComponent<EventSystem>();

            // Add StandaloneInputModule component
            inputModule = eventSystemObj.AddComponent<StandaloneInputModule>();
        }
    }
}