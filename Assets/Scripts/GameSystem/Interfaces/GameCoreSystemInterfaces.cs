using UnityEngine;

public interface ITile
{
    Vector2Int Pos { get; set; }
}

public interface IGameUnit
{
    Vector2Int CurPos { get; set; }
}
