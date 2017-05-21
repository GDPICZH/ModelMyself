using System;
using System.Collections.Generic;

public class SceneStartGameControl : BaseSceneControl
{

    public override void onStart()
    {
        Message msg = new Message(MsgCmd.Open_Main_Meun_UI, this);
        msg.Send();
    }

}

