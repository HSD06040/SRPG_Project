using UnityEngine;

public class Tile : MonoBehaviour, ITile
{
    public Vector2Int Pos { get => _pos; set => _pos = value; }
    [SerializeField] Vector2Int _pos;
    public TileData Data { get => _data; set => _data = value; }
    [SerializeField] private TileData _data;

    [SerializeField] GameObject _view;

    public void HighlightTile()
    {
        _view.SetActive(true);
    }

    public void DeHighlightTile()
    {
        _view.SetActive(false);
    }    

    public void SetTilePos(Vector2Int pos) => Pos = pos;    
}
