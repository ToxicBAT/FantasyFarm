
using UnityEngine;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float interactionDistance = 1.5f;
    [SerializeField] private LayerMask farmCellLayer = ~0;


    public void PerformAction()
    {
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement is not assigned!");
            return;
        }

        Vector3 direction = playerMovement.FacingDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            Debug.LogWarning("Move first to face a direction.");
            return;
        }

        direction.Normalize();

        Vector3 origin = transform.position + Vector3.up * 0.5f;

        Debug.DrawRay(origin, direction * 2f, Color.red, 3f);

        if (Physics.SphereCast(
            origin,
            0.45f,
            direction,
            out RaycastHit hit,
            2f,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Target detected: " + hit.collider.name);

            FarmCell cell = hit.collider.GetComponentInParent<FarmCell>();

            if (cell != null)
            {
                cell.Till();
                Debug.Log("Tilled cell: " + cell.name);
            }
            else
            {
                Debug.LogWarning("Target is not a FarmCell.");
            }
        }
        else
        {
            Debug.LogWarning("Sphere cast found no target.");
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
