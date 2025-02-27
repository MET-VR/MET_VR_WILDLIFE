using UnityEngine;
using System.Collections;

public class BirdRandomNormalSoundPlayer : MonoBehaviour
{
    public AudioClip[] birdSounds; // Assign mockingbird sounds in Inspector
    public float minInterval = 5f, maxInterval = 10.0f;
    private bool isFeeding = false; // Tracks if feeding is happening

    void Start()
    {
        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Only play random sounds if the bird is NOT feeding
            if (!isFeeding && birdSounds.Length > 0 && AudioManager1.Instance != null)
            {
                AudioClip soundToPlay = birdSounds[Random.Range(0, birdSounds.Length)];
                AudioManager1.Instance.PlaySFX(soundToPlay);
            }
        }
    }

    public void SetFeedingState(bool feeding)
    {
        isFeeding = feeding;
    }
}
