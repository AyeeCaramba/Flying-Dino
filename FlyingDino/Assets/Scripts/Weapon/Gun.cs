using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    private PlayerController playerCont;
    public Transform gunHand;

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
            if (playerCont.facingRight)
            {
                obj.transform.rotation = this.transform.rotation;
                obj.transform.position += this.transform.position;
            }
            else
            {
                obj.transform.localScale = new Vector3(obj.transform.lossyScale.x, -obj.transform.lossyScale.y, obj.transform.lossyScale.z);
                obj.transform.position -= this.transform.position;
            }
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), obj.GetComponent<BoxCollider2D>());
            SetFire();
        }
    }

    void SetFire()
    {
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
                GameObject obj = (GameObject)Instantiate(pickup.modelPrefab);
                obj.transform.parent = gunHand;
                obj.transform.localPosition =
                    new Vector3(-0.04041304f, 0.000799284F, -0.001902708f);
                obj.transform.localRotation =
                    Quaternion.Euler(new Vector3(-35.4891f, -92.111f, -87.2822f));

                if (this.transform.FindChild("Gun") != null)
                    Destroy(this.transform.FindChild("Gun"));

                obj.name = "Gun";

                pickup.ResetTimer();
            }
        }
    }
}
