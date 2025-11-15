using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    [SerializeField]
    private MapSystem mapSystem {  get; set; }

    [SerializedDictionary("ÁÂÇ¥", "À¯´Ö")]
    private SerializedDictionary<Vector2Int, IGameUnit> unitMap;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        unitMap = new();
        this.mapSystem = mapSystem = new();
    }

#region Test
    [SerializeField] MapData testMapData;
    [ContextMenu("MapGenerate")]
    private void TestMapGenerate()
    {
        mapSystem.MapGenerate(testMapData);
    }

    [ContextMenu("DeHightlight Tile")]
    private void DeHieghtlightTile()
    {
        mapSystem.DeHighlightTiles();
    }
#endregion

    public void CheckCanMove(IGameUnit unit)
    {       
        mapSystem.GetVisibleTile(unit);
    }
}
