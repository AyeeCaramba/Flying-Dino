using UnityEngine;
using System.Collections;

public class OrbitalRotation : MonoBehaviour
{
    private Transform closestPlanet
    {
        get
        {
            PlanetManager reference = PlanetManager.instance;
            if (reference.planetTransforms != null &&
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
    
    void Update()
    {
        this.transform.up = -gravityDirection;
    }
}
