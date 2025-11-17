using System.Collections.Generic;

namespace Events.MapEvent
{
    public struct TileHighlightRequestedEvent : IEvent
    {
        public List<ITile> TilesToHighlight { get; }
        public TileHighlightRequestedEvent(List<ITile> tiles) => TilesToHighlight = tiles;
    }

    public struct TileHighlightClearEvent : IEvent { }
}