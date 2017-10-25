using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    //싱글톤 패턴을 위한 오브젝트
    public static MainManager instance = null;
    //캐릭터의 상태
    //0 : 기본 상태
    //1 : 환경설정
    private int charactorStatus;
    //환경설정 변수들
    private float volumeEffect;
    private float volumeBGM;
    private float mouseSensitivity;
    //배경음악 AudioSource를 가지고 있는 오브젝트
    public GameObject charactor;
    public GameObject audioSourceCam;
    //
    public GameObject textTitle;
    public GameObject textStart;
    public GameObject textSettings;
    public GameObject textExit;

    public GameObject testHead;
    float moveMouse;
    float testMoveMouseY;//테스트용 변수

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        charactorStatus = 0;

        if (PlayerPrefs.HasKey("volumeBGM") == false)
        {
            this.volumeBGM = 0.5f;
        }
        else
        {
            this.volumeBGM = PlayerPrefs.GetFloat("volumeBGM");
        }

        if (PlayerPrefs.HasKey("volumeEffect") == false)
        {
            this.volumeEffect = 0.5f;
        }
        else
        {
            this.volumeEffect = PlayerPrefs.GetFloat("volumeEffect");
        }

        if (PlayerPrefs.HasKey("mouseSensitivity") == false)
        {
            this.mouseSensitivity = 60f;
        }
        else
        {
            this.mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity");
        }
        setVolumes();
    }

    // Update is called once per frame
    private void Update()
    {
        if (charactorStatus == 0)
        {
            mouseClicked();
            charMove();
        }
        else if (charactorStatus == 1)
        {
            this.GetComponent<MainSettings>().mouseClicked();
            this.GetComponent<MainSettings>().keyboardPushed();
        }
    }

    public void setVolumes()
    {
        audioSourceCam.GetComponent<AudioSource>().volume = volumeBGM;
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
                if (item.name.Equals("Start"))
                {
                    Application.LoadLevel("ClassRoomLogic");
                    Application.LoadLevelAdditive("ClassRoom");
                }
                else if(item.name.Equals("Settings")){
                    mainOnOff(false);
                    MainManager.instance.setCharactorStatus(1);
                    MainManager.instance.GetComponent<MainSettings>().initialization();
                }
                else if (item.name.Equals("Exit"))
                {
                    Application.Quit();
                }
            }
        }
    }


    public int getCharactorStatus()
    {
        return this.charactorStatus;
    }
    public float getVolumeBGM()
    {
        return this.volumeBGM;
    }
    public float getVolumeEffect()
    {
        return this.volumeEffect;
    }
    public float getMouseSensitivity()
    {
        return this.mouseSensitivity;
    }

    public void setCharactorStatus(int _charactorStatus)
    {
        this.charactorStatus = _charactorStatus;
    }
    public void setVolumeBGM(float _volumeBGM)
    {
        this.volumeBGM = _volumeBGM;
    }
    public void setVolumeEffect(float _volumeEffect)
    {
        this.volumeEffect = _volumeEffect;
    }
    public void setMouseSensitivity(float _mouseSensitivity)
    {
        this.mouseSensitivity = _mouseSensitivity;
    }

    //캐릭터 이동에 관련한 메소드
    public void charMove()
    {
        //방향키, 마우스 이동 정도를 각 변수에 대입
        moveMouse = Input.GetAxis("Mouse X");
        testMoveMouseY = Input.GetAxis("Mouse Y");//테스트용 변수

        //캐릭터의 회전
        this.transform.Rotate(Vector3.up * MainManager.instance.getMouseSensitivity() * Time.deltaTime * moveMouse);

        //테스트용 코드
        testHead.transform.Rotate(Vector3.left * MainManager.instance.getMouseSensitivity() * Time.deltaTime * testMoveMouseY);
    }

    public void mainOnOff(bool isOn)
    {
        textTitle.GetComponent<MeshRenderer>().enabled = isOn;
        textStart.GetComponent<MeshRenderer>().enabled = isOn;
        textSettings.GetComponent<MeshRenderer>().enabled = isOn;
        textExit.GetComponent<MeshRenderer>().enabled = isOn;
    }
}
