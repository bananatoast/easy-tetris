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
  private int frame;
  private static int deleteFrame = 30;
  private List<int> deletable;
  private Cell[][] cells;

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
      Cell[] row = cells[y];
      for (int x = 1; x < row.Length - 1; x++)
      {
        row[x].AddAlpha(-0.03f);
      }
    }
  }
  void Complete()
  {
    deletable.Sort();
    deletable.Reverse();
    foreach (int r in deletable)
    {
      Cell[] row = cells[r];
      for (int x = 1; x < row.Length - 1; x++)
      {
        row[x].State = State.Empty;
      }
      for (int y = r; y < cells.Length - 1; y++)
      {
        for (int x = 1; x < cells[y].Length - 1; x++)
        {
          cells[y][x].State = cells[y + 1][x].State;
        }
      }
    }
    DeletedEvent(this, new DeletedEventArgs(deletable.Count));
    gameObject.SetActive(false);
  }
  internal void StartDeleting()
  {
    frame = 0;
    gameObject.SetActive(true);
  }
  internal bool TryDeleting(Cell[][] cells)
  {
    var deletable = new List<int>();
    for (int y = 1; y < cells.Length; y++)
    {
      Cell[] row = cells[y];
      bool filled = true;
      for (int x = 1; x < row.Length - 1; x++)
      {
        filled = filled && (row[x].State != State.Empty);
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
