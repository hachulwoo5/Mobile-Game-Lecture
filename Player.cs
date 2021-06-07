using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;     //움직임
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;    //배열 변수
    public int hasGrenades;
    public GameObject grenadeObj;

    public Camera followCamera;     //쿼터뷰 카메라 변수
    public int ammo;
    public int coin;
    public int health;

    public int Gold;
    public int Silver;
    public int Bronze;


    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    float hAxis;     //움직임
    float vAxis;    //움직임

    bool wDown;
    bool jDown;
    bool fDown;
    bool gDown;
    bool rDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage;

    Vector3 moveVec;     //움직임
    Vector3 dodgeVec;    //회피 도중 방향전환 x 

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;

    GameObject nearObject;
    public Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;
    public GameObject menuPanel;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        turn();
        Jump();
        Grenade();
        Attack();
        Reload();
        Dodge();
        Interaction();
        Swap();

        if(health ==0)
        {
            SceneManager.LoadScene("SampleScene");
        }

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");   //움직임
        vAxis = Input.GetAxisRaw("Vertical");    //움직임
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interaction");
        sDown1= Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;     //움직임 변수

        if (isDodge)
            moveVec = dodgeVec;  //회피 중에는 움직임 벡터 > 회피 방향벡터로 바뀜
        
        //삼항 연산자 [조건 ? 참 : 거짓]

        if (isSwap || isReload || !isFireReady)
            moveVec = Vector3.zero;

        if(!isBorder)          //벽 보더값을 통해 통과 못하게함
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;     //움직임 코드

        anim.SetBool("isRun", moveVec != Vector3.zero);     //애니메이션
        anim.SetBool("isWalk", wDown);     //애니메이션
    }

    void turn()
    {
        transform.LookAt(transform.position + moveVec);  // 시야 키보드 회전

        if(fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);   //스크린에서 월드로 레이를 쏨 
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        


    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isJump && !isDodge)   // 점프가 false 일때만 작동
        {
            /*rigid.AddForce(Vector3.up * 150, ForceMode.Impulse);    //점프 리지드바디에 힘 줘서 올리기 
            anim.SetBool("isJump", true); //애니메이션 점프 ,트루 
            anim.SetTrigger("doJump");    // 트리거 값 두 점프
            isJump = true;*/
        }
    }

    void Grenade()
    {
        if (hasGrenades == 0)
            return;
        
        if(gDown && !isReload && !isSwap )
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);   //스크린에서 월드로 레이를 쏨 
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 15;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation); //프리팹 인스턴트화
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();   
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;  //공격속도 >> 파이어딜레이보다 커서 공격 가능
        Debug.Log(isFireReady);
        if (fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ?"doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        if (equipWeapon == null)
            return;

        if (equipWeapon.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if(rDown && !isJump && !isDodge && !isSwap && isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 1f);
        }
    }
    
    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo; 
        isReload = false;
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap)   // 점프가 false 일때만 작동
        {
            dodgeVec = moveVec;                 //구르기 변수
            speed *= 2;   //회피 시 이 속 
            anim.SetTrigger("doDodge");    // 트리거 값 두 닷지
            isDodge = true;

            Invoke("DodgeOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {

        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))

            return;

        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
        
          return;

        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))

         return;



            int weaponIndex = -1;
            if (sDown1) weaponIndex = 0;
            if (sDown2) weaponIndex = 1;
            if (sDown3) weaponIndex = 2;


            if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
            {
                if (equipWeapon != null)        //빈손 x 
                    equipWeapon.gameObject.SetActive(false);

                equipWeaponIndex = weaponIndex;
                equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();   //기존에 장착된 무기 변수 
                equipWeapon.gameObject.SetActive(true);

                anim.SetTrigger("doSwap");
                isSwap = true;

                Invoke("SwapOut", 0.4f);
            }

        }
    
        void SwapOut()
        {
            isSwap = false;
        }

        void Interaction()
        {
            if (iDown && nearObject != null && !isJump && !isDodge)
            {
                if (nearObject.tag == "Weapon")
                {
                    Item item = nearObject.GetComponent<Item>();
                    int weaponIndex = item.value;       //무기 번호
                    hasWeapons[weaponIndex] = true;    // 배열 중 무기번호 

                    Destroy(nearObject);
                }


            }
        }

        void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;

    }

        void StopToWall()
    {

        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);  //플레이어 앞 물체 인식 
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }
    void OnCollisionEnter(Collision collision)   // 점프 바닥태그로 제한
        {
            if (collision.gameObject.tag == "Floor")
            {
                anim.SetBool("isJump", false);
                isJump = false;
            }
        }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)                //플레이어 수치 최대값 넘지 않게 
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)                //플레이어 수치 최대값 넘지 않게 
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)                //플레이어 수치 최대값 넘지 않게 
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    if (hasGrenades == maxHasGrenades)
                        return;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
             
            }
            Destroy(other.gameObject);



        }
        if (other.tag == "Gold")
        {
            Gold++;
            Destroy(other.gameObject);
        }
        if (other.tag == "Silver")
        {
            Silver++;
            Destroy(other.gameObject);
        }
        if (other.tag == "Bronze")
        {
            Bronze++;
            Destroy(other.gameObject);
        }

        if (other.tag == "water")
        {
            SceneManager.LoadScene("SampleScene");             //물에 빠지면 재시작 
        }

        if (other.tag == "Finish" && Gold == 5 && Silver == 5 && Bronze == 5)
        {
            SceneManager.LoadScene("SampleScene");          //피니쉬 깃발에 닿고 동전을 다 모아야 클리어
            menuPanel.SetActive(true);
        }

        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                

                StartCoroutine(OnDamage());
            }
            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }
        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
    }

    void OnTriggerStay(Collider other)
        {
            if (other.tag == "Weapon")
                nearObject = other.gameObject;

            
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Weapon")
                nearObject = null;
        }
    }

