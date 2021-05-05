using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  public Text txtScore;
  int score;
  internal void Reset()
  {
    score = 0;
    Render();
  }
  internal void Add(int lines)
  {
    if (lines == 1) score += 40;
    else if (lines == 2) score += 100;
    else if (lines == 3) score += 300;
    else if (lines == 4) score += 1200;
    Render();
  }
  void Render()
  {
    txtScore.text = score.ToString();
  }
}
