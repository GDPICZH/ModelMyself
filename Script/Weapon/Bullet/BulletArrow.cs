using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletArrow : BaseBullet
{
    private bool isUsed = false;
    private float lifeTime = 2f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 && !isUsed)
        {
            onExp();
        }
    }

    public override void onBulletCrash(Collision collision)
    {
        onExp();
    }

    //爆炸
    private void onExp()
    {
        if (isUsed)
        {
            return;
        }
        isUsed = true;
        EffectMgr.Instance.createEffect(10005, new EffectInfo(this.transform.position), false);
        Collider[] cols = Physics.OverlapSphere(this.transform.position, 10);
        for (int i = 0; i < cols.Length; i++)
        {

        }
        DestroyObject(this.gameObject, 1f);
    }

}
