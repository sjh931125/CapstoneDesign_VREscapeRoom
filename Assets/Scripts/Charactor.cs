﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charactor : MonoBehaviour {
    //캐릭터의 손 오브젝트
    public GameObject charHand;
    //캐릭터가 비추는 불빛을 나타낼 오브젝트
    public GameObject charLight;
    //UI창에 현재 아이템을 보여줄 UI 오브젝트
    public GameObject itemImg;
    //카메라를 담고있는 오브젝트
    public GameObject objCamera;
    //몸통을 나타내는 오브젝트
    public GameObject objBody;
    //머리를 나타내는 오브젝트
    public GameObject objHead;
    
    //방향키, 마우스 이동 정도를 나타낼 변수
    float moveHorizontal;
    float moveVertical;
    float moveMouse;
    float testMoveMouseY;//테스트용 변수

    //이동속도를 나타낼 변수
    public float moveSpeed;

    //AudioClip
    public AudioClip audioClipPickItem;
    public AudioClip audioClipThrowItem;
    public AudioClip audioClipLockedCabinet;
    public AudioClip audioClipFuse;
    public AudioClip audioClipTurnOnProjector;
    public AudioClip audioClipSecurity;
    public AudioClip audioClipEndingBGM;

    //테스트용 오브젝트
    //public GameObject testHead;

    //cache용 변수들
    private Transform transformCharHand;
    private Transform transformThis;
    private Transform transformCamera;
    private Transform transformBody;
    private Transform transformHead;

    // Use this for initialization
    void Start () {
        //caching
        transformCharHand = charHand.GetComponent<Transform>();
        transformThis = this.GetComponent<Transform>();
        transformCamera = objCamera.GetComponent<Transform>();
        transformBody = objBody.GetComponent<Transform>();
        transformHead = objHead.GetComponent<Transform>();
        //initializing
        moveSpeed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    //캐릭터 이동에 관련한 메소드
    public void charMove()
    {
        //방향키, 마우스 이동 정도를 각 변수에 대입
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        moveMouse = Input.GetAxis("Mouse X");
        //testMoveMouseY = Input.GetAxis("Mouse Y");//테스트용 변수
        

        //캐릭터의 이동
        Vector3 moveDirection = (Vector3.forward * moveVertical) + (Vector3.right * moveHorizontal);
        transformBody.Translate(moveDirection.normalized * Time.deltaTime * moveSpeed, Space.Self);
        transformHead.position = new Vector3(transformBody.position.x,transformHead.position.y,transformBody.position.z);

        //캐릭터의 회전
        transformCamera.Rotate(Vector3.up * GameManager.instance.getMouseSensitivity() * Time.deltaTime * moveMouse);
        transformBody.eulerAngles = new Vector3(transformBody.eulerAngles.x,transformCamera.eulerAngles.y,transformBody.eulerAngles.z);
        
        //테스트용 코드
        //testHead.transform.Rotate(Vector3.left*GameManager.instance.getMouseSensitivity()*Time.deltaTime*testMoveMouseY);
    }

    //마우스가 눌렸을 때 실행될 메소드
    public void mouseClicked()
    {
        //마우스 왼쪽 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(0))
        {
            //마우스 클릭 지점을 게임내의 좌표로 변환하여 주는 레이캐스트 변수
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
            //레이캐스트에 부딪힌 물체를 담을 변수
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //레이캐스트에 부딪힌 물체를 gameObject로 변환하여 변수에 저장
                GameObject item = hit.transform.GetComponent<Rigidbody>().gameObject;
                //레이캐스트에 부딪힌 물체와 캐릭터간의 거리를 구하여 변수에 저장
                float dist = Vector3.Distance(transformThis.position, hit.transform.position);

                //물건을 들고있을 때 들고있는 물건이 RC인 경우
                if (item.tag.Equals("Projector") && transformCharHand.childCount != 0)
                {
                    //charHand의 자식 오브젝트를 불러옴
                    GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;
                    if (child_item.name.Equals("RC"))
                    {
                        GameObject.FindGameObjectWithTag("Screen").GetComponent<MeshRenderer>().material = GameManager.instance.materialScreen;
                        Destroy(child_item);
                        //띄우고 있던 이미지 숨기기
                        itemImg.GetComponent<Image>().enabled = false;
                        GameManager.instance.playSfx(GameObject.FindGameObjectWithTag("Projector").transform.position, audioClipTurnOnProjector);
                    }
                }

                else if (dist <= 1.5f)
                {
                    //태그가 Item이고 물건을 가지고 있지 않을때, 물건을 집는다
                    if (item.tag.Equals("Item") && (transformCharHand.childCount == 0))
                    {
                        if (GameManager.instance.getFuseSolved() == true)
                        {
                            if(item.name.Equals("Red") || item.name.Equals("Green") || item.name.Equals("Blue"))
                            {
                                return;
                            }
                        }
                        //item을 charHand의 자식 오브젝트로 만든다
                        item.transform.SetParent(transformCharHand);

                        //item의 위치값과 회전값을 초기화시킨다.
                        item.transform.localPosition = Vector3.zero;
                        item.transform.rotation = new Quaternion(0, 0, 0, 0);

                        setPhyComponents(item, true);

                        //집은 물건의 이미지를 띄우기
                        itemImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/" + item.name);
                        itemImg.GetComponent<Image>().enabled = true;

                        //물건 집을 때의 소리 재생
                        GameManager.instance.playSfx(item.transform.position,audioClipPickItem);

                    }
                    //태그가 Door일 경우
                    else if (item.tag.Equals("Door"))
                    {
                        item.GetComponent<ClassForAnimation>().playAnimation();
                    }
                    //태그가 ElecDoor일 경우
                    else if (item.tag.Equals("ElecDoor"))
                    {
                        //현재 물건을 들고있을 경우
                        if (transformCharHand.childCount != 0)
                        {
                            //charHand의 자식 오브젝트를 불러옴
                            GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;
                            //들고있는 아이템이 ElecKey일 경우
                            if (child_item.name.Equals("ElecKey"))
                            {
                                item.GetComponent<ClassForAnimation>().playAnimation();
                                item.tag = "Door";

                                //들고있는 아이템 제거
                                Destroy(child_item);
                                //띄우고 있던 이미지 숨기기
                                itemImg.GetComponent<Image>().enabled = false;
                            }
                            else
                            {
                                GameManager.instance.playSfx(item.transform.position, audioClipLockedCabinet);
                            }
                        }
                        else
                        {
                            GameManager.instance.playSfx(item.transform.position, audioClipLockedCabinet);
                        }
                    }
                    //태그가 FuseHome일 경우
                    else if (item.tag.Equals("FuseHome"))
                    {
                        //현재 물건을 들고있을 경우
                        if (transformCharHand.childCount != 0)
                        {
                            //charHand의 자식 오브젝트를 불러옴
                            GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;
                            int fuseColor=0;
                            //들고있는 아이템이 Red, Green, Blue일 경우
                            if (child_item.name.Equals("Red") || child_item.name.Equals("Green") || child_item.name.Equals("Blue"))
                            {
                                if (child_item.name.Equals("Red")) fuseColor = 1;
                                else if (child_item.name.Equals("Green")) fuseColor = 2;
                                else if (child_item.name.Equals("Blue")) fuseColor = 3;
                                //child_item을 item의 자식 오브젝트로 만든다
                                child_item.transform.SetParent(item.transform);
                                //속성값과 위치, 회전값들을 변경
                                setPhyComponents(child_item, false);
                                child_item.GetComponent<Rigidbody>().isKinematic = true;
                                child_item.transform.localPosition = Vector3.zero;
                                child_item.transform.rotation = new Quaternion(0, 0, 0, 0);
                                //띄우고 있던 이미지 숨기기
                                itemImg.GetComponent<Image>().enabled = false;
                                //효과음
                                GameManager.instance.playSfx(item.transform.position,audioClipFuse);

                                if (item.name.Equals("fuse1h")) GameManager.instance.setInsertFuse(0, fuseColor);
                                if (item.name.Equals("fuse2h")) GameManager.instance.setInsertFuse(1, fuseColor);
                                if (item.name.Equals("fuse3h")) GameManager.instance.setInsertFuse(2, fuseColor);
                                GameManager.instance.checkFuse();
                            }
                        }
                    }
                    //태그가 Smartphone일 경우 charLight를 활성화시킴
                    if (item.tag.Equals("Smartphone"))
                    {
                        charLight.GetComponent<Light>().enabled = true;
                        Destroy(item);
                        //물건 집을 때의 소리 재생
                        GameManager.instance.playSfx(item.transform.position, audioClipPickItem);
                    }
                    //태그가 CabDoor일 경우
                    else if (item.tag.Equals("CabDoor"))
                    {
                        GameManager.instance.GetComponent<Lock>().setSelectedCabinet(item);
                        GameManager.instance.setCharactorStatus(1);
                        GameManager.instance.GetComponent<Lock>().displayOnOff(true);
                        GameManager.instance.playSfx(item.transform.position, audioClipLockedCabinet);
                    }
                    //태그가 Security일 경우
                    else if (item.tag.Equals("Security"))
                    {
                        //현재 물건을 들고있을 경우
                        if (transformCharHand.childCount != 0)
                        {
                            //charHand의 자식 오브젝트를 불러옴
                            GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;
                            if (child_item.name.Equals("LastKey"))
                            {
                                item.GetComponent<MeshRenderer>().material=GameManager.instance.materialSecurity;
                                GameManager.instance.playSfx(item.transform.position, audioClipSecurity);
                                GameManager.instance.setSecurityOpened(true);
                                Destroy(child_item);
                                //띄우고 있던 이미지 숨기기
                                itemImg.GetComponent<Image>().enabled = false;
                            }
                        }
                    }
                    //태그가 LastDoor일 경우
                    else if (item.tag.Equals("LastDoor"))
                    {
                        if (GameManager.instance.getSecurityOpened() == true)
                        {
                            item.GetComponent<ClassForAnimation>().playAnimation();
                            item.GetComponent<Collider>().enabled = false;
                            GameManager.instance.audioSourceCam.GetComponent<AudioSource>().clip = audioClipEndingBGM;
                            GameManager.instance.audioSourceCam.GetComponent<AudioSource>().Play();
                            GameManager.instance.GetComponent<LastEventControl>().startEvent(item.GetComponent<ClassForAnimation>().objAdditional);
                            GameManager.instance.GetComponent<Lock>().defaultUI.GetComponent<Canvas>().enabled = false;
                        }
                        else
                        {
                            GameManager.instance.playSfx(item.transform.position, audioClipLockedCabinet);
                        }
                    }
                }
            }
        }

        //마우스 오른쪽 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(1))
        {
            //물건을 집고있는 상태일 경우, 물건을 바닥에 버린다.
            if (transformCharHand.childCount != 0)
            {
                //charHand의 자식 오브젝트를 불러옴
                GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;

                setPhyComponents(child_item, false);

                //item을 charHand의 자식에서 제거한다
                transformCharHand.DetachChildren();

                //띄우고 있던 이미지 숨기기
                itemImg.GetComponent<Image>().enabled = false;

                //물건 버릴 때의 소리 재생
                GameManager.instance.playSfx(transformCharHand.position, audioClipThrowItem);
            }
        }
    }

    //물리속성, 렌더러 속성들 on/off를 위한 메소드
    void setPhyComponents(GameObject item, bool isSet)
    {
        Collider[] itemColliders = item.GetComponents<Collider>();
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        MeshRenderer itemMeshRend = item.GetComponent<MeshRenderer>();

        for(int i = 0; i < itemColliders.Length; i++)
        {
            itemColliders[i].enabled = !isSet;
        }
        itemRigidbody.isKinematic = isSet;
        itemMeshRend.enabled = !isSet;
    }

    //키보드가 눌렸을 때 실행될 메소드
    public void keyboardPushed()
    {
        //ESC가 눌렸을 때, 메뉴 띄우기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.setCharactorStatus(2);
            GameManager.instance.GetComponent<Settings>().initialization();
        }
    }

    public void controlWalkingSound()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            this.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            this.GetComponent<AudioSource>().mute = true;
        }
    }
}
