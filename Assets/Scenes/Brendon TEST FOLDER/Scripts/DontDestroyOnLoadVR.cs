using UnityEngine;

public class DontDestroyOnLoadVR : MonoBehaviour
{
    private void Awake()
    {
        // Check if there's already an instance of this object in the scene
        if (GameObject.Find(gameObject.name) != gameObject)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
}
