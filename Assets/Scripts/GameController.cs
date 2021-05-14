using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public Camera cam;
  public Canvas canvas;
  public Board board;
  public Text ScoreText;
  public GameOver gameOver;
  public ReadyGo ready;
  public AudioSource bgmAudioSource;
  private int frame;
  private int score;
  void Start()
  {
    frame = 60;
    score = 0;
    Application.targetFrameRate = frame;
    board.GameOverEvent += OnGameOver;
    board.DeletedEvent += OnDeleted;
    ready.ReadyGoEvent += OnReadyGo;
    gameOver.RestartEvent += OnRestart;
    gameOver.QuitEvent += OnQuit;

    board.Init(frame);

    ready.Activate();
  }
  void OnReadyGo(object sender, EventArgs e)
  {
    board.gameObject.SetActive(true);
    bgmAudioSource.Play();
  }
  void OnDeleted(object sender, DeletedEventArgs e)
  {
    if (e.Lines == 1) score += 40;
    else if (e.Lines == 2) score += 100;
    else if (e.Lines == 3) score += 300;
    else score += e.Lines * 300;
    ScoreText.text = score.ToString();
  }
  void OnGameOver(object sender, EventArgs e)
  {
    gameOver.Activate();
  }
  void OnRestart(object sender, EventArgs e)
  {
    board.Reset();
    score = 0;
    ready.Activate();
  }
  void OnQuit(object sender, EventArgs e)
  {
    canvas.gameObject.SetActive(false);
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
  }
}

