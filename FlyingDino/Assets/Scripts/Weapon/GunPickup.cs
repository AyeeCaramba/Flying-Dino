using UnityEngine;
using System.Collections;

public class GunPickup : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float gunRespawnTimer;
    [HideInInspector]
    public bool canPickup = true;

    void Update()
    {
        if (gunRespawnTimer < 5)
        {
            canPickup = false;
            gunRespawnTimer += Time.deltaTime;
        }
        else
            canPickup = true;
    }

    public void ResetTimer()
    {
        gunRespawnTimer = 0;
    }
}
