using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrowerSprite : MonoBehaviour {
    public Gun gun;
    public Transform target;

    public void Shoot() {
        var isFacingRight = target != null ? target.transform.position.x > transform.position.x : true;
        gun.Shoot(isFacingRight);
    }
}
