using UnityEngine;
using Events.UnitEvent;
using UnitEventBinding = EventBinding<Events.UnitEvent.UnitEvent>;

public abstract class UnitBase : MonoBehaviour, IGameUnit
{
    public Vector2Int CurPos { get => _curPos; set { _curPos = value; EventBus<UnitEvent>.Raise(new UnitEvent(this)); } }
    [SerializeField] Vector2Int _curPos;
    [field:SerializeField] public UnitData UnitData { get; set; }    

    public static UnitEventBinding UnitEvent = new UnitEventBinding();

    public void MoveVisual(Vector2Int tilePos)
    {
        // 움직임 알고리즘
    }
}