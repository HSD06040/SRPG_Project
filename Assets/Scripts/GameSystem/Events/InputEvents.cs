namespace Events.InputEvent
{
    public record struct UnitSelectEvent(IGameUnit Unit) : IEvent;
    public record struct TileSelectEvent(ITile Tile) : IEvent;
}