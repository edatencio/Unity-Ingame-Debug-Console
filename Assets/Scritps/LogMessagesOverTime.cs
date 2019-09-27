using UnityEngine;

public class LogMessagesOverTime : MonoBehaviour
{
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool logMessage = false;
    [SerializeField] private bool logWarning = false;
    [SerializeField] private bool logError = false;
    private float timer;

    private void Update()
    {
        if (timer <= 0)
        {
            timer = delay;

            if (logMessage) Debug.Log("Message");
            if (logWarning) Debug.LogWarning("Warning");
            if (logError) Debug.LogError("Error");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
