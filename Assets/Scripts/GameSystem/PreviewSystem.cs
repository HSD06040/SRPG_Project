public class PreviewSystem
{
    public IGameUnit SelectUnit { get; private set; }
    public Tile SelectTile { get; private set; }

    public void UnitSelect(IGameUnit unit) => SelectUnit = unit;
    public void TileSelect(Tile tile) => SelectTile = tile;

    public void PreviewMove()
    {
        SelectUnit.MoveVisual(SelectTile.Pos);
    }

    public void CancelPreview()
    {
        SelectUnit.MoveVisual(SelectUnit.CurPos);
        SelectUnit = null;
        SelectTile = null;
    }
}
