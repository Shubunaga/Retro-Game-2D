using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFocusCamera : MonoBehaviour
{
    public Transform characterPosition; //the character to get focused
    public float focusTime = 1.0f; //time do focus on target
    public float returnTime = 1.0f; //time to return do original position
    public float smoothSpeed = 7f; //smoothnes of camera movement
    
    public float cameraZoom = 6.0f; //usar no size

    private Vector2 originalPosition;
    private bool focusing = false;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !focusing) // Example: Press "F" to focus
        {
            focusing = true;
            StartCoroutine(FocusCo());
        }
    }

    IEnumerator FocusCo()
    {
        Vector3 targetPosition = characterPosition.position;
        while(Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            //gameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 
            yield return null;
        }

        yield return new WaitForSeconds(focusTime);

        float elapsedTime = 0;
        while(elapsedTime < returnTime) 
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, smoothSpeed * Time.deltaTime);
            yield return null;
        }
        focusing = false;
    }
}
