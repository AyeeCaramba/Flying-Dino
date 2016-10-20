using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityController : MonoBehaviour
{
    private Rigidbody2D rigid;

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
        ApplyGravity();
    }

    void ApplyGravity()
    {
        transform.position += gravityDirection * gravityStr * Time.deltaTime;
    }
}
