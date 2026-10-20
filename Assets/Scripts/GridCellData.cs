
using System;

public enum GridTerrainType
{
    Soil,
    Grass,
    Water,
    Stone
}

[Serializable]
public class GridCellData
{
    public int X { get; }
    public int Z { get; }

    public bool IsUnlocked { get; private set; }
    public GridTerrainType TerrainType { get; private set; }

    public string OccupantId { get; private set; }

    public bool IsOccupied => !string.IsNullOrEmpty(OccupantId);

    public GridCellData(
        int x,
        int z,
        bool isUnlocked = true,
        GridTerrainType terrainType = GridTerrainType.Soil)
    {
        X = x;
        Z = z;
        IsUnlocked = isUnlocked;
        TerrainType = terrainType;
        OccupantId = string.Empty;
    }

    public void SetUnlocked(bool unlocked)
    {
        IsUnlocked = unlocked;
    }

    public void SetTerrain(GridTerrainType terrainType)
    {
        TerrainType = terrainType;
    }

    public bool TrySetOccupant(string occupantId)
    {
        if (string.IsNullOrWhiteSpace(occupantId) || IsOccupied)
            return false;

        OccupantId = occupantId;
        return true;
    }

    public bool ClearOccupant(string occupantId)
    {
        if (!IsOccupied || OccupantId != occupantId)
            return false;

        OccupantId = string.Empty;
        return true;
    }
}
