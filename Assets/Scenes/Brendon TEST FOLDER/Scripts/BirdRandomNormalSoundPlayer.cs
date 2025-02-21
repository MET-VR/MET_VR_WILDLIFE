using UnityEngine;
using System.Collections;

public class BirdRandomNormalSoundPlayer : MonoBehaviour
{
    public AudioClip[] birdSounds; // Assign mockingbird sounds in Inspector
    public float minInterval = 5f, maxInterval = 10.0f;

    void Start()
    {
        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            if (birdSounds.Length > 0 && AudioManager1.Instance != null)
            {
                AudioClip soundToPlay = birdSounds[Random.Range(0, birdSounds.Length)];
                AudioManager1.Instance.PlaySFX(soundToPlay);
            }
            else
            {
                Debug.LogWarning("AudioManager1 instance is missing or birdSounds array is empty.");
            }
        }
    }
}
