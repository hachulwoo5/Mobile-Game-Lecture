using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type {Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;



    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;   //위치에서 나올 총알 
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");

        }
        else if (type == Type.Range && curAmmo >0)
        {
            curAmmo--;
           
            StartCoroutine("Shot");

        }
    }
    IEnumerator Swing()
    {
        
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2 
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;


    }
    //Use 메인 루틴 >> 스윙으로 호출 서브루틴  > 다시 메인 루틴으로 순차적 실행  기존
    //코루틴 메인루틴 ++ 같이 스윙 코루틴 >(코옵)

    IEnumerator Shot()
    {
        //총알 발사 
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 100;
        yield return null;
        //탄피배출
        GameObject instantCase= Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
         caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
