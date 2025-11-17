using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IGameUnit
{
    public Vector2Int CurPos { get => _curPos; set { _curPos = value; EventBus<UnitEvent>.Raise(new UnitEvent(this)); } }
    [SerializeField] Vector2Int _curPos;

    [field: SerializeField] public UnitData UnitData { get; set; }

    public Transform Transform { get => transform; }

    public static EventBinding<UnitEvent> UnitEvent = new();

    public void MoveVisual(Vector2Int tilePos)
    {
        // 움직임 알고리즘
    }
}