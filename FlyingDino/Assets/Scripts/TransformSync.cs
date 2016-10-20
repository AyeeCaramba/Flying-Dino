using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TransformSync : NetworkBehaviour
{

    [SyncVar]
    Vector3 pos;

    [SyncVar]
    Quaternion rot;

    public float sendRate;
    float curDelay;

    public float posLerpSpeed;
    public float rotLerpSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer && curDelay <= 0 && (Vector3.Distance(pos, transform.position) > 0.02f || Quaternion.Angle(rot, transform.rotation) > 3))
            SendTransform();
        else
            UpdateTransform();

        curDelay -= Time.deltaTime;
    }

    void SendTransform()
    {
        CmdSendPosition(transform.position, transform.rotation);
        curDelay = sendRate;
    }

    void UpdateTransform()
    {
        transform.position = Vector3.Lerp(transform.position, pos, posLerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotLerpSpeed * Time.deltaTime);
    }

    [Command]
    void CmdSendPosition(Vector3 curPos, Quaternion curRot)
    {
        pos = curPos;
        rot = curRot;
        RPCSendPosition(curPos, curRot);
    }

    [RPC]
    void RPCSendPosition(Vector3 curPos, Quaternion curRot)
    {
        pos = curPos;
        rot = curRot;
    }
}
