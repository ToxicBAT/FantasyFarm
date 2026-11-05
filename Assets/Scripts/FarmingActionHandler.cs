
using UnityEngine;

public class FarmingActionHandler
{
    private readonly PlayerInventory playerInventory;

    public FarmingActionHandler(PlayerInventory playerInventory)
    {
        this.playerInventory = playerInventory;
    }

    public void PerformAction(FarmCell cell)
    {
        if (cell == null)
            return;

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

