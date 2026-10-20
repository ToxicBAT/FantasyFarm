
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class FarmCell : MonoBehaviour
{
    [Header("Soil")]
    [SerializeField] private Renderer cellRenderer;
    [SerializeField] private Material untilledMaterial;
    [SerializeField] private Material tilledMaterial;

    [Header("Crop Prefabs")]
    [SerializeField] private GameObject seedlingPrefab;
    [SerializeField] private GameObject smallPlantPrefab;
    [SerializeField] private GameObject matureCropPrefab;

    [Header("Growth Timing")]
    [SerializeField] private float stageDuration = 30f;

    public bool IsTilled { get; private set; }
    public bool HasCrop { get; private set; }
    public int GrowthStage { get; private set; }

    private GameObject cropVisual;
    private float growthTimer;

    private void Awake()
    {
        if (cellRenderer == null)
            cellRenderer = GetComponent<Renderer>();

        RefreshSoilAppearance();
    }

    public void Till()
    {
        if (IsTilled)
            return;

        IsTilled = true;
        RefreshSoilAppearance();
    }

    public void Plant()
    {
        if (!IsTilled || HasCrop)
            return;

        if (seedlingPrefab == null ||
            smallPlantPrefab == null ||
            matureCropPrefab == null)
        {
            Debug.LogError(
                "Assign all three crop prefabs to FarmCell."
            );
            return;
        }

        HasCrop = true;
        GrowthStage = 0;
        growthTimer = 0f;
        ShowCropStage();
    }

    public bool Harvest()
    {
        if (!HasCrop || GrowthStage < 2)
            return false;

        if (cropVisual != null)
            Destroy(cropVisual);

        cropVisual = null;
        HasCrop = false;
        GrowthStage = 0;
        growthTimer = 0f;

        IsTilled = false;
        RefreshSoilAppearance();

        Debug.Log("Crop harvested! Soil returned to untilled state.");
        return true;
    }

    private void Update()
    {
        if (!HasCrop || GrowthStage >= 2)
            return;

        growthTimer += Time.deltaTime;

        if (growthTimer >= stageDuration)
        {
            growthTimer -= stageDuration;
            GrowthStage++;
            ShowCropStage();
        }
    }

    private void ShowCropStage()
    {
        if (cropVisual != null)
            Destroy(cropVisual);

        GameObject prefab = GrowthStage switch
        {
            0 => seedlingPrefab,
            1 => smallPlantPrefab,
            _ => matureCropPrefab
        };

        Vector3 position = transform.position
            + Vector3.up * 0.15f;

        cropVisual = Instantiate(
            prefab,
            position,
            Quaternion.identity,
            transform
        );
    }

    private void RefreshSoilAppearance()
    {
        if (cellRenderer != null)
        {
            cellRenderer.sharedMaterial =
                IsTilled ? tilledMaterial : untilledMaterial;
        }
    }
}
