using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State : int
{
  Empty = 0,
  Wall = 1,
  I = 2,
  O = 3,
  S = 4,
  Z = 5,
  J = 6,
  L = 7,
  T = 8
}
class Colors
{
  internal static Color Get(State state)
  {
    return values[(int)state];
  }
  internal static Color Get(BlockType type)
  {
    return values[(int)type];
  }
  private static Color[] values = {
    new Color(0.1254902f, 0.1254902f, 0.1254902f, 1.0f),  //Empty
    new Color(0.6226415f, 0.6226415f, 0.6226415f, 1.0f),  //Wall
    new Color(0.0117f, 0.6627f, 0.9568f, 1.0f), //I
    new Color(1.0f, 0.8470f, 0.2313f, 1.0f),  //O
    new Color(0.2980f, 0.6862f, 0.3137f, 1.0f), //S
    new Color(0.9568f, 0.2627f, 0.2117f, 1.0f), //Z
    new Color(0.2470f, 0.3176f, 0.7098f, 1.0f), //J
    new Color(1.0f, 0.5960f, 1.0f, 1.0f), //L
    new Color(0.7098f, 0.2392f, 0.7686f, 1.0f)  //T
  };
}
class Cell
{
  private State state;
  internal State State
  {
    get { return state; }
    set { state = value; renderer.color = Colors.Get(value); }
  }
  private Vector2 position;
  internal Vector2 Position
  {
    get { return position; }
    set { position = value; renderer.transform.position = value; }
  }
  private SpriteRenderer renderer;
  internal GameObject GameObject { get; }
  internal Cell(State state, GameObject obj, Vector2 position)
  {
    GameObject = obj;
    renderer = obj.GetComponent<SpriteRenderer>();
    Position = position;
    State = state;
  }
  internal void AddAlpha(float alpha)
  {
    Color color = renderer.color;
    color.a = color.a + alpha;
    renderer.color = color;
  }
  internal void Clean()
  {
    renderer.color = Colors.Get(State.Empty);
    State = State.Empty;
  }
}
