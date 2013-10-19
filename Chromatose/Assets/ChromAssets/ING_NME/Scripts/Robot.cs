using UnityEngine;
using System.Collections;


//TODEl Script a deleter si le module 2 ne voit pas le jour
public class Robot : MonoBehaviour {
	
	public bool rotates = false;
	public float turnRate = 1f;
	public bool alternate = false;
	public int angle = 180;
	/*[System.SerializableAttribute]
	public class DirectionClass{
		public bool up = true;
		public bool down = true;
		public bool left = false;
		public bool right = false;
		
	}*/
	
	//public DirectionClass directions;
	
	string[] sprites = new string[2] {"robot1", "robot2"};
	int curSprite = 0;
	int directionBin;
	float timer = 0;
	public float delay = 1;
	Transform myLaser;
	Transform t;
	tk2dSprite spriteInfo;

	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
		spriteInfo = t.parent.GetComponent<tk2dSprite>();
		
		/*
		if (t.rotation.eulerAngles.z < -85 && t.rotation.eulerAngles.z > -95){
			curSprite = 1;
		}*/
		
		myLaser = GetComponentInChildren<Transform>();
		if (alternate){
			delay = turnRate;
		}
		/*
		if (directions.up) directionBin ++;
		if (directions.down) directionBin += 2;
		if (directions.left) directionBin += 4;
		if (directions.right) directionBin += 8;
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (!rotates) return;
		
		
		timer += Time.deltaTime;
		if (timer >= delay){
			curSprite = 1 - curSprite;
			//spriteInfo.SetSprite(sprites[curSprite]);
			if (curSprite == 0){
				transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
			}
			else{
				transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
			}
			timer = 0;
			transform.Rotate(new Vector3(0, 0, angle * turnRate));
		}
	}
	
	void Disable(){
		myLaser.gameObject.SetActive(false);
	}
}
