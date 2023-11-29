using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private static Stack<GameObject> infectedStack = new Stack<GameObject>();
    public rangepopup popupController;
    public InfectedPath infectedPathMovementScript;


    public static Stack<GameObject> GetInfectedStack()
    {
        return infectedStack;
    }

    void Start()
    {
        isInitialVirus = GameManager.Instance.isInitialVirus;
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
        popupController = FindObjectOfType<rangepopup>();
        string originalSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        checkForMultipleShooters();
        // Calculate the distance between the bullet's initial position and its current position.
        float distanceSquared = (transform.position - initialPosition).sqrMagnitude;

        // If the squared distance exceeds the squared maximum range, destroy the bullet
        if (distanceSquared > maxRange * maxRange)
        {
            Destroy(gameObject);
        }

    }

    private void checkForMultipleShooters()
    {
        List<GameObject> nvHumans = new List<GameObject>();

        // Step 1: Find all NVHuman objects in the scene.
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("HumanNV"))
            {

                nvHumans.Add(obj);
            }
        }
        List<GameObject> nvHumansWithBothChildren = new List<GameObject>();
        foreach (var nvHuman in nvHumans)
        {
            bool hasInitialVirus = false;
            bool hasRotatePoint = false;

            // Check if NVHuman has both InitialVirus and RotatePoint as children.
            foreach (Transform child in nvHuman.transform)
            {
                if (child.name.StartsWith("InitialVirus"))
                {
                    foreach (Transform grandChild in child)
                    {
                        if (grandChild.name.StartsWith("Rotate Point"))
                        {
                            hasRotatePoint = true;
                            break;
                        }
                    }
                    hasInitialVirus = true;
                }
            }

            if (hasInitialVirus && hasRotatePoint)
            {
                nvHumansWithBothChildren.Add(nvHuman);
            }
        }

        // Step 3: If you find more than one NVHuman meeting the criteria, destroy the RotatePoint of one.
        if (nvHumansWithBothChildren.Count > 1)
        {
            foreach (Transform child in nvHumansWithBothChildren[0].transform)
            {
                if (child.name.StartsWith("InitialVirus"))
                {
                    foreach (Transform grandChild in child)
                    {
                        if (grandChild.name.StartsWith("Rotate Point"))
                        {

                            Destroy(grandChild.gameObject);  // Destroy the RotatePoint.
                            break;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canCollide = true;
    }

    private void RemoveFromInfectedStackByName(string targetName)
    {
        if (infectedStack.Count > 0 && infectedStack.Peek().name == targetName)
        {
            infectedStack.Pop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canCollide) return;

        // if the bullet touches vaccinated human
        if (collision.gameObject.tag == "VaccinatedHuman")
        {
            float previousRange = maxRange;
            maxRange -= 2;
            // Ensure that maxRange doesn't go below a certain threshold if required
            if (maxRange < 2)
            {
                maxRange = 2;
            }
            if (popupController != null && maxRange < previousRange)
            {
                popupController.ShowPopup("Range Decreased!");
            }
            if (!isInitialVirus)
            {
                Destroy(objectToDestroy);
                GameManager.Instance.isInitialVirus = true;
                objectToDestroy = null;
            }

            Destroy(gameObject);
            if (shooter != null && shooter.transform.parent != null)
            {

                // Destroy all shooter objects inside the shooter's parent
                foreach (Transform child in shooter.transform.parent)
                {
                    Destroy(child.gameObject);
                }

                if (infectedStack.Count > 0)
                {


                    RemoveFromInfectedStackByName(shooter.transform.parent.name);

                    if (infectedStack.Count > 0 && infectedStack.Peek().name != shooter.transform.parent.name)
                    {


                        GameObject nextInfected = infectedStack.Peek();
                        Transform initialVirusChild = null;
                        // Find the "InitialVirus" child of the next infected NVHuman
                        foreach (Transform child in nextInfected.transform)
                        {
                            if (child.name.StartsWith("InitialVirus"))
                            {
                                initialVirusChild = child;
                                break;
                            }
                        }
                        if (initialVirusChild)
                        {
                            // Destroy the existing "InitialVirus" child
                            Destroy(initialVirusChild.gameObject);
                            // Instantiate a new "InitialVirus" prefab under the next infected NVHuman
                            InstantiateVirus(nextInfected.transform);
                        }

                    }
                }
            }
            else if (shooter != null && !isInitialVirus)
            {

                if (popUpCanvas != null)
                {
                    popUpCanvas.ShowPopUp("Virus Lost!");
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

            // Shooting.shooterInstance.NotifyHit();
            //delete shooting for the shooter so that it can go to next human
            foreach (Transform child in shooter.transform)
            {
                Destroy(child.gameObject);
            }


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

            //initially I will destroy the virus object
            if (!isInitialVirus)
            {
                Destroy(objectToDestroy);
                GameManager.Instance.isInitialVirus = true;
                objectToDestroy = null;
            }
            maxRange += 1;
            maxRange = Mathf.Min(maxRange, 6);

            if (popupController != null)
            {
                popupController.ShowPopup("Range Increased!");
            }
            // Destroy the bullet
            UIManager.Instance.AddBullets(2);
            Destroy(gameObject);

        }

        if (collision.gameObject.tag == "Blocker")
        {
            Destroy(gameObject);
        }
    }

    public void LogDelay() {
        popupController = FindObjectOfType<rangepopup>();
        popupController.ShowPopup(" ");
        return;
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
        MonoBehaviour virusScript = (MonoBehaviour)newVirus.GetComponent(typeof(MonoBehaviour));
        if (virusScript != null)
        {
            virusScript.enabled = false;
        }


        if (infectedStack.Count == 0 || infectedStack.Peek() != parentTransform.gameObject)
        {
            infectedStack.Push(parentTransform.gameObject);
        }
    }
}