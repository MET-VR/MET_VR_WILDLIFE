using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjustment : MonoBehaviour
{
    
    public Transform Cameraoffset;
    public float heightAmount;
    public Vector3 PlayerHeight;

    void Start()
    {
        PlayerHeight = new Vector3(0, .43f, 0);
    }

    void Update() 
    {
        //Cameraoffset.position = PlayerHeight;
    }

  
    // Adds height
    public void AddHeight()
    {
        Cameraoffset.position += new Vector3(0, heightAmount, 0);       
    }

    //Subtracts height
    public void MinusHeight()
    {
        Cameraoffset.position -= new Vector3(0, heightAmount, 0);
    }
}
