using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContolTest : BaseSceneControl {
    private int uid = 1;
    private int tempId = 1001;

    public override void onStart()
    {
        for (int i = 0; i < 4; i++)
        {
            EntityMgr.Instance.CreateEntity(tempId, uid);
            tempId++;
            uid++;
        }
        EntityMgr.Instance.CreateEntity(2001, uid);
        tempId++;
        uid++;
    }

}
