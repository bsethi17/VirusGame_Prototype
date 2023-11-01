using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text bulletCountText;
    public TMP_Text grenadeCountText; // UI element for grenade count

    private static UIManager _instance;

    // Bullet management
    public int GlobalBulletCount { get; private set; } = 4; // Initial bullet value

    // Grenade management
    public int GlobalGrenadeCount { get; private set; } = 0; // Initial grenade value

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
        UpdateGrenadeCount(GlobalGrenadeCount); // Initialize grenade UI
    }

    public void UpdateBulletCount(int count)
    {
        bulletCountText.text = " : " + count;
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

    public void ResetBulletsToInitialCount()
    {
        GlobalBulletCount = 4; // Reset to initial value
        UpdateBulletCount(GlobalBulletCount);
    }

    // Grenade related methods
    public void UpdateGrenadeCount(int count)
    {
        grenadeCountText.text = "Grenades: " + count/2;
    }

    public void UseGrenades(int count)
    {
        GlobalGrenadeCount -= count;
        UpdateGrenadeCount(GlobalGrenadeCount);
    }

    public void AddGrenades(int count)
    {
        GlobalGrenadeCount += count;
        UpdateGrenadeCount(GlobalGrenadeCount);
    }

    public void ResetGrenadesToInitialCount()
    {
        GlobalGrenadeCount = 0; // Reset to initial value
        UpdateGrenadeCount(GlobalGrenadeCount);
    }
}
