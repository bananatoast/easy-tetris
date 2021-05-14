using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public int StartingFrameRate = 60;
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
    frame = StartingFrameRate;
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
    Scoring(e.Lines);
    ChangeSpeed(score);
  }
  private void Scoring(int lines)
  {
    if (lines == 1) score += 40;
    else if (lines == 2) score += 100;
    else if (lines == 3) score += 300;
    else score += lines * 300;
    ScoreText.text = score.ToString();
  }
  private void ChangeSpeed(int score)
  {
    if (score > 8000) frame = (int)(StartingFrameRate * 0.2);
    else if (score > 5000) frame = (int)(StartingFrameRate * 0.3);
    else if (score > 3000) frame = (int)(StartingFrameRate * 0.5);
    else if (score > 1500) frame = (int)(StartingFrameRate * 0.7);
    else if (score > 500) frame = (int)(StartingFrameRate * 0.9);
    board.DropFrame = frame;
  }
  void OnGameOver(object sender, EventArgs e)
  {
    gameOver.Activate();
  }
  void OnRestart(object sender, EventArgs e)
  {
    frame = StartingFrameRate;
    board.Reset(frame);
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

