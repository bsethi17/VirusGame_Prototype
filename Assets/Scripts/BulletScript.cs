using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private Vector3 initialPosition;
    public float maxRange; // Maximum range the bullet can travel.
    public float force;
    void Start()
    {
        // the initial position of the bullet
        initialPosition = transform.position;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        // for bullets to rotate
        // Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        // float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    void Update()
    {
        // Calculate the distance between the bullet's initial position and its current position.
        // Calculate the distance squared between the bullet's initial position and its current position.
        float distanceSquared = (transform.position - initialPosition).sqrMagnitude;

        Debug.Log("Distance Squared: " + distanceSquared + " sprtMaxRange" + maxRange * maxRange);

        // If the squared distance exceeds the squared maximum range, destroy the bulle
        if (distanceSquared > maxRange * maxRange)
        {
            Debug.Log("Bullet Destroyed");
            Destroy(gameObject);
        }
    }
}
