using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart , Weapon };  //변수가 아닌 타입
    public Type type;       //변수는 따로 
    public int value;


    Rigidbody rigid;
    SphereCollider sphereCollider;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }

}
