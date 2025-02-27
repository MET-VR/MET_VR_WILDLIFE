using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFeedingHeartEffect : MonoBehaviour
{
    public string requiredItemTag = "Food"; // The tag of the object you need to feed
    public Transform itemHolder; // Where the new item appears after feeding

    [Header("Effects & Animation")]
    public Animator birdAnimator; // Bird Animator
    public AudioClip feedingSound; // Sound effect
    public GameObject heartPrefab; // Heart image prefab
    public Transform effectSpawnPoint; // Where the effect appears
    public int numberOfHearts = 10; // How many hearts to spawn
    public float heartLifetime = 2f; // How long hearts last

    private BirdRandomNormalSoundPlayer soundPlayer; // Reference to random sound player

    private void Start()
    {
        // Get the BirdRandomNormalSoundPlayer script if it's on the same GameObject
        soundPlayer = GetComponent<BirdRandomNormalSoundPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player is holding the correct item
        if (other.CompareTag(requiredItemTag))
        {
            FeedBird(other.gameObject);
        }
    }

    void FeedBird(GameObject foodItem)
    {
        // Stop random bird sounds
        if (soundPlayer != null)
        {
            soundPlayer.SetFeedingState(true);
        }

        // Play animation
        if (birdAnimator != null)
        {
            birdAnimator.SetTrigger("Eat");
        }

        // Play feeding sound
        if (AudioManager1.Instance != null && feedingSound != null)
        {
            AudioManager1.Instance.PlaySFX(feedingSound);
        }

        // Spawn hearts around the bird
        SpawnHearts();

        // Change player's held item (for now, we just destroy it and spawn a new one)
        Destroy(foodItem);
        GiveNewItem();

        // Resume random bird sounds after feeding delay
        StartCoroutine(ResumeRandomSounds());
    }

    IEnumerator ResumeRandomSounds()
    {
        yield return new WaitForSeconds(2.0f); // Wait for feeding sound to finish
        if (soundPlayer != null)
        {
            soundPlayer.SetFeedingState(false);
        }
    }

    void SpawnHearts()
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(0.5f, 1.5f), Random.Range(-2f, 2f));
            GameObject heart = Instantiate(heartPrefab, effectSpawnPoint.position + randomOffset, Quaternion.identity);

            // Make hearts face the camera
            heart.transform.LookAt(Camera.main.transform);
            heart.transform.Rotate(0, 180, 0); // Adjust rotation if needed

            // Make hearts float upwards
            StartCoroutine(FloatAndDestroy(heart));
        }
    }

    IEnumerator FloatAndDestroy(GameObject heart)
    {
        float elapsedTime = 0;
        Vector3 startPos = heart.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 1.5f, 0); // Move up

        while (elapsedTime < heartLifetime)
        {
            heart.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / heartLifetime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(heart);
    }

    void GiveNewItem()
    {
        // Replace with the new object (modify this part for your inventory system)
        GameObject newItem = Instantiate(Resources.Load<GameObject>("NewItem"), itemHolder.position, Quaternion.identity);
        newItem.transform.SetParent(itemHolder);
    }
}
