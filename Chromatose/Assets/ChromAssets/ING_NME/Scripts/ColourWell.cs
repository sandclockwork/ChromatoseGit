using UnityEngine;
using System.Collections;

public class ColourWell : MonoBehaviour {
	
	public enum _WellTypeEnum{
		RedGyser_1, RedGyser_2, RedLeak_1, BlueWell_1, BlueLeak_1, BlueLeak_2
	}
	
	public _WellTypeEnum wellType;
	
	private ChromatoseManager _Manager;
	private Avatar _AvatarScript;
	private Color myColor;
	private tk2dAnimatedSprite _MainAnim;
	
	
	private string redWellString1 = "redWellL1_gyser";
	private string redWellString2 = "redWellL2_gyser";
	private string redWellString3 = "redWellL3_wall";
	private string blueWellString1 = "blueWellL4";
	private string blueWellString2 = "blueWell_godBlavatar";
	private string blueWellString3 = "blueWell_leakPomp";
	
	void Start () {
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
		
		switch(wellType){
		case _WellTypeEnum.RedGyser_1:
			myColor = Color.red;
			_MainAnim.Play(redWellString1);
			
			break;
		case _WellTypeEnum.RedGyser_2:
			myColor = Color.red;
			_MainAnim.Play(redWellString2);
			BoxCollider bCollider = GetComponent<BoxCollider>();
			bCollider.center = new Vector3(3.5f, -50f, 0f);
			
			break;
		case _WellTypeEnum.RedLeak_1:
			myColor = Color.red;
			_MainAnim.Play(redWellString3);
			
			break;
		case _WellTypeEnum.BlueWell_1:
			myColor = Color.blue;
			_MainAnim.Play(blueWellString1);
			
			break;
		case _WellTypeEnum.BlueLeak_1:
			myColor = Color.blue;
			_MainAnim.Play(blueWellString2);
			
			break;
		case _WellTypeEnum.BlueLeak_2:
			myColor = Color.blue;
			_MainAnim.Play(blueWellString3);
			
			break;
		}
	}
	
	
	void OnTriggerStay(Collider collider){
		
		if (collider.tag != "avatar") return;
		if (myColor == Color.red){
			GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().OnRedWell = true;
		}
		
	 	HUDManager.hudManager.UpdateAction(Actions.Absorb, Trigger);		//this tells the manager that I want to do something. But I'll have to wait in line!
	}
	void OnTriggerExit(Collider collider){
		if(collider.tag != "avatar") return;
		if(myColor == Color.red){
			GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().OnRedWell = false;
		}
		HUDManager.hudManager.OffAction();
	}
	
	
	void Trigger(){

		GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>().FillBucket(myColor);
		MusicManager.soundManager.PlaySFX(25);
	}
}
