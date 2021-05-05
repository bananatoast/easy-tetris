﻿using System;
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
  public GameOver gameOver;
  public ReadyGo ready;
  public AudioSource bgmAudioSource;
  int frame;
  void Start()
  {
    frame = 60;
    Application.targetFrameRate = frame;
    board.GameOverEvent += OnGameOver;
    board.DeletedEvent += OnDeleted;
    ready.ReadyGoEvent += OnReadyGo;
    gameOver.RestartEvent += OnRestart;
    gameOver.QuitEvent += OnQuit;

    colors.Init(cam.backgroundColor);
    cells.Init(colors);
    board.Init(cells);
    board.DropFrame = frame;
    score.Reset();
    board.Render();

    ready.Activate();
  }
  void OnReadyGo(object sender, EventArgs e)
  {
    board.gameObject.SetActive(true);
    bgmAudioSource.Play();
  }
  void OnDeleted(object sender, DeletedEventArgs e)
  {
    score.Add(e.Lines);
  }
  void OnGameOver(object sender, EventArgs e)
  {
    gameOver.Activate();
  }
  void OnRestart(object sender, EventArgs e)
  {
    board.Reset();
    score.Reset();
    ready.Activate();
  }
  void OnQuit(object sender, EventArgs e)
  {
    cells.Disable();
    canvas.gameObject.SetActive(false);
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
  }
}

