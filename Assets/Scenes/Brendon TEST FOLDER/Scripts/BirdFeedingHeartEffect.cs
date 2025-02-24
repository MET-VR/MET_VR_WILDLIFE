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
    public float heartLifetime = 3f; // How long hearts last

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
        // Play animation
        if (birdAnimator != null)
        {
            birdAnimator.SetTrigger("Eat");
        }

        // Play sound effect
        if (AudioManager1.Instance != null && feedingSound != null)
        {
            AudioManager1.Instance.PlaySFX(feedingSound);
        }

        // Spawn hearts around the bird
        SpawnHearts();

        // Change player's held item (for now, we just destroy it and spawn a new one)
        Destroy(foodItem);
        GiveNewItem();
    }

    void SpawnHearts()
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(0.5f, 1.5f), Random.Range(-2f, 2f));
            GameObject heart = Instantiate(heartPrefab, effectSpawnPoint.position + randomOffset, Quaternion.identity);
            Destroy(heart, heartLifetime); // Destroy the heart after it fades out
        }
    }

    void GiveNewItem()
    {
        // Replace with the new object (modify this part for your inventory system)
        GameObject newItem = Instantiate(Resources.Load<GameObject>("NewItem"), itemHolder.position, Quaternion.identity);
        newItem.transform.SetParent(itemHolder);
    }
}
