using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Over : MonoBehaviour
{
  public AudioClip soundGameover;
  public Text title;
  public Text restart;
  public Text quit;
  public AudioSource audioSource;
  public AudioSource bgmAudioSource;
  internal void Init()
  {
    gameObject.SetActive(false);
  }
  internal void Activate()
  {
    bgmAudioSource.Stop();
    audioSource.PlayOneShot(soundGameover);
    restart.GetComponent<Selectable>().Select();
    gameObject.SetActive(true);
  }
  internal void SetActive(bool active)
  {
    gameObject.SetActive(active);
  }
}
