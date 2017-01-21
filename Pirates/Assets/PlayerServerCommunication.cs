using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

public class PlayerServerCommunication : NetworkBehaviour
{
    [SyncVar]
    public int progress = 0;

    void Update()
    {
        if (!isLocalPlayer)
            return;
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            progress -= 1;
            CmdProgress(progress);
            print("progress " + progress);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            progress += 1;
            CmdProgress(progress);
            print("progress " + progress);
        }
    }

    [Command]
    public void CmdProgress(int p)
    {
        if (!isServer)
            return;

        progress = p;
    }

    public override void OnStartLocalPlayer()
    {
        print("OnStartLocalPlayer");
    }
}
