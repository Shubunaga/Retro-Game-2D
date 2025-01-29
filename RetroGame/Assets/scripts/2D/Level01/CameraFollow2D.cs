using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform Player;
    public float minX, maxX, minY, maxY;
    
    //Camera movimentation
    public float timeLerp;

    private void FixedUpdate()
    {
        Vector3 newPosition = Player.position + new Vector3(0,0,-10); 
        newPosition.y = -0.2f;

        transform.position = newPosition;

        newPosition = Vector3.Lerp(transform.position, newPosition, timeLerp);
        transform.position = newPosition;
        
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
         transform.position.y, transform.position.z);
        
    }
}
