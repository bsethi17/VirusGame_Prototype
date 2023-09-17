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
    public GameObject objectToDestroy;

    public GameObject initialVirusPrefab;

    private bool canCollide = false;


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

        objectToDestroy = GameObject.FindWithTag("InitialVirus");
        StartCoroutine(EnableCollisionAfterDelay(0.1f));

    }

    void Update()
    {
        // Calculate the distance between the bullet's initial position and its current position.
        // Calculate the distance squared between the bullet's initial position and its current position.
        float distanceSquared = (transform.position - initialPosition).sqrMagnitude;

        // If the squared distance exceeds the squared maximum range, destroy the bulle
        if (distanceSquared > maxRange * maxRange)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canCollide = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canCollide) return;
        if (collision.gameObject.tag == "NVHuman2")
        {
            // Notify the shooter of the successful hit
            Shooting.shooterInstance.NotifyHit();

            GameObject newVirus = Instantiate(initialVirusPrefab, collision.transform.position, Quaternion.identity);

            // Set the new virus object as a child of the NVHuman2 game object
            newVirus.transform.SetParent(collision.transform);
            SpriteRenderer sr = newVirus.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 1; // Set to a value that ensures it is rendered in front of NVHuman2
            }

            // Set the local position to ensure it is visible
            newVirus.transform.localPosition = new Vector3(0, 0, 0);

            // Destroy the bullet
            Destroy(gameObject);
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
        }
    }

}
