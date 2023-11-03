using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS : MonoBehaviour
{
    // Human2 Shield
    public GameObject shield;
    // set how many times we need to shoot to make the shield of human2 disappear
    private int degree;
    // color component of shield
    private SpriteRenderer shieldRenderer;
    public Color shieldColor1;
    public Color shieldColor2;
    public Color shieldColor3;

    void Start()
    {
        shield = this.gameObject;
        // degree = 6;
        shieldRenderer = shield.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "bullet" && gameObject != null) 
        {
            Debug.Log("SHIELD DETECTED");
            // Debug.Log(degree);
            Destroy(collision.gameObject);
            // degree -= 1;
            // if (degree == 5 || degree == 4)
            // {
            //     shieldRenderer.color = shieldColor2;
            //     Debug.Log("color2");
            //     Debug.Log(degree);
            // }
            // else if (degree == 3 || degree == 2)
            // {
            //     shieldRenderer.color = shieldColor3;
            //     Debug.Log("color3");
            //     Debug.Log(degree);
            // }
            // else
            // {
                // Debug.Log("destroy");
                // Debug.Log(degree);
                Destroy(gameObject);     
            // }

        }
    }
}
