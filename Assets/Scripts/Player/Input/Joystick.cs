using UnityEngine.EventSystems;
using UnityEngine;

public class Joystick : MonoBehaviour,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] RectTransform knobRT;
    RectTransform backgroundRT;

    Vector2 backgroundRTSize;

    public static Vector2 joystickInputAxis;

    public static bool dragActive;
    public static Vector2 dragAxis;
    public static bool dragging;

    void Awake()
    {
        dragging = false;
        dragAxis = Vector2.zero;
        dragActive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        joystickInputAxis = Vector2.zero;
        backgroundRT = GetComponent<RectTransform>();
        backgroundRTSize = new Vector2(backgroundRT.rect.size.x * CanvasScaleConstants.uiCanvasScale.x, backgroundRT.rect.size.y * CanvasScaleConstants.uiCanvasScale.x);
    }
    void Update()
    {
        if (!dragActive || !dragging)
            ResetJoystickPosition();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (dragActive)
        {
            //Debug.Log("Joystick Dragging");
            joystickInputAxis = new Vector2((eventData.position.x - backgroundRT.position.x) / (backgroundRTSize.x / 2),
                (eventData.position.y - backgroundRT.position.y) / (backgroundRTSize.y / 2));
            joystickInputAxis = Vector2.ClampMagnitude(joystickInputAxis, 1f);
            if (dragAxis != Vector2.zero)
            {
                float xNewInput;
                float yNewInput;
                if (dragAxis.x == -1)
                    xNewInput = Mathf.Clamp(joystickInputAxis.x, dragAxis.x, 0);
                else
                    xNewInput = Mathf.Clamp(joystickInputAxis.x, 0, dragAxis.x);
                if (dragAxis.y == -1)
                    yNewInput = Mathf.Clamp(joystickInputAxis.y, dragAxis.y, 0);
                else
                    yNewInput = Mathf.Clamp(joystickInputAxis.y, 0, dragAxis.y);

                joystickInputAxis = new Vector2(xNewInput, yNewInput);
            }
            knobRT.position = new Vector2(joystickInputAxis.x * backgroundRTSize.x / 2 + backgroundRT.position.x, joystickInputAxis.y * backgroundRTSize.y / 2 + backgroundRT.position.y);
        }
        //Debug.Log(joystickInputAxis.x);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetJoystickPosition();
    }

    public void ResetJoystickPosition()
    {
        knobRT.position = backgroundRT.position;
        joystickInputAxis = Vector2.zero;
    }

}
