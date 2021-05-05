using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  public Delete del;
  public AudioClip soundDrop;
  public AudioClip soundDelete;
  public AudioSource audioSource;
  public event EventHandler GameOverEvent;
  public event EventHandler<DeletedEventArgs> DeletedEvent;
  internal Cell[,] cells;
  internal int minX = 1, maxX, minY = 1, maxY;
  Status s = new Status();
  Next next = new Next();
  bool insert;
  int frame;
  int dropFrame = 60;
  int fastFrame;
  internal int DropFrame
  {
    set { this.dropFrame = value; }
    get { return this.dropFrame; }
  }
  internal void Init(Cells c)
  {
    fastFrame = dropFrame / 2;
    cells = c.main;
    maxX = cells.GetLength(0) - 1;
    maxY = cells.GetLength(1) - 2;
    del.Init(this);
    del.DeletedEvent += DeletedEvent;
    next.Init(c.next);
    insert = false;
    frame = 0;
    Next();
  }
  internal void Reset()
  {
    insert = false;
    frame = 0;
    next.Hide();
    next.Reset();
    DeleteAll();

    Next();
    gameObject.SetActive(false);
    Render();
  }
  void Update()
  {
    frame++;
    HandleInput();
    if (frame >= dropFrame)
    {
      Drop();
    }
    Render();
  }
  internal void HandleInput()
  {
    Key.Handle();
    if (Key.PressingDown())
    {
      if (!insert) frame += fastFrame;
    }
    else
    {
      insert = false;
    }
    if (Key.Left())
    {
      Move(-1, 0);
    }
    else if (Key.Right())
    {
      Move(1, 0);
    }
    else if (Key.Rotate())
    {
      Rotate();
    }
  }
  void Insert()
  {
    s.XY(5, 20); // first place
    if (s.id == Blocks.i) s.y++;
    s.ResetRotate();
    insert = true;
    //-> check collision
    Point[] r = Blocks.Relatives(s);
    if (!IsEmpty(s.x, s.y, r))
    {
      gameObject.SetActive(false);
      GameOverEvent(this, null);
    }
  }
  void Fix()
  {
    cells[s.x, s.y].id = s.id;
    Point[] r = Blocks.Relatives(s);
    int cx, cy;
    for (int i = 0; i < r.Length; i++)
    {
      cx = r[i].x; cy = r[i].y;
      cells[s.x + cx, s.y + cy].id = s.id;
    }
  }
  void Hide()
  {
    cells[s.x, s.y].id = Blocks.empty;
    Point[] r = Blocks.Relatives(s);
    int cx, cy;
    for (int i = 0; i < r.Length; i++)
    {
      cx = r[i].x; cy = r[i].y;
      cells[s.x + cx, s.y + cy].id = Blocks.empty;
    }
  }
  bool IsEmpty(int x, int y, Point[] r)
  {
    int b = cells[x, y].id;
    if (b != Blocks.empty) return false;
    int rX, rY;
    for (int i = 0; i < r.Length; i++)
    {
      rX = r[i].x; rY = r[i].y;
      b = cells[x + rX, y + rY].id;
      if (b != Blocks.empty)
      {
        return false;
      }
    }
    return true;
  }
  internal bool Move(int x, int y)
  {
    Hide();
    int nx = s.x + x;
    int ny = s.y + y;
    Point[] r = Blocks.Relatives(s);
    bool move = IsEmpty(nx, ny, r);
    if (move) { s.x = nx; s.y = ny; }
    Fix();
    return move;
  }

  internal void Rotate()
  {
    if (s.id == Blocks.o) return; // none
    Hide();
    int cr = s.rotate;
    s.Rotate();
    Point[] r = Blocks.Relatives(s);
    if (!IsEmpty(s.x, s.y, r))
    {
      s.rotate = cr; // rollback
    }
    Fix();
  }
  internal void Next()
  {
    s.id = next.Id();
    Insert();
    Fix();
  }
  internal void Drop()
  {
    frame = 0;
    if (Move(0, -1)) return;
    //-> dropped. no space to move.
    if (del.Check())
    {
      audioSource.PlayOneShot(soundDelete);
    }
    else
    {
      audioSource.PlayOneShot(soundDrop);
    }
  }
  internal void Render()
  {
    for (int y = minY; y < maxY; y++)
    {
      for (int x = minX; x < maxX; x++)
      {
        cells[x, y].Render();
      }
    }
  }
  void DeleteAll()
  {
    for (int y = minY; y < maxY; y++)
    {
      for (int x = minX; x < maxX; x++)
      {
        cells[x, y].id = Blocks.empty;
      }
    }
  }
}
