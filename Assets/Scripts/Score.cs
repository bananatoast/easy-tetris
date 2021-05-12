using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  public Text ScoreText;
  private int score = 0;
  private int ScoreValue
  {
    get { return score; }
    set { score = value; ScoreText.text = score.ToString(); }
  }
  internal void Reset()
  {
    ScoreValue = 0;
  }
  internal void Add(int lines)
  {
    if (lines == 1) ScoreValue += 40;
    else if (lines == 2) ScoreValue += 100;
    else if (lines == 3) ScoreValue += 300;
    else if (lines == 4) ScoreValue += 1200;
  }
}
