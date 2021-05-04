using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyGo : MonoBehaviour
{
  public Text label;
  public AudioClip soundGo;
  public AudioClip soundReady;
  public AudioSource audioSource;
  public string readyText;
  public string goText;
  public event EventHandler ReadyGoEvent;
  int frame;
  internal void Activate()
  {
    frame = 0;
    label.text = readyText;
    gameObject.SetActive(true);
    audioSource.PlayOneShot(soundReady);
  }
  void Update()
  {
    frame++;
    if (frame == 60)
    {
      audioSource.PlayOneShot(soundGo);
      label.text = goText;
    }
    else if (frame == 120)
    {
      gameObject.SetActive(false);
      ReadyGoEvent(this, null);
    }
  }
}
