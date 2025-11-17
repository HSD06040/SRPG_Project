using Events.InputEvent;

public class PreviewSystem
{
    public IGameUnit SelectUnit { get; private set; }
    public Tile SelectTile { get; private set; }

#region Events
    readonly EventBinding<TileSelectEvent> tileSelectBinding;
    readonly EventBinding<UnitSelectEvent> unitSelectBinding;

    private void EventBinding()
    {        
        tileSelectBinding.Add();
        EventBus<TileSelectEvent>.Register(tileSelectBinding);

        unitSelectBinding.Add();
        EventBus<UnitSelectEvent>.Register(unitSelectBinding);
    }
#endregion

    public PreviewSystem()
    {
        tileSelectBinding = new EventBinding<TileSelectEvent>();
        unitSelectBinding = new EventBinding<UnitSelectEvent>();
    }

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
