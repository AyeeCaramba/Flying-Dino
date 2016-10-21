using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(OrbitalRotation))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeed;

    private GravityController gravCont;
    public float diminishingGravRate;
    private Rigidbody2D rigid;

    private float lifeTime = 5;
    public int ammoCount;

    void Start()
    {
        gravCont = GetComponent<GravityController>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        UpdateBullet();
        if (lifeTime < 0) Destroy(this.gameObject);
        else lifeTime -= Time.deltaTime;
    }

    public virtual void UpdateBullet()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
        gravCont.gravityStr -= diminishingGravRate * Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Planet") Destroy(this.gameObject);
        if (other.gameObject.tag == "Player") Debug.Log("Hit player");
    }
}
