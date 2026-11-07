
using UnityEngine;

public class ConstructPlacement
{
    private readonly WorldGrid worldGrid;

    public ConstructPlacement(WorldGrid worldGrid)
    {
        this.worldGrid = worldGrid;
    }

    public bool TryPlace(
        GameObject constructPrefab,
        FarmCell targetCell,
        Vector3 position,
        Quaternion rotation,
        out GameObject placedConstruct)
    {
        placedConstruct = null;

        if (worldGrid == null ||
            !worldGrid.IsInitialized ||
            constructPrefab == null ||
            targetCell == null)
        {
            return false;
        }

        if (targetCell.IsOccupied)
        {
            Debug.Log("Placement failed: Cell is already occupied.");
            return false;
        }

        GameObject instance = Object.Instantiate(
            constructPrefab,
            position,
            rotation,
            worldGrid.transform
        );

        if (!targetCell.TryPlaceConstruct(instance))
        {
            Object.Destroy(instance);

            Debug.Log("Placement failed: Could not register construct.");
            return false;
        }

        placedConstruct = instance;

        Debug.Log(
            $"Construct placed successfully: {constructPrefab.name}"
        );

        return true;
    }
}

