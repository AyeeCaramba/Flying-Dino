using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    private PlayerController playerCont;

    void Start() {
        playerCont = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerCont.attackButtonDown)
        {
            if (bulletPrefab != null) Fire();
        }
    }

    void Fire()
    {
        GameObject obj = Instantiate(bulletPrefab);
        obj.transform.position += this.transform.position;
        if (playerCont.facingRight)
            obj.transform.rotation = this.transform.rotation;
        else
            obj.transform.rotation = new Quaternion(transform.rotation.x, -transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), obj.GetComponent<BoxCollider2D>());
    }

    void ChangeBulletType(GameObject newBullet)
    {
        bulletPrefab = newBullet;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            GunPickup pickup = other.gameObject.GetComponent<GunPickup>();
            if (pickup.canPickup)
            {
                ChangeBulletType(pickup.bulletPrefab);
                pickup.ResetTimer();
            }
        }
    }
}
