using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
  public Camera cam;
  public Canvas canvas;
  public Colors colors;
  public Cells cells;
  public Board board;
  public Score score;
  public Over over;
  public ReadyGo ready;
  public AudioSource bgmAudioSource;
  void Start()
  {
    Application.targetFrameRate = 60;
    colors.Init(cam.backgroundColor);
    cells.Init(this);
    board.Init(this);
    ready.ReadyGoEvent += OnReadyGo;
    over.Init();
    score.Resets();
    board.Render();
    ready.Activate();
  }
  void OnReadyGo(object sender, EventArgs e)
  {
    board.gameObject.SetActive(true);
    board.Drop();
    bgmAudioSource.Play();
  }
  public void Restart()
  {
    over.SetActive(false);
    board.Resets();
    score.Resets();
    board.Render();
    ready.Activate();
  }
  public void Quit()
  {
    over.SetActive(false);
    cells.Disable();
    canvas.gameObject.SetActive(false);
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
  }
}

