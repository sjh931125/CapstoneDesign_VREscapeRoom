using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charactor : MonoBehaviour {
    //캐릭터의 손 오브젝트를 받을 변수
    public GameObject charHand;
    //캐릭터가 비추는 불빛을 나타낼 오브젝트를 담을 변수
    public GameObject charLight;
    //UI창에 현재 아이템을 보여줄 UI 변수
    public GameObject itemImg;
    
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

    //테스트용 오브젝트
    public GameObject testHead;

    // Use this for initialization
    void Start () {
        moveSpeed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        //캐릭터의 상태가 기본상태일 경우
        if (GameManager.instance.getCharactorStatus() == 0)
        {
            //charMove();
            //mouseClicked();
            //keyboardPushed();
        }
	}

    //캐릭터 이동에 관련한 메소드
    public void charMove()
    {
        //방향키, 마우스 이동 정도를 각 변수에 대입
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        moveMouse = Input.GetAxis("Mouse X");
        testMoveMouseY = Input.GetAxis("Mouse Y");//테스트용 변수
        

        //캐릭터의 이동
        Vector3 moveDirection = (Vector3.forward * moveVertical) + (Vector3.right * moveHorizontal);
        this.transform.Translate(moveDirection.normalized * Time.deltaTime * moveSpeed, Space.Self);

        //캐릭터의 회전
        this.transform.Rotate(Vector3.up * GameManager.instance.getMouseSensitivity() * Time.deltaTime * moveMouse);
        
        //테스트용 코드
        testHead.transform.Rotate(Vector3.left*GameManager.instance.getMouseSensitivity()*Time.deltaTime*testMoveMouseY);
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
                float dist = Vector3.Distance(this.transform.position, hit.transform.position);

                if (dist <= 1.5f)
                {
                    //태그가 Smartphone일 경우 charLight를 활성화시킴
                    if (item.tag.Equals("Smartphone"))
                    {
                        charLight.GetComponent<Light>().enabled = true;
                        Destroy(item);
                        GameManager.instance.setProgress(1);
                        //물건 집을 때의 소리 재생
                        GameManager.instance.playSfx(item.transform.position, audioClipPickItem);
                    }
                    //태그가 Item이고 물건을 가지고 있지 않을때, 물건을 집는다
                    if (item.tag.Equals("Item") && (charHand.transform.childCount == 0))
                    {
                        //item을 charHand의 자식 오브젝트로 만든다
                        item.transform.SetParent(charHand.transform);

                        //item의 위치값과 회전값을 초기화시킨다.
                        item.transform.localPosition = Vector3.zero;
                        item.transform.rotation = new Quaternion(0, 0, 0, 0);

                        setPhyComponents(item, true);

                        //집은 물건의 이미지를 띄우기
                        itemImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/" + item.name);
                        Debug.Log(item.name);
                        itemImg.GetComponent<Image>().enabled = true;

                        //물건 집을 때의 소리 재생
                        GameManager.instance.playSfx(item.transform.position,audioClipPickItem);

                    }
                    //태그가 ElecDoor일 경우
                    if (item.tag.Equals("ElecDoor"))
                    {
                        //progress가 1이고 현재 물건을 들고있을 경우
                        if (GameManager.instance.getProgress() == 1 && charHand.transform.childCount != 0)
                        {
                            //charHand의 자식 오브젝트를 불러옴
                            GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;
                            //들고있는 아이템이 ElecKey일 경우
                            if (child_item.name.Equals("ElecKey"))
                            {
                                //배전반 문여는 소스코드

                                //들고있는 아이템 제거
                                Destroy(child_item);
                                //띄우고 있던 이미지 숨기기
                                itemImg.GetComponent<Image>().enabled = false;
                            }
                        }
                    }
                    //태그가 FuseHome일 경우
                    if (item.tag.Equals("FuseHome"))
                    {
                        //progres가 2이고 현재 물건을 들고있을 경우
                        if (GameManager.instance.getProgress() == 2 && charHand.transform.childCount != 0)
                        {
                            //charHand의 자식 오브젝트를 불러옴
                            GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;
                            //들고있는 아이템이 Red, Green, Blue일 경우
                            if (child_item.name.Equals("Red") || child_item.name.Equals("Green") || child_item.name.Equals("Blue"))
                            {
                                //child_item을 item의 자식 오브젝트로 만든다
                                child_item.transform.SetParent(item.transform);
                                //속성값과 위치, 회전값들을 변경
                                setPhyComponents(child_item, false);
                                child_item.GetComponent<Rigidbody>().isKinematic = false;
                                child_item.transform.localPosition = Vector3.zero;
                                child_item.transform.rotation = new Quaternion(0, 0, 0, 0);
                                //띄우고 있던 이미지 숨기기
                                itemImg.GetComponent<Image>().enabled = false;
                            }
                        }
                    }
                    //태그가 CabDoor일 경우
                    if (item.tag.Equals("CabDoor"))
                    {
                        //progress가 3이고 오브젝트 이름이 Cabinet1인 경우
                        if (GameManager.instance.getProgress() == 0 && item.name.Equals("Cabinet1"))
                        {
                            GameManager.instance.setCharactorStatus(1);
                            GameManager.instance.GetComponent<Lock>().displayOnOff(true);
                        }
                        //progres가 5이고 오브젝트 이름이 Cabinet2인 경우
                        if (GameManager.instance.getProgress() == 5 && item.name.Equals("Cabinet2"))
                        {
                            GameManager.instance.setCharactorStatus(1);
                            GameManager.instance.GetComponent<Lock>().displayOnOff(true);
                        }
                    }
                    //태그가 LastDoor일 경우
                    if (item.tag.Equals("LastDoor"))
                    {
                        //progress가 6이고 현재 물건을 들고있을 경우
                        if (GameManager.instance.getProgress() == 6 && charHand.transform.childCount != 0)
                        {
                            //charHand의 자식 오브젝트를 불러옴
                            GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;
                            if (child_item.name.Equals("LastKey"))
                            {
                                //마지막 문을 여는 이벤트
                            }
                        }
                    }
                }
            }
        }

        //마우스 오른쪽 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(1))
        {
            //물건을 집고있는 상태일 경우, 물건을 바닥에 버린다.
            if (charHand.transform.childCount != 0)
            {
                //charHand의 자식 오브젝트를 불러옴
                GameObject child_item = charHand.GetComponentInChildren<Rigidbody>().gameObject;

                setPhyComponents(child_item, false);

                //item을 charHand의 자식에서 제거한다
                charHand.transform.DetachChildren();

                //띄우고 있던 이미지 숨기기
                itemImg.GetComponent<Image>().enabled = false;

                //물건 버릴 때의 소리 재생
                GameManager.instance.playSfx(charHand.transform.position, audioClipThrowItem);
            }
        }
    }

    //물리속성, 렌더러 속성들 on/off를 위한 메소드
    void setPhyComponents(GameObject item, bool isSet)
    {
        Collider[] itemColliders = item.GetComponents<Collider>();
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        MeshRenderer itemMeshRend = item.GetComponent<MeshRenderer>();

        foreach (Collider itemCollider in itemColliders)
        {
            itemCollider.enabled = !isSet;
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
