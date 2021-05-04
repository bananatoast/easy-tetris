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
public class Delete : MonoBehaviour
{
  public event EventHandler<DeletedEventArgs> DeletedEvent;
  Board b;
  int frm;
  int delete = 30;
  List<int> lines = new List<int>();
  internal void Init(Board board)
  {
    b = board;
    frm = 0;
  }
  void Update()
  {
    frm++;
    if (frm == delete) Complete();
    else Deleting();
  }
  void Deleting()
  {
    foreach (int y in lines)
    {
      for (int x = b.minX; x < b.maxX; x++)
      {
        b.cells[x, y].AddAlpha(-0.03f);
      }
    }
  }
  void Complete()
  {
    int count = lines.Count;
    for (int i = 0; i < count; i++)
    {
      for (int y = lines[i] - i; y < b.maxY; y++)
      {
        for (int x = b.minX; x < b.maxX; x++)
        {
          b.cells[x, y].id = b.cells[x, y + 1].id;
        }
      }
    }
    frm = 0;
    lines.Clear();
    gameObject.SetActive(false);

    DeletedEvent(this, new DeletedEventArgs(count));

    b.gameObject.SetActive(true);
    b.Next();
  }
  internal void All()
  {
    for (int y = b.minY; y < b.maxY; y++)
    {
      for (int x = b.minX; x < b.maxX; x++)
      {
        b.cells[x, y].id = Blocks.empty;
      }
    }
  }
  void Enable()
  {
    gameObject.SetActive(true);
    b.gameObject.SetActive(false);
  }
  internal bool Check()
  {
    for (int y = b.minY; y < b.maxY; y++)
    {
      for (int x = b.minX; x < b.maxX; x++)
      {
        if (b.cells[x, y].id == Blocks.empty) break;
        if (x == 10) lines.Add(y);
      }
    }
    if (lines.Count == 0)
    {
      b.Next();
      return false;
    }
    else
    {
      Enable();
      return true;
    }
  }
}
