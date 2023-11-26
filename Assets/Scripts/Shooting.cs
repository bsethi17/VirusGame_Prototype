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
    public float triangleBaseSize = 0.6f; // This value will determine the width of the triangle's base
    public float triangleHeight = 1.2f; // This value will determine the triangle's height

    public int bulletsPerBurst = 2;
    //private int bulletsFiredInBurst = 0;

    public Color filledColor = Color.green;
    public Color emptyColor = Color.clear;

    public GameObject crosshair;

    public enum ShootingMode
    {
        Bullets,
        Grenades
    }
    private float shootingRange;
    private ShootingMode currentMode = ShootingMode.Bullets;

    public GameObject crosshairInstance;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shooterInstance = this;

        shootingRange = BulletScript.maxRange;
        if (crosshairInstance != null)
        {
            Destroy(crosshairInstance);
        }
        if (crosshair == null)
        {
            Debug.LogError("Crosshair not assigned");
        }

        else if (crosshair != null)
{
    Debug.Log("Instantiating crosshair");
    crosshairInstance = Instantiate(crosshair);
    crosshairInstance.SetActive(true);
}
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

        triangleRenderer.material = new Material(Shader.Find("Sprites/Default"));
        triangleRenderer.startColor = emptyColor;
        triangleRenderer.endColor = emptyColor;
        //crosshairInstance = Instantiate(crosshair, Vector3.zero, Quaternion.identity);
        crosshairInstance.transform.SetParent(transform, false); // Set the triangle GameObject as the parent
        crosshairInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Adjust scale as needed
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
            triangleRenderer.startColor = filledColor;
            triangleRenderer.endColor = filledColor;
            currentMode = ShootingMode.Grenades;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            triangleRenderer.startColor = emptyColor;
            triangleRenderer.endColor = emptyColor;
            currentMode = ShootingMode.Bullets;
        }

        // Get mouse position
        mousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.nearClipPlane));
        mousePos.z = 0; // Ensure that the z position is 0  

        Vector3 direction = (mousePos - virusObject.transform.position).normalized;  // Calculate the normalized direction vector from the virusObject to the mouse cursor
        Vector3 triangleApex = virusObject.transform.position + direction * triangleHeight;  // Calculate the position of the triangle apex

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        transform.position = virusObject.transform.position + direction * distanceFromVirusObject;  // Calculate the position of the small circle
        bulletTransform.position = triangleApex;  // Set the bulletTransform position to the triangle apex

        Vector3 leftBaseCorner = transform.position - Quaternion.Euler(0, 0, 90) * direction * (triangleBaseSize / 2);  // Calculate the left base corner of the triangle
        Vector3 rightBaseCorner = transform.position + Quaternion.Euler(0, 0, 90) * direction * (triangleBaseSize / 2);  // Calculate the right base corner of the triangle
        // Set triangle renderer's positions
        triangleRenderer.SetPosition(0, leftBaseCorner);
        triangleRenderer.SetPosition(1, triangleApex);
        triangleRenderer.SetPosition(2, rightBaseCorner);
        triangleRenderer.SetPosition(3, leftBaseCorner);

        UpdateCrosshairPosition();

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

    private void UpdateCrosshairPosition()
{
    if (crosshairInstance != null)
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.nearClipPlane);
        mousePos = mainCam.ScreenToWorldPoint(mouseScreenPosition);
        mousePos.z = virusObject.transform.position.z; // Make sure it's on the same plane as the virusObject

        Vector3 direction = (mousePos - virusObject.transform.position).normalized;
        float distance = Mathf.Min(shootingRange, Vector3.Distance(virusObject.transform.position, mousePos));

        // Set crosshairInstance position instead of crosshair
        crosshairInstance.transform.position = virusObject.transform.position + (direction * distance);
    }
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