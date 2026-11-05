using UnityEngine;

public class FarmTargeting
{
    private readonly PlayerMovement playerMovement;
    private readonly WorldGrid worldGrid;
    private readonly Transform origin;
    private readonly float targetDistance;

    public FarmTargeting(
        PlayerMovement playerMovement,
        WorldGrid worldGrid,
        Transform origin,
        float targetDistance)
    {
        this.playerMovement = playerMovement;
        this.worldGrid = worldGrid;
        this.origin = origin;
        this.targetDistance = targetDistance;
    }

    public bool TryGetTargetCell(
        out FarmCell cell,
        out int x,
        out int z)
    {
        cell = null;
        x = 0;
        z = 0;

        if (playerMovement == null ||
            worldGrid == null ||
            origin == null)
        {
            return false;
        }

        Vector3 direction = playerMovement.FacingDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return false;

        direction.Normalize();

        Vector3 targetPosition =
            origin.position + direction * targetDistance;

        if (!worldGrid.WorldToGrid(targetPosition, out x, out z))
            return false;

        cell = worldGrid.GetCell(x, z);

        return cell != null;
    }
}
