using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
  internal int id;
  SpriteRenderer renderer;
  Colors colors;
  internal Cell(int id, SpriteRenderer renderer, Colors colors)
  {
    this.id = id;
    this.renderer = renderer;
    this.colors = colors;
  }
  internal void AddAlpha(float alpha)
  {
    Color color = renderer.color;
    color.a = color.a + alpha;
    renderer.color = color;
  }
  internal void Render()
  {
    renderer.color = colors.Get(id);
  }
  internal void ToBackground()
  {
    renderer.color = colors.back;
  }
  internal void Color(Status status)
  {
    renderer.color = colors.Get(status.id);
  }
}
