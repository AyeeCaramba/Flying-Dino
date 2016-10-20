using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public Bullet bulletPrefab;
	
	void Update () {
        if (Input.GetKey(KeyCode.RightShift))
        {
            Fire();
        }
	}

    void Fire() {
        Bullet obj = Instantiate(bulletPrefab);
        obj.transform.position += this.transform.position;
        //obj.transform.rotation = new Quaternion(0, 0, 90, 0);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), obj.GetComponent<BoxCollider2D>());
    }

    void ChangeBulletType(Bullet newBullet) {
        bulletPrefab = newBullet;
    }

    void OnTriggerEnter2D(Collider2D other) {
        //if (other.gameObject.tag == "Pistol Pickup")
            //ChangeBulletType(other.gameObject.GetComponent<GunPickup>());
    }
}
