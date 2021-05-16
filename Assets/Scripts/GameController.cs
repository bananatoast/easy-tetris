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
  public AudioClip normalBgm;
  public AudioClip criticalBgm;
  public AudioSource bgmAudioSource;
  private int frame;
  private int score;
  private DateTime startTime;
  void Start()
  {
    frame = StartingFrameRate;
    score = 0;
    Application.targetFrameRate = frame;
    board.GameOverEvent += OnGameOver;
    board.DeletedEvent += OnDeleted;
    board.ChangedSituationEvent += OnChangedSituation;
    ready.ReadyGoEvent += OnReadyGo;
    gameOver.RestartEvent += OnRestart;
    gameOver.QuitEvent += OnQuit;

    board.Init(frame);
    ready.Activate();
  }
  void Update()
  {
    ChangeSpeed();
  }
  void OnReadyGo(object sender, EventArgs e)
  {
    board.gameObject.SetActive(true);
    bgmAudioSource.clip = normalBgm;
    bgmAudioSource.Play();
    startTime = DateTime.Now;
  }
  void OnChangedSituation(object sender, ChangedSituationEventArgs e)
  {
    if (e.Situation == Situation.Normal)
    {
      bgmAudioSource.Stop();
      bgmAudioSource.clip = normalBgm;
      bgmAudioSource.Play();
    }
    else if (e.Situation == Situation.Critical)
    {
      bgmAudioSource.Stop();
      bgmAudioSource.clip = criticalBgm;
      bgmAudioSource.Play();
    }
  }
  void OnDeleted(object sender, DeletedEventArgs e)
  {
    Scoring(e.Lines);
    ChangeSpeed();
  }
  private void Scoring(int lines)
  {
    if (lines == 1) score += 40;
    else if (lines == 2) score += 100;
    else if (lines == 3) score += 300;
    else score += lines * 300;
    ScoreText.text = score.ToString();
  }
  private void ChangeSpeed()
  {
    double interval = (DateTime.Now - startTime).TotalSeconds;

    if (score < 300) frame = StartingFrameRate;
    else if (score >= 20000 || interval >= 480) frame = (int)(StartingFrameRate * 0.1);
    else if (score >= 8000 || interval >= 300) frame = (int)(StartingFrameRate * 0.2);
    else if (score >= 5000 || interval >= 210) frame = (int)(StartingFrameRate * 0.3);
    else if (score >= 3000 || interval >= 120) frame = (int)(StartingFrameRate * 0.5);
    else if (score >= 1500 || interval >= 60) frame = (int)(StartingFrameRate * 0.7);
    else if (score >= 500 || interval >= 30) frame = (int)(StartingFrameRate * 0.9);
    if (board.DropFrame != frame)
    {
      Debug.Log("change speed: " + frame);
      board.DropFrame = frame;
    }
  }
  void OnGameOver(object sender, EventArgs e)
  {
    bgmAudioSource.Stop();
    gameOver.Activate();
  }
  void OnRestart(object sender, EventArgs e)
  {
    frame = StartingFrameRate;
    board.Reset(frame);
    score = 0;
    ScoreText.text = score.ToString();
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

