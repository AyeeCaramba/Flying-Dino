using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    Camera cam;

    enum ZoomType { Still, Out, In };
    ZoomType zoomType;

    [Range(0, 1)]
    public float outerClipRange;

    [Range(0, 1)]
    public float innerClipRange;

    public float zoomSpeed;

    public float followSpeed;

    public float innerZoomLimit = 5;

    public float zoomDistanceModifier = 5;

    Vector3 furthestTarget;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPositions();

        switch (zoomType)
        {
            case ZoomType.Out:
                ZoomOut();
                break;
            case ZoomType.In:
                ZoomIn();
                break;
            default:
                break;
        }

        Follow();
    }

    void CheckPlayerPositions()
    {
        zoomType = ZoomType.Still;

        foreach (PlayerController pc in PlayerManager.instance.players)
        {
            if (cam.WorldToViewportPoint(pc.transform.position).x < 0.5f - (outerClipRange * 0.5f) || cam.WorldToViewportPoint(pc.transform.position).x > 0.5f + (outerClipRange * 0.5f) ||
                cam.WorldToViewportPoint(pc.transform.position).y < 0.5f - (outerClipRange * 0.5f) || cam.WorldToViewportPoint(pc.transform.position).y > 0.5f + (outerClipRange * 0.5f))
            {
                zoomType = ZoomType.Out;
                furthestTarget = pc.transform.position;
            }
            Debug.Log(cam.WorldToViewportPoint(pc.transform.position));
            Debug.Log(zoomType);
        }

        if (zoomType != ZoomType.Out)
        {
            bool allPlayersIn = true;

            foreach (PlayerController pc in PlayerManager.instance.players)
            {
                if (cam.WorldToViewportPoint(pc.transform.position).x < 0.5f - (innerClipRange * 0.5f) || cam.WorldToViewportPoint(pc.transform.position).x > 0.5f + (innerClipRange * 0.5f) ||
                    cam.WorldToViewportPoint(pc.transform.position).y < 0.5f - (innerClipRange * 0.5f) || cam.WorldToViewportPoint(pc.transform.position).y > 0.5f + (innerClipRange * 0.5f)) allPlayersIn = false;
            }
            Debug.Log(zoomType);
            if (allPlayersIn) zoomType = ZoomType.In;
        }
    }
    
    void ZoomIn()
    {
        //Debug.Log("IN");
        if (cam.orthographicSize > innerZoomLimit)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize - zoomSpeed, Time.deltaTime);
        else
            cam.orthographicSize = innerZoomLimit;
    }

    void ZoomOut()
    {
        Vector2 zoomTarget = new Vector2(Mathf.Abs(furthestTarget.x), Mathf.Abs(furthestTarget.y));
        //Debug.Log("OUT");
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize + (zoomDistanceModifier * (Vector2.Distance(cam.WorldToViewportPoint(furthestTarget),new Vector2(0.5f + (outerClipRange * 0.5f), 0.5f + (outerClipRange * 0.5f))) * 10)), zoomSpeed * Time.deltaTime);
    }

    void Follow()
    {
        Vector3 avgPos = Vector3.zero;

        foreach (PlayerController pc in PlayerManager.instance.players) avgPos += pc.transform.position;

        avgPos /= PlayerManager.instance.players.Count;

        avgPos.z = -100;

        transform.position = Vector3.Lerp(transform.position, avgPos, followSpeed * Time.deltaTime);
    }
}
