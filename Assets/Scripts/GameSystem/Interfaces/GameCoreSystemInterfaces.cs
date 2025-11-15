using UnityEngine;

public interface ITile : IHighlight
{
    Vector2Int Pos { get; set; }
    TileData Data { get; set; }
    
    void SetTilePos(Vector2Int pos);
}

public interface IHighlight
{
    void HighlightTile();
    void DeHighlightTile();
}

public interface IGameUnit
{
    Vector2Int CurPos { get; set; }
    UnitData UnitData { get; set; }
}
