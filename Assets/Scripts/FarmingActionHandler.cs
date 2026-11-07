
using UnityEngine;

public class FarmingActionHandler
{
    private readonly PlayerInventory playerInventory;

    public FarmingActionHandler(PlayerInventory playerInventory)
    {
        this.playerInventory = playerInventory;
    }

    public void PerformAction(CropPlot plot)
    {
        if (plot == null)
        {
            Debug.Log("There is no crop plot at the targeted cell.");
            return;
        }

        if (!plot.IsTilled)
        {
            if (plot.Till())
                Debug.Log("Soil tilled.");
            else
                Debug.Log("Unable to till this plot.");
        }
        else if (!plot.HasCrop)
        {
            if (plot.Plant())
                Debug.Log("Seed planted.");
            else
                Debug.Log("Unable to plant on this plot.");
        }
        else if (plot.GrowthStage >= 2)
        {
            if (!plot.Harvest())
                return;

            if (playerInventory != null)
            {
                playerInventory.AddCrops(1);
                Debug.Log("Harvest successful!");
            }
            else
            {
                Debug.LogError(
                    "PlayerInventory is not assigned in the Inspector."
                );
            }
        }
        else
        {
            Debug.Log("Crop is still growing.");
        }
    }
}
