
using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingInteraction : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WorldGrid worldGrid;
    [SerializeField] private float targetDistance = 1.05f;
    [SerializeField] private PlayerInventory playerInventory;

    public void PerformAction()
    {
        if (playerMovement == null || worldGrid == null)
        {
            Debug.LogError(
                "Assign PlayerMovement and worldGrid in the Inspector."
            );
            return;
        }

        Vector3 direction = playerMovement.FacingDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            Debug.LogWarning("Move first to choose a facing direction.");
            return;
        }

        direction.Normalize();

        Vector3 targetPosition =
            transform.position + direction * targetDistance;

        if (worldGrid.WorldToGrid(targetPosition, out int x, out int z))
        {
            FarmCell cell = worldGrid.GetCell(x, z);

            if (cell != null)
            {
                if (!cell.IsTilled)
                {
                    cell.Till();
                    Debug.Log("Soil tilled.");
                }
                else if (!cell.HasCrop)
                {
                    cell.Plant();
                    Debug.Log("Seed planted.");
                }
                else if (cell.GrowthStage >= 2)
                {
                    if (cell.Harvest())
                    {
                        if (playerInventory != null)
                        {
                            playerInventory.AddCrops(1);
                        }
                        else
                        {
                            Debug.LogError(
                                "PlayerInventory is not assigned in the Inspector."
                            );
                        }

                        Debug.Log("Harvest successful!");
                    }
                }
                else
                {
                    Debug.Log("Crop is still growing.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Target position is outside the farm grid.");
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PerformAction();
        }
#endif
    }
}
