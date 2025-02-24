using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweezerCollision : MonoBehaviour
{
    public RadialSelection radialSelection; // Reference to the RadialSelection script
    public bool isResetTrigger = false;     // Flag to determine if this is a reset trigger

    private void OnTriggerEnter(Collider other)
    {
        // If this is not a reset trigger and the collided object is part of the secondary objects list
        if (!isResetTrigger && radialSelection.secondaryObjects.Contains(other.gameObject))
        {
            // Replace the tweezer with the secondary object (like food)
            radialSelection.ReplaceTweezerWithSecondary(other.gameObject);
        }
        // If this is a reset trigger and the collided object is in the reset triggers list
        else if (isResetTrigger && radialSelection.resetTriggers.Contains(other.gameObject))
        {
            // Reset to the original tweezer
            radialSelection.ResetToTweezer();
        }
        // If the collided object is tagged as "Food", replace with food tweezer
        if (other.CompareTag("Food"))
        {
            // Replace the tweezer with the tweezer that has food in it
            radialSelection.ReplaceTweezerWithFood();
        }
    }
}
