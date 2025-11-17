using UnityEngine;

namespace Events.UnitEvent
{
    public struct UnitTilePosEvent : IEvent
    {
        public Vector2Int TilePos;
    }

    public struct UnitEvent : IEvent
    {
        public IGameUnit Unit;

        public UnitEvent(IGameUnit unit) { Unit = unit; }
    }

    public struct UnitMoveRequestedEvent : IEvent
    {
        public IGameUnit UnitToMove { get; }
        public Tile TargetTile { get; }
        public UnitMoveRequestedEvent(IGameUnit unit, Tile tile)
        {
            UnitToMove = unit;
            TargetTile = tile;
        }
    }

    public struct UnitMoveCommittedEvent : IEvent
    {
        public MoveActionData ActionData { get; }
        public UnitMoveCommittedEvent(MoveActionData actionData) => ActionData = actionData;
    }

}