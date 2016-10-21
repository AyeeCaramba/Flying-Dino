using UnityEngine;
using System.Collections;

public class GunPickup : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float gunRespawnTimer;
    [HideInInspector]
    public bool canPickup = true;

    private Quaternion nextRot;
    public float changeTime;

    void Start()
    {
        nextRot = Random.rotation;
    }

    void Update()
    {
        HandleRespawn();
        HandleRotation();
    }

    void HandleRespawn()
    {
        if (gunRespawnTimer < 5)
        {
            canPickup = false;
            gunRespawnTimer += Time.deltaTime;
            SetFade(true);
        }
        else
        {
            canPickup = true;
            SetFade(false);
        }
    }

    void HandleRotation()
    {
        if (canPickup)
            if (Quaternion.Angle(transform.rotation, nextRot) < 5)
                nextRot = Random.rotation;
            else
                transform.rotation = 
                    Quaternion.Slerp(transform.rotation, nextRot, changeTime * Time.deltaTime);
    }

    void SetFade(bool value)
    {
        foreach (Material mat in GetComponent<Renderer>().materials)
        {
            Color col = mat.GetColor("_Color");
            if (value) mat.SetColor("_Color", new Color(col.r, col.b, col.g, .2f));
            else if (!value) mat.SetColor("_Color", new Color(col.r, col.b, col.g, 1f));
        }
    }

    public void ResetTimer()
    {
        gunRespawnTimer = 0;
    }
}
