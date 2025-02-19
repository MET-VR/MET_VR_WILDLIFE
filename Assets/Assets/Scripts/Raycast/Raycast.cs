using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] private Outline FileCabnet;
    [SerializeField] private float RayRange;
    [SerializeField] private LayerMask info;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * RayRange, Color.red);

        if (Physics.Raycast(ray, out hit))
        {

        }


    }
}
