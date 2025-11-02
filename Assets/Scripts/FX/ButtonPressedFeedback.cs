using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonPressedFeedback : MonoBehaviour, IPointerDownHandler
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.Play("Button_Pressed");
    }
}
