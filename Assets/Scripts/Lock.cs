using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lock : MonoBehaviour {
    public GameObject defaultUI;
    public GameObject cuttonUI;
    public GameObject lockUI;
    public GameObject[] upImg = new GameObject[4];
    public GameObject[] downImg = new GameObject[4];
    public GameObject[] numBG = new GameObject[4];
    public GameObject[] numText = new GameObject[4];
    private int numPosition;
    private int[,] answerCab = { {3,2,5,0},{7,4,8,5} };
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //캐릭터의 상태가 자물쇠 열기일 경우
        if (GameManager.instance.getCharactorStatus() == 1)
        {
            //mouseClicked();
            //keyboardPushed();
        }
    }

    //마우스가 눌렸을 때 실행될 메소드
    public void mouseClicked()
    {
        //마우스 왼쪽 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(0))
        {
            bool check=true;
            int[] inputValue = new int[4];
            for (int i = 0; i < 4; i++) inputValue[i] = int.Parse(numText[i].GetComponent<Text>().text);

            if (GameManager.instance.getProgress() == 3)
            {
                for(int i = 0; i < 4; i++)
                {
                    if (inputValue[i] != answerCab[0, i])
                    {
                        check = false;
                        break;
                    }
                }
            }
            else if (GameManager.instance.getProgress() == 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (inputValue[i] != answerCab[1, i])
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (check == true)
            {
                //문열기 성공
            }
            else
            {
                //문열기 실패
            }

        }

        //마우스 오른쪽 버튼이 눌렸을 때, 자물쇠 이벤트에서 나가기
        if (Input.GetMouseButtonDown(1))
        {
            displayOnOff(false);
            GameManager.instance.setCharactorStatus(0);
        }
    }
    //키보드가 눌렸을 때 실행될 메소드
    public void keyboardPushed()
    {
        //현재 가리키고 있는 위치의 숫자를 저장하는 변수
        int positionValue;
        //W가 눌렸을 때, 현재 위치의 숫자를 1 더함
        if (Input.GetKeyDown(KeyCode.W))
        {
            positionValue = int.Parse(numText[numPosition].GetComponent<Text>().text);
            positionValue++;
            if (positionValue >= 10) positionValue -= 10;
            numText[numPosition].GetComponent<Text>().text = positionValue.ToString();
        }
        //S가 눌렸을 때, 현재 위치의 숫자를 1 뺌
        if (Input.GetKeyDown(KeyCode.S))
        {
            positionValue = int.Parse(numText[numPosition].GetComponent<Text>().text);
            positionValue--;
            if (positionValue < 0) positionValue += 10;
            numText[numPosition].GetComponent<Text>().text = positionValue.ToString();
        }
        //A가 눌렸을 때, 왼쪽으로 한칸 이동
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (numPosition > 0)
            {
                numPosition--;
                movePosition();
            }
        }
        //D가 눌렸을 때, 오른쪽으로 한칸 이동
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (numPosition < 3)
            {
                numPosition++;
                movePosition();
            }
        }
        //ESC를 눌렀을 때, 자물쇠 이벤트에서 나가기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            displayOnOff(false);
            GameManager.instance.setCharactorStatus(0);
        }
    }
    //UI화면 on/off하는 메소드
    public void displayOnOff(bool isSet)
    {
        lockUI.GetComponent<Canvas>().enabled = isSet;
        cuttonUI.GetComponent<Canvas>().enabled = isSet;
        defaultUI.GetComponent<Canvas>().enabled = !isSet;
        //isSet이 true일 경우 초기화
        if (isSet)
        {
            numPosition = 0;
            upImg[0].GetComponent<Image>().enabled = true;
            downImg[0].GetComponent<Image>().enabled = true;
            numBG[0].GetComponent<RawImage>().color = new Color(1,1,0,200/255f);
            for (int i = 1; i < 4; i++)
            {
                upImg[i].GetComponent<Image>().enabled = false;
                downImg[i].GetComponent<Image>().enabled = false;
                numBG[i].GetComponent<RawImage>().color = new Color(1, 1, 1, 200 / 255f);
            }
        }
    }
    //position의 위치가 바뀌면 화살표위치와 배경색 위치를 바꿈
    void movePosition()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == numPosition) continue;
            upImg[i].GetComponent<Image>().enabled = false;
            downImg[i].GetComponent<Image>().enabled = false;
            numBG[i].GetComponent<RawImage>().color = new Color(1, 1, 1, 200 / 255f);
        }
        upImg[numPosition].GetComponent<Image>().enabled = true;
        downImg[numPosition].GetComponent<Image>().enabled = true;
        numBG[numPosition].GetComponent<RawImage>().color = new Color(1, 1, 0, 200 / 255f);
    }
}
