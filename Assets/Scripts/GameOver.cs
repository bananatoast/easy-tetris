using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
  public AudioClip soundGameover;
  public Text title;
  public Text restart;
  public Text quit;
  public AudioSource audioSource;
  public event EventHandler RestartEvent;
  public event EventHandler QuitEvent;
  void Awake()
  {
    gameObject.SetActive(false);
  }
  internal void Activate()
  {
    audioSource.PlayOneShot(soundGameover);
    restart.GetComponent<Selectable>().Select();
    gameObject.SetActive(true);
  }
  public void Restart()
  {
    gameObject.SetActive(false);
    RestartEvent(this, null);
  }
  public void Quit()
  {
    gameObject.SetActive(false);
    QuitEvent(this, null);
  }
}
