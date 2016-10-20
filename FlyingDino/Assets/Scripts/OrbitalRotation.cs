using UnityEngine;
using System.Collections;

public class OrbitalRotation : MonoBehaviour
{

    #region Variables

    #region Modifiers

    public float correctionSpeed = 5f;

    #endregion

    #region Transforms

    Transform lastClosestPlanet;

    #endregion

    #region Checks

    bool correctingRotation = false;

    #endregion

    #region Properties

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
                    dist -= (Vector3.Magnitude(tForm.localScale) * 0.5f);
                    if (dist < result)
                    {
                        result = dist;
                        target = tForm;
                    }
                }

                if (target != null)
                {
                    lastClosestPlanet = target;
                    return target;
                }
                else return null;
            }
            else return null;
        }
    }

    #endregion

    #endregion

    #region Functions

    #region Unity

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
        CorrectionCheck();
        ApplyRotation();
    }

    #endregion

    void ApplyRotation()
    {
        if(!correctingRotation)
            transform.up = gravityDirection;
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward, gravityDirection), correctionSpeed * Time.deltaTime);
    }

    void CorrectionCheck()
    {
        if (!correctingRotation)
            correctingRotation = (lastClosestPlanet != closestPlanet);

        else if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(transform.forward, gravityDirection)) < 3)
            correctingRotation = false;
    }

    #endregion

}
