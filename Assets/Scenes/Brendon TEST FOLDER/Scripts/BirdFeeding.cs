using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFeedingScript : MonoBehaviour
{
    public string requiredItemTag = "Food"; // The tag of the object you need to feed
    public Transform itemHolder; // Where the new item appears after feeding

    [Header("Effects & Animation")]
    public Animator birdAnimator; // Bird Animator
    public AudioClip feedingSound; // Sound effect
    public GameObject visualEffectPrefab; // Particle effect prefab
    

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

        
        // Change player's held item (for now, we just destroy it and spawn a new one)
        Destroy(foodItem);
        GiveNewItem();
    }

    void GiveNewItem()
    {
        // Replace with the new object (modify this part for your inventory system)
        GameObject newItem = Instantiate(Resources.Load<GameObject>("NewItem"), itemHolder.position, Quaternion.identity);
        newItem.transform.SetParent(itemHolder);
    }
}
