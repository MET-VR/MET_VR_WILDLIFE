using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabnetAnimations : MonoBehaviour
{
    [SerializeField] private Animator[] animators;
    [SerializeField] private int Drawernub = 1;
    // Start is called before the first frame update

    void Start()
    {
        Open();
    }

    // Update is called once per frame
    void Update()
    {
        animators[Drawernub].SetInteger("Drawer", Drawernub);
        
    }

    public void Open()
    {
        animators[Drawernub].SetTrigger("Open");
    }
    public void Close()
    {
        animators[Drawernub].SetTrigger("Close");
    }

}
