using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class GravityController : MonoBehaviour
{
    private Rigidbody2D rigid;
    public bool useGravity = true;

    private Transform closestPlanet
    {
        get
        {
            if (PlanetManager.instance.planetTransforms != null &&
                PlanetManager.instance.planetTransforms.Count > 0)
            {
                float result = float.MaxValue;
                Transform target = null;

                foreach (Transform tForm in PlanetManager.instance.planetTransforms)
                {
                    float dist = Vector3.Distance(tForm.position, transform.position);
                    dist -= (Vector3.Magnitude(tForm.localScale) * 0.5f);
                    if (dist < result)
                    {
                        result = dist;
                        target = tForm;
                    }
                }

                if (target != null) return target;
                else return null;
            }
            else return null;
        }
    }

    private Vector3 gravityDirection
    {
        get
        {
            Transform target = closestPlanet;

            if (target != null)
                return (target.position - transform.position).normalized;

            else return Vector3.zero;
        }
    }

    public float gravityStr;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(useGravity)
            ApplyGravity();
    }

    void ApplyGravity()
    {
        transform.position += gravityDirection * gravityStr * Time.deltaTime;
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Planet") Destroy(this.gameObject);
    }
}
