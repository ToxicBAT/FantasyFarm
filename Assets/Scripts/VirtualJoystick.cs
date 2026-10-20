
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform baseRect;
    [SerializeField] private RectTransform handleRect;
    [SerializeField, Range(0.1f, 1f)]
    private float handleRange = 0.4f;

    public Vector2 Direction { get; private set; }

    private Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        if (baseRect == null)
            baseRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (baseRect == null || handleRect == null)
            return;

        Camera eventCamera =
            parentCanvas != null &&
            parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? parentCanvas.worldCamera
            : null;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            baseRect, eventData.position, eventCamera,
            out Vector2 localPoint))
            return;

        Vector2 halfSize = baseRect.rect.size * 0.5f;

        Direction = new Vector2(
            halfSize.x > 0 ? localPoint.x / halfSize.x : 0,
            halfSize.y > 0 ? localPoint.y / halfSize.y : 0
        );

        Direction = Vector2.ClampMagnitude(Direction, 1f);

        handleRect.anchoredPosition = new Vector2(
            Direction.x * halfSize.x * handleRange,
            Direction.y * halfSize.y * handleRange
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;

        if (handleRect != null)
            handleRect.anchoredPosition = Vector2.zero;
    }
}
