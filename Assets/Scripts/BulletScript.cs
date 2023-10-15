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
    public GameObject shooter; // This will hold the reference to the shooter object


    void Start()
    {
        popUpCanvas = PopUpCanvas.Instance;
        // the initial position of the bullet
        initialPosition = transform.position;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        objectToDestroy = GameObject.FindWithTag("InitialVirus");
        StartCoroutine(EnableCollisionAfterDelay(0.01f));

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

        if (!AreVirusesRemaining())
    {
        EndGame("Human wins!");
    }
    }

    private bool AreVirusesRemaining()
{
    // Check for top-level "InitialVirus" objects
    GameObject[] viruses = GameObject.FindGameObjectsWithTag("InitialVirus");
    if (viruses.Length > 0)
    {
        return true; // A top-level virus is found
    }

    // Check for child objects tagged "InitialVirus"
    GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
    foreach (GameObject obj in allObjects)
    {
        foreach (Transform child in obj.transform)
        {
            if (child.CompareTag("InitialVirus"))
            {
                return true; // A child virus is found
            }
        }
    }
    return false; // No viruses found
}

private void EndGame(string message)
{
    if (popUpCanvas != null)
    {
        popUpCanvas.ShowPopUp(message);
    }
    else
    {
        Debug.LogWarning("PopUpCanvas reference is not assigned!");
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
            if (shooter != null && shooter.transform.parent != null)
            {
                Debug.Log("Bullet was shot by: " + shooter.name);
                // Destroy all shooter objects inside the shooter's parent
                foreach (Transform child in shooter.transform.parent)
                {

                    Destroy(child.gameObject);

                }
            }
            else if (shooter != null && !isInitialVirus)
            {
                
                if (popUpCanvas != null)
                {
                    Debug.Log("Attempting to show popup...");
                    popUpCanvas.ShowPopUp("Human wins!");
                    Destroy(shooter);
                }
                else
                {
                    Debug.LogWarning("PopUpCanvas reference is not assigned!");
                }
            }
            else
            {
                Debug.Log("Shooter or its parent reference is not set for the bullet!");
            }

            return;
        }

        if (collision.gameObject.tag == "NVHuman2" || collision.gameObject.tag == "NVHuman1" || collision.gameObject.tag == "NVHuman3" || collision.gameObject.tag == "NVHuman4")
        {
            Transform initialVirusChild = null;
            Transform rotatePointGrandChild = null;

            foreach (Transform child in collision.transform)
            {
                if (child.name.StartsWith("InitialVirus"))
                {
                    initialVirusChild = child;
                    foreach (Transform grandChild in child)
                    {
                        if (grandChild.name.StartsWith("RotatePoint"))
                        {
                            rotatePointGrandChild = grandChild;
                            break;
                        }
                    }
                }
            }

            //3 cases will be there


            if (initialVirusChild == null)
            {
                // Case 1: Instantiate the initialVirusPrefab under this NVHuman
                InstantiateVirus(collision.transform);
            }
            else if (rotatePointGrandChild == null)
            {
                // Case 2: Destroy the existing initialVirus and instantiate a new one with shooting
                Destroy(initialVirusChild.gameObject);
                InstantiateVirus(collision.transform);
            }

            //case 3 infected human with shooting capability is hit then do nothing
            
            // Notify the shooter of the successful hit
            Shooting.shooterInstance.NotifyHit();

            //initially I will destroy the virus object
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

private void InstantiateVirus(Transform parentTransform)
{
    GameObject newVirus = Instantiate(initialVirusPrefab, parentTransform.position, Quaternion.identity);
    newVirus.transform.SetParent(parentTransform);
    SpriteRenderer sr = newVirus.GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        sr.sortingOrder = 1; // Ensure it is rendered in front
    }
    newVirus.transform.localPosition = new Vector3(0, 0, 0);
    maxRange += 2;
}
}
