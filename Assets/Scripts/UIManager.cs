using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text bulletCountText;
    private static UIManager _instance;

    // Add bullet management
    public int GlobalBulletCount { get; private set; } = 3;  // Initial value

    public static UIManager Instance 
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UIManager is null!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        UpdateBulletCount(GlobalBulletCount);
    }

    public void UpdateBulletCount(int count)
    {
        bulletCountText.text = "Bullets: " + count;
    }

    public void UseBullets(int count)
    {
        GlobalBulletCount -= count;
        UpdateBulletCount(GlobalBulletCount);
    }

    public void AddBullets(int count)
    {
        GlobalBulletCount += count;
        UpdateBulletCount(GlobalBulletCount);
    }
}

