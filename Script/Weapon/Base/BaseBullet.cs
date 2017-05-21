using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

    public int Damge;

    private void Start()
    {
        onStart();
    }

    public virtual void onStart()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        onBulletCrash(collision);
    }

    public virtual void onBulletCrash(Collision collision)
    {

    }

}
