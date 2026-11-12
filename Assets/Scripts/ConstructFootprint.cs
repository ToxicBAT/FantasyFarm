
using UnityEngine;

public class ConstructFootprint
{
    public int Width { get; }
    public int Depth { get; }

    public bool IsValid => Width > 0 && Depth > 0;

    public ConstructFootprint(int width, int depth)
    {
        if (width <= 0 || depth <= 0)
        {
            Debug.LogError(
                "ConstructFootprint: Width and depth must be positive."
            );

            Width = 0;
            Depth = 0;
            return;
        }

        Width = width;
        Depth = depth;
    }

    private static int NormalizeRotation(int rotation)
    {
        return ((rotation % 360) + 360) % 360;
    }

    public Vector2Int GetRotatedSize(int rotation)
    {
        switch (NormalizeRotation(rotation))
        {
            case 0:
            case 180:
                return new Vector2Int(Width, Depth);

            case 90:
            case 270:
                return new Vector2Int(Depth, Width);

            default:
                Debug.LogError(
                    "ConstructFootprint: Only cardinal rotations are supported."
                );
                return Vector2Int.zero;
        }
    }

    public Vector2 GetVisualCenterOffset(int rotation)
    {
        float halfWidth = (Width - 1) / 2f;
        float halfDepth = (Depth - 1) / 2f;

        switch (NormalizeRotation(rotation))
        {
            case 0:
                return new Vector2(halfWidth, halfDepth);

            case 90:
                return new Vector2(-halfDepth, halfWidth);

            case 180:
                return new Vector2(-halfWidth, -halfDepth);

            case 270:
                return new Vector2(halfDepth, -halfWidth);

            default:
                Debug.LogError(
                    "ConstructFootprint: Unsupported rotation."
                );
                return Vector2.zero;
        }
    }

    public Vector2Int[] GetOccupiedCells(
        Vector2Int anchor,
        int rotation)
    {
        int normalizedRotation = NormalizeRotation(rotation);
        Vector2Int size = GetRotatedSize(normalizedRotation);

        if (!IsValid || size == Vector2Int.zero)
            return new Vector2Int[0];

        Vector2Int[] occupiedCells =
            new Vector2Int[size.x * size.y];

        int index = 0;

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Depth; z++)
            {
                Vector2Int offset;

                switch (normalizedRotation)
                {
                    case 0:
                        offset = new Vector2Int(x, z);
                        break;

                    case 90:
                        offset = new Vector2Int(-z, x);
                        break;

                    case 180:
                        offset = new Vector2Int(-x, -z);
                        break;

                    case 270:
                        offset = new Vector2Int(z, -x);
                        break;

                    default:
                        return new Vector2Int[0];
                }

                occupiedCells[index] = anchor + offset;
                index++;
            }
        }

        return occupiedCells;
    }
}
