using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    private PlayerController playerCont;

    public int currentAmmo = 1;
    private float resetFireRate;
    public float fireRate = 1;

    private bool fired = false;

    public bool hasGun = false;

    void Start()
    {
        playerCont = transform.parent.GetComponent<PlayerController>();
        resetFireRate = fireRate;
        fireRate = 0;
    }

    void Update()
    {
        if (playerCont.attackButtonDown)
        {
            if (bulletPrefab != null)
                Fire();
        }

        if (fired)
            fireRate -= Time.deltaTime;
        else if (fireRate < 0)
            fired = false;
    }

    void Fire()
    {
        if (currentAmmo >= 0 && fireRate <= 0)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.position += this.transform.position;
            if (playerCont.facingRight)
                obj.transform.rotation = this.transform.rotation;
            else
                obj.transform.rotation = new Quaternion(transform.rotation.x, -transform.rotation.y, transform.rotation.z, transform.rotation.w);
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), obj.GetComponent<BoxCollider2D>());
            SetFire();
        }
    }

    void SetFire() {
        currentAmmo--;
        fired = true;
        fireRate = resetFireRate;
        if (currentAmmo <= 0)
        {
            bulletPrefab = null;
            fireRate = 0;
        }
    }

    void ChangeBulletType(GameObject newBullet)
    {
        bulletPrefab = newBullet;
        currentAmmo = newBullet.GetComponent<Bullet>().ammoCount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            GunPickup pickup = other.gameObject.GetComponent<GunPickup>();
            if (pickup.canPickup && bulletPrefab != pickup.bulletPrefab)
            {
                hasGun = true;
                ChangeBulletType(pickup.bulletPrefab);
                pickup.ResetTimer();
            }
        }
    }
}
