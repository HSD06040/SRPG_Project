using System.Collections.Generic;

namespace Events.MapEvent
{
    public record struct TileHighlightRequestedEvent(List<ITile> TilesToHighlight) : IEvent;    

    public struct TileHighlightClearEvent : IEvent { }
}