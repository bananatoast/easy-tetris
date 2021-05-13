using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletedEventArgs : EventArgs
{
  public DeletedEventArgs(int lines)
  {
    Lines = lines;
  }
  public int Lines { get; set; }
}
public class DeleteBehaviour : MonoBehaviour
{
  public event EventHandler<DeletedEventArgs> DeletedEvent;
  public AudioClip soundDelete;
  public AudioSource audioSource;
  private int frame;
  private static int deleteFrame = 30;
  private List<int> deletable;
  private Cell[,] cells;

  internal void Update()
  {
    frame++;
    if (frame >= deleteFrame)
    {
      Complete();
    }
    else
    {
      Deleting();
    }
  }
  void Deleting()
  {
    foreach (int y in deletable)
    {
      for (int x = 1; x < Board.Width - 1; x++)
      {
        cells[x, y].AddAlpha(-0.03f);
      }
    }
  }
  void Complete()
  {
    deletable.Sort();
    deletable.Reverse();
    foreach (int r in deletable)
    {
      for (int x = 1; x < Board.Width - 1; x++)
      {
        cells[x, r].State = State.Empty;
      }
      for (int y = r; y < Board.Height - 1; y++)
      {
        for (int x = 1; x < Board.Width - 1; x++)
        {
          cells[x, y].State = cells[x, y + 1].State;
        }
      }
    }
    DeletedEvent(this, new DeletedEventArgs(deletable.Count));
    gameObject.SetActive(false);
  }
  internal void StartDeleting()
  {
    audioSource.PlayOneShot(soundDelete);
    frame = 0;
    gameObject.SetActive(true);
  }
  internal bool TryDeleting(Cell[,] cells)
  {
    var deletable = new List<int>();
    for (int y = 1; y < Board.Height; y++)
    {
      bool filled = true;
      for (int x = 1; x < Board.Width - 1; x++)
      {
        filled = filled && (cells[x, y].State != State.Empty);
      }
      if (filled)
      {
        deletable.Add(y);
      }
    }
    if (deletable.Count > 0)
    {
      this.deletable = deletable;
      this.cells = cells;
      return true;
    }
    return false;
  }
}
