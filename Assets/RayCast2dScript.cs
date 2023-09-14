using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast2dScript : MonoBehaviour
{
    public float rayLength = 100f;
    public LayerMask hitLayers;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRayToMouse();
        }
    }

    void CastRayToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 objectPosition = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(objectPosition, mousePosition - objectPosition, rayLength, hitLayers);

        if (hit.collider != null)
        {
            Debug.Log("Hit object: " + hit.collider.name);
        }
        else
        {
            Debug.Log("No hit");
        }
    }
}