
public class PreviewSystem
{
    public IGameUnit SelectedUnit { get; private set; }
    public ITile SelectedTile { get; private set; }

    #region Events
    readonly EventBinding<TileSelectEvent> tileSelectBinding;
    readonly EventBinding<UnitSelectEvent> unitSelectBinding;

    private void EventBinding()
    {
        tileSelectBinding.Add(SelectTile);
        EventBus<TileSelectEvent>.Register(tileSelectBinding);

        unitSelectBinding.Add(SelectUnit);
        EventBus<UnitSelectEvent>.Register(unitSelectBinding);
    }

    void SelectTile(TileSelectEvent selectEvent) => SelectedTile = selectEvent.Tile;
    void SelectUnit(UnitSelectEvent selectEvent) => SelectedUnit = selectEvent.Unit;
    #endregion

    public PreviewSystem()
    {
        tileSelectBinding = new EventBinding<TileSelectEvent>();
        unitSelectBinding = new EventBinding<UnitSelectEvent>();

        EventBinding();
    }

    public void UnitSelect(IGameUnit unit) => SelectedUnit = unit;
    public void TileSelect(Tile tile) => SelectedTile = tile;

    public void PreviewMove()
    {
        SelectedUnit.MoveVisual(SelectedTile.Pos);
    }

    public void CancelPreview()
    {
        SelectedUnit.MoveVisual(SelectedUnit.CurPos);
        SelectedUnit = null;
        SelectedTile = null;
    }
}
