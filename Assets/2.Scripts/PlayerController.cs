using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum eAniKind
    {
        IDLE=0,
        FLY=1,
        DOWNHILL,
        DEAD,
    }
    eAniKind currentKind;

    Animator anim;
    Rigidbody2D rigid;

    [SerializeField] float flyPower = 3;
    [SerializeField] float touchDuration = 0.5f;
    float touchCount = 0.5f;
    bool isDead = false;
    float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        downHill();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) //실수를 줄이기 위한 제동장치
        {
            touchCount += Time.deltaTime;

            if(transform.position.y<4.5f&&touchCount>touchDuration)
                flyTouch();

            downHill();
        }
        ApplyAngle();
    }

    void changeAnim(eAniKind kind)              //현재 상태를 받아와서
    {
        if (!isDead) //죽은 후에는 더이상 애니메이션 실행 X            
            anim.SetInteger("AnKind", (int)kind);   //불 형이 아닌 인트 형으로 만든 파라매터를 현재 상태값으로 저장해둔 이넘으로 각 상황에 맞는 애니메이션 실행
    }

    void flyTouch()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            touchCount = 0;
            rigid.velocity = new Vector2(0, flyPower);
            //리지드바디가 가지고 있는 물리적인 속도를 조정(직관적으로 조정 가능)
            //addforce는 질량에 따라 속도가 지정됨            
            changeAnim(eAniKind.FLY);            
        }
    }

    void downHill()
    {
        //속도가 음수면, 내려가는중이면 하강 애니메이션 실행
        if (rigid.velocity.y < 0)
            changeAnim(eAniKind.DOWNHILL);
    }

    void ApplyAngle()       //플레이어 속도에 따른 각도 변경
    {
        float targetAngle;

        targetAngle = Mathf.Atan2(rigid.velocity.y, 5) * Mathf.Rad2Deg;
        //아크탄젠트를 사용하여 현재 속도에 맞는 각도 구하기
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10);
        //러프 함수를 사용하여 점진적 각도 변경
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
            return;
       
        changeAnim(eAniKind.DEAD);
        isDead = true;
        Debug.Log(collision.transform.name + " 충돌");
    }

}
