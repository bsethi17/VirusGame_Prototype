using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private Vector3 initialPosition;
    public static float maxRange = 2; // Maximum range the bullet can travel.
    public float force;
    public GameObject objectToDestroy;

    public GameObject initialVirusPrefab;

    private bool canCollide = false;

    public static bool isInitialVirus = false;

    public PopUpCanvas popUpCanvas;

    void Start()
    {
        popUpCanvas = PopUpCanvas.Instance;
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
        float distanceSquared = (transform.position - initialPosition).sqrMagnitude;

        // If the squared distance exceeds the squared maximum range, destroy the bullet
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

        // if the bullet touches vaccinated human
        if (collision.gameObject.tag == "VaccinatedHuman")
        {
            // Virus die, human wins directly
            Destroy(gameObject);
            if (popUpCanvas != null)
            {
                popUpCanvas.ShowPopUp("Human wins!");
            }
            else
            {
                Debug.LogWarning("PopUpCanvas reference is not assigned!");
            }
            return;
        }

        if (collision.gameObject.tag == "NVHuman2" || collision.gameObject.tag == "NVHuman1" || collision.gameObject.tag == "NVHuman3")
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
            maxRange += 2;

            if (!isInitialVirus)
            {
                Destroy(objectToDestroy);
                isInitialVirus = true;
                objectToDestroy = null;
            }
            // Destroy the bullet
            Destroy(gameObject);

        }
    }

    public void ResetToInitialState()
    {
        maxRange = 2;
        canCollide = false;
        isInitialVirus = false;
    }
}
