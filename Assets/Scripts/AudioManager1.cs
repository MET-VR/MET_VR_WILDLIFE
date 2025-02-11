using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager1 : MonoBehaviour
{
   [Header("----------- Audio Source -----------")]
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource SFXSource;

   [Header("----------- Audio Clip -----------")]
   public AudioClip background;

   public AudioClip mockingbirdNormal;
   public AudioClip mockingbirdDistressed;

   public AudioClip fileCabinetHandle;
   public AudioClip fileCabinetOpen;

   public AudioClip CabinetHandle;
   public AudioClip CabinetOpen;

   public AudioClip filePickUp1;
   public AudioClip filePickUp2;
   public AudioClip filePickUp3;

   public AudioClip objectPickUp;

   public AudioClip openMenu;
   public AudioClip menuSelection;

   private void Start()
   {
    musicSource.clip = background;
    musicSource.loop = true;
    musicSource.Play();
    musicSource.volume = 10;
   }
   public void PlaySFX(AudioClip clip)
   {
      SFXSource.PlayOneShot(clip);
   }
}
