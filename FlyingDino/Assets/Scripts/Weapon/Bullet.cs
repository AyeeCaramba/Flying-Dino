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

    void Start()
    {
        gravCont = GetComponent<GravityController>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        UpdateBullet();
    }

    public virtual void UpdateBullet()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
        gravCont.gravityStr -= diminishingGravRate * Time.deltaTime;
    }
}
