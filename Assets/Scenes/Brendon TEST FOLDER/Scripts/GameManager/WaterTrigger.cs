using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public GameMangaer gameManager;  // Reference to Game Manager
    //private string currentTag;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tweezers"))
        {
             if (this.gameObject.tag == "Chlorhexidine")
             {
                gameManager.triggers += 1;
                gameManager.UpdateDebugText(this.gameObject.tag);
                if (gameManager.triggers != 1)
                {
                    gameManager.ResetTweezers();
                    Debug.Log(gameManager.triggers);
                }
             }
             else if (this.gameObject.tag == "10% bleach")
             {
                gameManager.triggers += 2;
                gameManager.UpdateDebugText(this.gameObject.tag);
                if (gameManager.triggers != 2)
                {
                    Debug.Log(gameManager.triggers);
                    gameManager.ResetTweezers();
                    
                }
             }
            else if (this.gameObject.tag == "water1")
            {
                gameManager.triggers += 3;
                gameManager.UpdateDebugText(this.gameObject.tag);
                if (gameManager.triggers != 6)
                {
                    Debug.Log(gameManager.triggers);
                    gameManager.ResetTweezers();                   
                }
            }
            else if (this.gameObject.tag == "water2")
            {
                gameManager.triggers += 4;
                gameManager.UpdateDebugText(this.gameObject.tag);
                if (gameManager.triggers != 10)
                {
                    Debug.Log(gameManager.triggers);
                    gameManager.ResetTweezers();
                }
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        //gameManager.triggers = 0;
    }
}
