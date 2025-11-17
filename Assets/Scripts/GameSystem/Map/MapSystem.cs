using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Events.MapEvent;
using Events.UnitEvent;

[Serializable]
public class MapSystem : IDisposable
{
    private readonly Dictionary<IGameUnit, List<ITile>> moveableTileCache = new();
    private readonly EventBinding<UnitMoveCommittedEvent> commitBinding;

    [SerializedDictionary("좌표", "타일")]
    readonly SerializedDictionary<Vector2Int, ITile> tileMap = new();

    public MapSystem()
    {
        commitBinding = new EventBinding<UnitMoveCommittedEvent>();
        EventBinding();
    }

    public void MapGenerate(MapData mapData)
    {
        Transform tileParent = new GameObject("MapTiles").transform;
        GameObject tilePrefab = Resources.Load<GameObject>("Tile");

        int col = mapData.MapSize.x;
        int row = mapData.MapSize.y;

        for (int y = 1; y <= col; y++)
        {
            for (int x = 1; x <= row; x++)
            {
                Vector2Int tilePos = new Vector2Int(y, x);

                ITile tile = UnityEngine.Object.Instantiate
                    (tilePrefab, new Vector3(tilePos.x, 0, tilePos.y),
                    Quaternion.identity, tileParent).GetComponent<ITile>();

                tile.SetTilePos(tilePos);
                tileMap.Add(tilePos, tile);
            }
        }
    }

    private void EventBinding()
    {
        commitBinding.Add(OnUnitMoveCommitted);
        EventBus<UnitMoveCommittedEvent>.Register(commitBinding);
    }

    public List<ITile> GetMoveableTiles(IGameUnit unit)
    {
        if (moveableTileCache.TryGetValue(unit, out List<ITile> cachedTiles))
        {
            return cachedTiles;
        }

        List<ITile> calculatedTiles = CalculateMoveableTiles(unit);

        moveableTileCache[unit] = calculatedTiles;

        return calculatedTiles;
    }

    private void OnUnitMoveCommitted(UnitMoveCommittedEvent evt)
    {
        IGameUnit unit = evt.ActionData.Unit;
        moveableTileCache[unit] = CalculateMoveableTiles(unit);        
    }

    public List<ITile> CalculateMoveableTiles(IGameUnit unit)
    {
        if (!tileMap.TryGetValue(unit.CurPos, out ITile startTile))
        {
            Debug.LogError("유닛의 현재 위치에 타일이 없습니다.");
            return new List<ITile>();
        }

        int movement = unit.UnitData.StatData.Movement;
        List<ITile> moveableTiles = new List<ITile>();

        Queue<(ITile tile, int distance)> queue = new();
        Dictionary<ITile, int> visited = new();

        queue.Enqueue((startTile, 0));
        visited.Add(startTile, 0);

        while (queue.Count > 0)
        {
            var (currentTile, currentDist) = queue.Dequeue();

            if (currentDist >= movement) continue;

            Vector2Int curPos = currentTile.Pos;
            Vector2Int[] neighbors = new Vector2Int[]
            {
                curPos + Vector2Int.up,
                curPos + Vector2Int.down,
                curPos + Vector2Int.left,
                curPos + Vector2Int.right,
            };

            foreach (Vector2Int neighborPos in neighbors)
            {
                if (tileMap.TryGetValue(neighborPos, out ITile neighborTile))
                {
                    if (visited.ContainsKey(neighborTile) || !CanMoveTo(neighborTile, unit))
                    {
                        continue;
                    }

                    int nextDist = currentDist + 1;

                    if (nextDist <= movement)
                    {
                        visited.Add(neighborTile, nextDist);
                        moveableTiles.Add(neighborTile);
                        queue.Enqueue((neighborTile, nextDist));
                    }
                }
            }
        }

        return moveableTiles;
    }

    public List<ITile> GetVisibleTile(IGameUnit unit)
    {
        EventBus<TileHighlightClearEvent>.Raise(new TileHighlightClearEvent());

        List<ITile> tiles = GetMoveableTiles(unit);

        EventBus<TileHighlightRequestedEvent>.Raise(new TileHighlightRequestedEvent(tiles));

        return tiles;
    }

    private bool CanMoveTo(ITile tile, IGameUnit unit)
    {
        if (tile.Data.isWall)
            return false;

        if (!tile.Data.isWakable)
        {
            return unit.UnitData.isFly;
        }  

        return true;
    }

    public void Dispose()
    {
        EventBus<UnitMoveCommittedEvent>.Deregister(commitBinding);
    }
}