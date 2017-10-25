using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LastEventControl : MonoBehaviour {
    //삭제할 오브젝트
    public GameObject[] objDestroy;
    //벽 오브젝트
    public GameObject[] objWall;
    //액자 오브젝트
    public GameObject[] objFrame;
    //사진 오브젝트
    public GameObject objString;
    //사람모델 오브젝트
    public GameObject objHumanModel;
    public GameObject objModelSpot;
    //캐릭터가 비추는 불빛을 나타낼 오브젝트
    public GameObject charLight;
    //Fade out 효과를 줄 UI Image 오브젝트
    public GameObject fadeOutUI;
    //Audio Clip
    public AudioClip audioClipScream;

    private int frameStack;
    private int stringStack;
    private int progress;
    private float timer;
    private float transparency;
	// Use this for initialization
	void Start () {
        frameStack = 0;
        stringStack = 0;
        timer = 0f;
        progress = 0;
        transparency = 0f;
        objDestroy = new GameObject[2];
        objWall = new GameObject[2];
        objFrame = new GameObject[2];

	}
	
	// Update is called once per frame
	void Update () {
        if (frameStack > 0 && frameStack < 4)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
            //레이캐스트에 부딪힌 물체를 담을 변수
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag.Equals("Frame") && frameStack == stringStack)
                {
                    frameStack++;
                    onGoingEvent();
                }
                else if (hit.transform.gameObject.tag.Equals("String") && frameStack == stringStack + 1)
                {
                    stringStack++;
                    onGoingEvent();
                }
            }
        }
        else if (frameStack >= 4)
        {
            timer += Time.deltaTime;
            if (timer > 13 && transparency<=5f) transparency += Time.deltaTime;
            lastEvent();
        }
	}

    public void startEvent(GameObject[] _obj)
    {
        frameStack = 1;
        for(int i = 0; i < 4; i++)
        {
            Destroy(_obj[i]);
        }
        for(int i = 4; i < 6; i++)
        {
            this.objDestroy[i - 4] = _obj[i];
        }
        for (int i = 6; i < 10; i++)
        {
            _obj[i].GetComponent<Collider>().enabled = true;
            if(i<8) _obj[i].GetComponent<MeshRenderer>().enabled = true;
        }
        for (int i = 10; i < 12; i++)
        {
            this.objFrame[i - 10] = _obj[i];
        }
        this.objString = _obj[12];
        this.objString.GetComponent<Image>().enabled = true;

        this.objHumanModel = _obj[13];
    }

    public void onGoingEvent()
    {
        int tmp;
        if (frameStack == 2 && stringStack == 1)
        {
            objString.GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/StringDontgo");
        }
        if (frameStack == 3 && stringStack == 2)
        {
            objString.GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/StringDie");
        }

        if (frameStack == 1 && stringStack == 1)
        {
            tmp = objDestroy.Length;
            for (int i = 0; i < tmp; i++) Destroy(objDestroy[i]);
            objFrame[0].GetComponent<Canvas>().enabled = true;
        }
        if (frameStack == 2 && stringStack == 2)
        {
            objFrame[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/BiggerEnd");
        }
        if (frameStack == 3 && stringStack == 3)
        {
            objFrame[1].GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }

    public void lastEvent()
    {
        if (timer > 2 && progress==0)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if(timer>2.1 && progress==1)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 3 && progress == 2)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer > 3.1 && progress == 3)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 3.9 && progress == 4)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer > 4 && progress == 5)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 4.6 && progress == 6)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer > 4.7 && progress == 7)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 5.1 && progress == 8)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer > 5.2 && progress == 9)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 5.4 && progress == 10)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer > 5.5 && progress == 11)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 5.6 && progress == 12)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer > 5.7 && progress == 13)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 5.8 && progress == 14)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer > 5.9 && progress == 15)
        {
            charLight.GetComponent<Light>().enabled = true;
            progress++;
        }
        if (timer > 6 && progress == 16)
        {
            charLight.GetComponent<Light>().enabled = false;
            progress++;
        }
        if (timer>8 && progress == 17)
        {
            objHumanModel.transform.SetParent(objModelSpot.transform);
            objHumanModel.transform.localPosition = Vector3.zero;
            objHumanModel.transform.rotation = new Quaternion(0, 0, 0, 0);
            objHumanModel.GetComponent<Animator>().SetBool("isActivated", true);
            GameManager.instance.playSfx(GameManager.instance.charactor.transform.position, audioClipScream);
            fadeOutUI.GetComponent<Image>().enabled = true;
            progress++;
        }
        if(timer>13 && progress == 18)
        {
            fadeOutUI.GetComponent<Image>().color = new Color(0,0,0,transparency/5f);
        }
        if (timer > 19)
        {
            SceneManager.LoadScene("Main");
            SceneManager.UnloadSceneAsync("ClassRoomLogic");
            SceneManager.UnloadSceneAsync("ClassRoom");
        }
    }
}
