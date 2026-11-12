
using UnityEngine;

public class ConstructPlacement
{
    private readonly WorldGrid worldGrid;

    public ConstructPlacement(WorldGrid worldGrid)
    {
        this.worldGrid = worldGrid;
    }

    // Multi-cell placement with automatic visual alignment.
    public bool TryPlace(
        GameObject constructPrefab,
        ConstructFootprint footprint,
        Vector2Int anchor,
        int rotationDegrees,
        float verticalOffset,
        out GameObject placedConstruct)
    {
        placedConstruct = null;

        if (worldGrid == null ||
            !worldGrid.IsInitialized ||
            constructPrefab == null ||
            footprint == null ||
            !footprint.IsValid)
        {
            return false;
        }

        Vector2 centerOffset =
            footprint.GetVisualCenterOffset(rotationDegrees);

        Vector3 localOffset = new Vector3(
            centerOffset.x * worldGrid.CellSize,
            0f,
            centerOffset.y * worldGrid.CellSize
        );

        Vector3 visualOffset =
            worldGrid.transform.TransformVector(localOffset);

        Vector3 position =
            worldGrid.GridToWorld(anchor.x, anchor.y)
            + visualOffset
            + worldGrid.transform.up * verticalOffset;

        Quaternion rotation =
            worldGrid.transform.rotation *
            Quaternion.Euler(0f, rotationDegrees, 0f);

        return TryPlaceInternal(
            constructPrefab,
            footprint,
            anchor,
            rotationDegrees,
            position,
            rotation,
            out placedConstruct
        );
    }

    // Validates and registers the construct.
    private bool TryPlaceInternal(
        GameObject constructPrefab,
        ConstructFootprint footprint,
        Vector2Int anchor,
        int rotationDegrees,
        Vector3 position,
        Quaternion rotation,
        out GameObject placedConstruct)
    {
        placedConstruct = null;

        if (worldGrid == null ||
            !worldGrid.IsInitialized ||
            constructPrefab == null ||
            footprint == null ||
            !footprint.IsValid)
        {
            return false;
        }

        Vector2Int[] occupiedCoordinates =
            footprint.GetOccupiedCells(anchor, rotationDegrees);

        if (occupiedCoordinates.Length == 0)
            return false;

        FarmCell[] occupiedCells =
            new FarmCell[occupiedCoordinates.Length];

        // Validate every cell before instantiating.
        for (int i = 0; i < occupiedCoordinates.Length; i++)
        {
            Vector2Int coordinate = occupiedCoordinates[i];

            if (!worldGrid.IsInsideBounds(
                coordinate.x,
                coordinate.y))
            {
                Debug.Log(
                    "Placement failed: Structure extends outside the grid."
                );
                return false;
            }

            FarmCell cell = worldGrid.GetCell(
                coordinate.x,
                coordinate.y
            );

            if (cell == null || cell.IsOccupied)
            {
                Debug.Log(
                    "Placement failed: A footprint cell is unavailable."
                );
                return false;
            }

            occupiedCells[i] = cell;
        }

        // Instantiate the structure once.
        GameObject instance = Object.Instantiate(
            constructPrefab,
            position,
            rotation,
            worldGrid.transform
        );

        // Register the same instance in every occupied cell.
        int registeredCount = 0;

        for (int i = 0; i < occupiedCells.Length; i++)
        {
            if (!occupiedCells[i].TryPlaceConstruct(instance))
            {
                // Roll back any registrations already completed.
                for (int j = 0; j < registeredCount; j++)
                {
                    occupiedCells[j].RemoveConstruct(instance);
                }

                Object.Destroy(instance);

                Debug.LogError(
                    "Placement failed: Occupancy registration failed."
                );

                return false;
            }

            registeredCount++;
        }

        placedConstruct = instance;

        Debug.Log(
            $"Construct placed: {constructPrefab.name}. " +
            $"Occupied cells: {registeredCount}"
        );

        return true;
    }
}
