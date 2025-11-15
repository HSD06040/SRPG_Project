using UnityEngine;

public struct UnitTilePosEvent : IEvent
{
    public Vector2Int tilePos;
}

public abstract class UnitBase : MonoBehaviour, IGameUnit
{
    [field:SerializeField] public Vector2Int CurPos { get; set; }
    [field:SerializeField] public UnitData UnitData { get; set; }    

    public static EventBinding<UnitTilePosEvent> unitPosEvent;
    protected GameSystemManager gameSystemManager;

    public void Init(GameSystemManager gameSystemManager)
    {
        this.gameSystemManager = gameSystemManager;
        unitPosEvent = new EventBinding<UnitTilePosEvent>(CurrentPositionUpdate);
    }

    [ContextMenu("Test")]
    public void CheckCanMoveTile()
    {
        gameSystemManager.CheckCanMove(this);
    }

    private void CurrentPositionUpdate(UnitTilePosEvent tilePosEvent)
    {
        CurPos = tilePosEvent.tilePos;
    }
}