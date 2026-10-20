
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory")]
    public int CropCount { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text cropCountText;

    private void Start()
    {
        UpdateCropCountDisplay();
    }

    public void AddCrops(int amount)
    {
        if (amount <= 0)
            return;

        CropCount += amount;
        UpdateCropCountDisplay();

        Debug.Log($"Crops in inventory: {CropCount}");
    }

    private void UpdateCropCountDisplay()
    {
        if (cropCountText != null)
        {
            cropCountText.text = $"Crops: {CropCount}";
        }
        else
        {
            Debug.LogWarning("Crop count text is not assigned.");
        }
    }
}
