using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    public GameObject virusObject;
    private float distanceFromVirusObject = 0.5f;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;

    public GameObject grenadePrefab;
    public Transform grenadeTransform;

    public bool canFire;
    // how frequentlty player can fire
    private float timer;
    public float timeBetweenFiring;
    public static Shooting shooterInstance;

    // Triangle Renderer to show shooting direction
    private LineRenderer triangleRenderer;
    public float triangleHeight = 1.2f; // This value will determine the triangle's height

    public int bulletsPerBurst = 2;
    //private int bulletsFiredInBurst = 0;

    public Color bulletColor = Color.white; // Default color for the bullet mode
    public Color grenadeColor = Color.red; // Color for when the grenade mode is

    public enum ShootingMode
    {
        Bullets,
        Grenades
    }

    private ShootingMode currentMode = ShootingMode.Bullets;

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

        // Initialize LineRenderer for single line
        triangleRenderer.startWidth = 0.05f;
        triangleRenderer.endWidth = 0.05f;
        triangleRenderer.positionCount = 2; // Only two points for a single line
        triangleRenderer.loop = false;
        triangleRenderer.material.color = bulletColor;
        triangleHeight = BulletScript.maxRange;
    }

    void Update()
    {
        if (Time.timeScale == 0)  // Check if the game is paused
        {
            canFire = false;  // Disable shooting
            return;  // Exit the Update method
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            currentMode = ShootingMode.Grenades;
            triangleRenderer.material.color = grenadeColor; // Change the triangle color to grenade color
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            currentMode = ShootingMode.Bullets;
            triangleRenderer.material.color = bulletColor; // Change the triangle color back to bullet color
        }

        // Get mouse position
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure that the z position is 0  
        triangleHeight = BulletScript.maxRange;

        Vector3 direction = (mousePos - virusObject.transform.position).normalized;  // Calculate the normalized direction vector from the virusObject to the mouse cursor
        Vector3 triangleApex = virusObject.transform.position + direction * triangleHeight;  // Calculate the position of the triangle apex

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        transform.position = virusObject.transform.position + direction * distanceFromVirusObject;  // Calculate the position of the small circle
        bulletTransform.position = transform.position;  // Set the bulletTransform position to the start of line 

        // Set LineRenderer's positions for the line
        triangleRenderer.SetPosition(0, virusObject.transform.position);
        triangleRenderer.SetPosition(1, triangleApex);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && canFire && UIManager.Instance.GlobalBulletCount > 0)
        {
            switch (currentMode)
            {
                case ShootingMode.Bullets:
                    if (UIManager.Instance.GlobalBulletCount > 0)
                    {
                        ShootBullets();
                    }
                    break;
                case ShootingMode.Grenades:
                    if (UIManager.Instance.GlobalGrenadeCount > 0)
                    {
                        ShootGrenades();
                    }
                    break;
            }
        }

        //Code to switch the shooting agent on right click of infected human
        HandleRightClick();
    }

    private void ShootBullets()
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

    private void ShootGrenades()
    {
        Debug.Log("g");
        if (UIManager.Instance.GlobalGrenadeCount > 0)
        {
            GameObject newGrenade = Instantiate(grenadePrefab, bulletTransform.position, Quaternion.identity);
            GrenadeScript grenadeScript = newGrenade.GetComponent<GrenadeScript>();

            // Apply forces or any other initialization to the grenade
            // For example:
            // Rigidbody2D rb = newGrenade.GetComponent<Rigidbody2D>();
            // rb.AddForce(...);

            canFire = false;
            UIManager.Instance.UseGrenades(1);
        }
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

