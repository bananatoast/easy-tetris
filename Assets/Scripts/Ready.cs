using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ready : MonoBehaviour
{
  public Text txt;
  public AudioClip soundGo;
  public AudioClip soundReady;
  AudioSource audioSource;
  AudioSource bgmAudioSource;

  Controller c; int frm; string ready;
  internal void Init(Controller ctrl)
  {
    audioSource = GameObject.Find("AudioHolder").GetComponent<AudioSource>();
    bgmAudioSource = GameObject.Find("BGMHolder").GetComponent<AudioSource>();
    c = ctrl; ready = txt.text;
  }
  internal void Enable()
  {
    frm = 0;
    txt.text = ready;
    gameObject.SetActive(true);
    audioSource.PlayOneShot(soundReady);
  }
  void Update()
  {
    frm++;
    if (frm == 60)
    {
      audioSource.PlayOneShot(soundGo);
      txt.text = "Go!";
    }
    else if (frm == 120)
    {
      gameObject.SetActive(false);
      c.board.gameObject.SetActive(true);
      bgmAudioSource.Play();
      c.board.Drop();
    }
  }
}
