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
                return (transform.position - target.position).normalized;

            else return Vector3.zero;
        }
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
