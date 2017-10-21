using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Char : MonoBehaviour {

    //float timer;
    //float waitingtime;
   // bool isTextOn;

    private Transform this_transform;
    private Transform cam_transform;
    private GameObject head_obj;
    private GameObject hand_obj;
    private GameObject item_Image;
    private Text sysmsg_text;

    public float movespeed = 10.0f;
    public float rotspeed = 100.0f;

	// Use this for initialization
	void Start () {
        this_transform = GetComponent<Transform>();
        cam_transform = GameObject.Find("Camera (eye)").GetComponent<Transform>();
        head_obj = GameObject.Find("Head");
        hand_obj = GameObject.Find("Hand");
        item_Image = GameObject.Find("Item_Image");
        item_Image.GetComponent<Image>().enabled = false;
        sysmsg_text = GameObject.Find("sysmsg_text").GetComponent<Text>();

       // timer = 0.0f;
       // waitingtime = 2.0f/Time.deltaTime;

    }
	
	// Update is called once per frame
	void Update () {
       float h = Input.GetAxis("Horizontal");
       float v = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse X");

        /*if(sysmsg_text.enabled)
        {
            timer += 1 / Time.deltaTime;

            if (timer >= waitingtime)
            {
                //timer = 0.0f;
                sysmsg_text.enabled = false;
            }
              
        }*/

                
        this_transform.rotation = Quaternion.Euler(new Vector3(0, cam_transform.eulerAngles.y, 0)); //카메라(머리) 방향대로 몸 회전

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        this_transform.Translate(moveDir.normalized * Time.deltaTime * movespeed, Space.Self); //몸 이동

        head_obj.transform.position = new Vector3(this_transform.position.x, head_obj.transform.position.y, this_transform.position.z); //카메라(머리)가 몸을 따라감

        //좌 우 회전
        /*if (Input.GetKeyDown("q")) 
             cam_transform.Rotate(0, -90, 0);
         if (Input.GetKeyDown("e")) 
             cam_transform.Rotate(0, 90, 0);*/

        this_transform.Rotate(Vector3.up * rotspeed * Time.deltaTime * mouse);
        head_obj.transform.Rotate(Vector3.up * rotspeed * Time.deltaTime * mouse);

        if (Input.GetMouseButton(0)) //왼쪽 키 줍기
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string item_tag = hit.transform.GetComponent<GenItem>().GetTag();

                if (item_tag.Equals("Item") && (hand_obj.transform.childCount == 0))
                {
                    GameObject item = hit.transform.GetComponent<Rigidbody>().gameObject;
                    item.GetComponent<GenItem>().GetThis(hand_obj);
                    Pickup(item);
                    
                }
                if(item_tag.Equals("Door"))
                {
                    //timer = 0.0f;

                    if(hand_obj.transform.Find("Last_key"))
                    {
                        //sysmsg_text.enabled = true;
                        sysmsg_text.text = "Opened!";
                    }
                    else
                    {
                       // sysmsg_text.enabled = true;
                        sysmsg_text.text = "Locked..";
                    }
                }
            }
        }

        if(Input.GetMouseButton(1)) //우측키 버리기
        {
            if(hand_obj.transform.childCount != 0)
            {
                Drop();   
            }
        }
    }

    public void Pickup (GameObject item)
    {
        SetEquip(item, true);
        item_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/"+item.name);
        Debug.Log(item.name);
        item_Image.GetComponent<Image>().enabled = true;

    }

    public void Drop()
    {
        GameObject item = hand_obj.GetComponentInChildren<Rigidbody>().gameObject;

        SetEquip(item, false);
        
        hand_obj.transform.DetachChildren();
        item_Image.GetComponent<Image>().enabled = false;
    }

    void SetEquip(GameObject item, bool isGet)
    {
        Collider[] itemColliders = item.GetComponents<Collider>();
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        MeshRenderer ItemMeshRend = item.GetComponent<MeshRenderer>();

        foreach(Collider itemCollider in itemColliders)
        {
            itemCollider.enabled = !isGet;
        }

        itemRigidbody.isKinematic = isGet;
        ItemMeshRend.enabled = !isGet;
    }
}
