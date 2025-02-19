using UnityEngine;

public class HeightManager : MonoBehaviour
{
    // Static instance for Singleton access
    public static HeightManager Instance;

    public float playerHeight = 0.43f; // Default height

    private void Awake()
    {
        // Ensure only one instance of HeightManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
}
