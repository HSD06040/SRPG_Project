using System;
using UnityEngine;

[Serializable]
public class TileData
{
    public bool isWall { get; set; }
    public bool isWakable { get; set; } = true;
}
