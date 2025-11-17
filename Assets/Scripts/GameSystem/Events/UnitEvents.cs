using UnityEngine;

namespace Events.UnitEvent
{    
    public record struct UnitEvent(IGameUnit Unit) : IEvent;    

    public record struct UnitMoveRequestedEvent(IGameUnit UnitToMove, Tile TargetTile) : IEvent;

    public record struct UnitMoveCommittedEvent(MoveActionData ActionData) : IEvent;
}