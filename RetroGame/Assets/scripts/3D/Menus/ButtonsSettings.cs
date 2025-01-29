using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    // Original size of the button
    private Vector3 originalScale;

    // Scale factor for button increase
    public float scaleFactor = 1.2f;
    public AudioSource pointerEnterSound;
    public AudioSource pointerSelect;

    void Start()
    {
        // Save the original scale of the button
        originalScale = transform.localScale;
    }
    public void OnSelect(BaseEventData eventData)
    {
        pointerSelect.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Increase the size of the button when the mouse enters
        transform.localScale = originalScale * scaleFactor;
        pointerEnterSound.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the size of the button when the mouse exits
        transform.localScale = originalScale;
    }
}
