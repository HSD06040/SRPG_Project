using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    #region Systems
    private MapSystem mapSystem;
    private MapVisualSystem mapVisualSystem;
    private GameSystem gameSystem;
    private GameUndoSystem undoSystem;
    #endregion

    [SerializedDictionary("ÁÂÇ¥", "À¯´Ö")]
    private SerializedDictionary<Vector2Int, IGameUnit> unitMap;

    private void Awake()
    {
        undoSystem = new GameUndoSystem();
        mapSystem = new MapSystem();
        mapVisualSystem = new MapVisualSystem();
        gameSystem = new GameSystem(mapSystem);

        Init();
    }

    #region Test
    
    #endregion

    public void Init()
    {
        unitMap = new();
    }

    public void CheckCanMove(IGameUnit unit)
    {
        mapSystem.GetVisibleTile(unit);
    }

    private void OnDestroy()
    {
        undoSystem.Dispose();
        mapSystem.Dispose();
        mapVisualSystem.Dispose();
        gameSystem.Dispose();
    }
}