using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    public GameObject virusObject;
    private float distanceFromVirusObject = 1.0f;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    // how frequentlty player can fire
    private float timer;
    public float timeBetweenFiring;
    public static Shooting shooterInstance;

    // Triangle Renderer to show shooting direction
    private LineRenderer triangleRenderer;
    public float triangleBaseSize = 1.0f; // This value will determine the width of the triangle's base
    public float triangleHeight = 2.0f; // This value will determine the triangle's height

    public int bulletsPerBurst = 2;
    //private int bulletsFiredInBurst = 0;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shooterInstance = this;

        // Initialize TriangleRenderer
        triangleRenderer = GetComponent<LineRenderer>();
        if (triangleRenderer == null)
        {
            triangleRenderer = gameObject.AddComponent<LineRenderer>();
        }
        // triangleHeight = BulletScript.maxRange;
        triangleRenderer.startWidth = 0.05f;
        triangleRenderer.endWidth = 0.05f;
        triangleRenderer.positionCount = 4; // Three corners + close the triangle (returning to the starting point)
        triangleRenderer.loop = true; // Connect the last point to the first to close the triangle
    }

    void Update()
    {
        // Get mouse position
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        // Rotate the red dot to face the mouse cursor.
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        // Set the position of the small circle relative to the rectangular object.
        transform.position = virusObject.transform.position + rotation.normalized * (distanceFromVirusObject + triangleHeight);
        Vector3 shootingPoint = virusObject.transform.position + rotation.normalized * distanceFromVirusObject;

        // Calculate triangle points based on direction and sizes
        Vector3 triangleApex = shootingPoint + rotation.normalized * triangleHeight;
        Vector3 leftBaseCorner = shootingPoint - Quaternion.Euler(0, 0, 90) * rotation.normalized * (triangleBaseSize / 2);
        Vector3 rightBaseCorner = shootingPoint + Quaternion.Euler(0, 0, 90) * rotation.normalized * (triangleBaseSize / 2);

        // Set triangle renderer's positions
        triangleRenderer.SetPosition(0, leftBaseCorner);
        triangleRenderer.SetPosition(1, triangleApex);
        triangleRenderer.SetPosition(2, rightBaseCorner);
        triangleRenderer.SetPosition(3, leftBaseCorner); // close the triangle
        // Set bulletTransform at the tip of the triangle
        bulletTransform.position = triangleApex;


        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && canFire&&UIManager.Instance.GlobalBulletCount > 0)
        {
           
            for (int i = 0; i < bulletsPerBurst; i++) // Loop to fire multiple bullets
            {
                // Fire a bullet
                BulletScript newBullet = Instantiate(bullet, bulletTransform.position, Quaternion.identity).GetComponent<BulletScript>();
                if (this.gameObject.transform.parent != null)
                {
                    newBullet.shooter = this.gameObject.transform.parent.gameObject;
                }
                else
                {
                    // Handle the case where gameObject does not have a parent
                    Debug.LogWarning("The game object " + gameObject.name + " does not have a parent!");
                }
            }

            canFire = false; // Set canFire to false after firing the burst of bullets
             UIManager.Instance.UseBullets(1);
        }

        //Code to switch the shooting agent on right click of infected human
        HandleRightClick();
    }


    public void NotifyHit()
    {
        // Deactivate the shooter object upon a successful hit
        gameObject.SetActive(false);
        Destroy(gameObject);

    }


    private void HandleRightClick()
    {

        if (Input.GetMouseButtonDown(1)) // 1 is for right click
        {

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {

                bool hasInitialVirusChild = false;
                Transform targetVirusTransform = null;

                foreach (Transform child in hit.collider.transform)
                {
                    if (child.CompareTag("InitialVirus"))
                    {
                        hasInitialVirusChild = true;
                        targetVirusTransform = child;
                        break;
                    }
                }

                if (hasInitialVirusChild || hit.collider.CompareTag("InitialVirus"))
                {

                    if (!hasInitialVirusChild)
                        targetVirusTransform = hit.collider.transform;

                    Transform parentTransform = targetVirusTransform.parent;


                    if (parentTransform != null &&
                       (parentTransform.CompareTag("NVHuman1") || parentTransform.CompareTag("NVHuman2") || parentTransform.CompareTag("NVHuman3") || parentTransform.CompareTag("NVHuman4")))
                    {
                        Shooting shootingScript = null;

                        foreach (Transform virusChild in parentTransform)
                        {
                            if (virusChild.CompareTag("InitialVirus"))
                            {
                                foreach (Transform potentialRotatePoint in virusChild)
                                {

                                    if (potentialRotatePoint.name.StartsWith("Rotate Point"))
                                    {
                                        Debug.Log("Found Rotate Point under: " + virusChild.name);
                                        shootingScript = potentialRotatePoint.GetComponent<Shooting>();
                                        break;
                                    }
                                }
                            }

                            //If it doesn't have a RotatePoint child, instantiate one and attach Shooting script
                            if (shootingScript == null)
                            {
                                Debug.Log("creating new shooting agent");
                                GameObject newRotatePoint = Instantiate(gameObject, targetVirusTransform.position, Quaternion.identity);
                                newRotatePoint.transform.SetParent(targetVirusTransform);
                                shootingScript = newRotatePoint.GetComponent<Shooting>();
                                shootingScript.virusObject = parentTransform.gameObject;
                                Destroy(gameObject);
                            }

                            if (shootingScript != null)
                            {
                                Debug.Log(" no need of creating new shooting agent");
                                shootingScript.ActivateShooter();
                            }
                            else
                            {
                                Debug.Log("Failed to activate Shooting script for: " + targetVirusTransform.name);
                            }
                        }
                    }
                }

            }
        }
    }
    public void ActivateShooter()
    {
        // Activate your Shooting functionality here
        Debug.Log("Activating Shooter on: " + gameObject.name);
        canFire = true;
        this.enabled = true;
    }

}