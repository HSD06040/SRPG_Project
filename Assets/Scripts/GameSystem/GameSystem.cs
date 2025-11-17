using SRPG.ActionData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : IDisposable
{
    private readonly MapSystem mapSystem;
    private readonly EventBinding<UnitMoveRequestedEvent> moveRequestedBinding;

    public GameSystem(MapSystem mapSystem)
    {
        this.mapSystem = mapSystem;

        moveRequestedBinding = new EventBinding<UnitMoveRequestedEvent>();
        EventBinding();
    }

    private void EventBinding()
    {
        moveRequestedBinding.Add(OnMoveRequested);
        EventBus<UnitMoveRequestedEvent>.Register(moveRequestedBinding);
    }

    private void OnMoveRequested(UnitMoveRequestedEvent moveEvent)
    {
        var unitToMove = moveEvent.UnitToMove;
        var targetTile = moveEvent.TargetTile;

        if (unitToMove == null || targetTile == null || !IsValidMove(unitToMove, targetTile))
        {
            Debug.LogWarning("Invalid move request or unit/tile is null.");
            return;
        }

        var actionData = ExecuteMove(unitToMove, targetTile);

        EventBus<UnitMoveCommittedEvent>.Raise(new UnitMoveCommittedEvent(actionData));
    }

    /// <summary>
    /// 실제 게임 상태를 변경하고 ActionData를 생성합니다.
    /// </summary>
    private MoveActionData ExecuteMove(IGameUnit unit, Tile tile)
    {
        Vector2Int before = unit.CurPos;
        Vector2Int after = tile.Pos;

        unit.CurPos = after;

        return new MoveActionData(unit, before, after);
    }

    private bool IsValidMove(IGameUnit unit, Tile tile)
    {
        List<ITile> moveableTiles = mapSystem.CalculateMoveableTiles(unit);

        return moveableTiles.Contains(tile);
    }

    public void Dispose()
    {
        EventBus<UnitMoveRequestedEvent>.Deregister(moveRequestedBinding);
    }
}
