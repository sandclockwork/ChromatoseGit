using UnityEngine;
using System.Collections;

public class MiniBoss : MonoBehaviour {
	
	
	public int requiredPayment = 0;
	public int shootingArea = 500;
	public float fireRate = 2.5f;
	
	public bool shootFlame = false;
	
	public GameObject flameBullet;
	
	private tk2dAnimatedSprite[] myFlames;
	private tk2dAnimatedSprite _MainAnim;
	private Avatar _AvatarScript;
	private Transform _AvatarT;
	private ChromatoseManager _Manager;	
	private MiniBoss_PayZone _PayZoneScript;
	private MiniBossShootingZone _ShootingZone;
	
	private float _KnockBack = 50f;
	private float _FadeRate = 0.005f;
	private float _FadingCounter = 1;
	private float _ShooterCounter = 0;
	
	private bool beingExtinguished = false;
	private bool _CanDie = false;
	

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(_PayZoneScript.inPayZone && _Manager.rCollected >= requiredPayment){
			_Manager.UpdateAction(Actions.Release, DelegateActionTest);
			_AvatarScript.WantsToRelease = true;
		}
		
		if(_CanDie){
			_FadingCounter -= _FadeRate;
			_MainAnim.color = new Color(0, 0, 0 ,_FadingCounter);
			
			if(myFlames != null){
				foreach(tk2dAnimatedSprite sprite in myFlames){
					sprite.color = new Color(0, 0, 0, _FadingCounter);
				}
			}
			if(_FadingCounter <= 0 ){
				Die ();
			}
		}
		
		if (shootFlame && _ShootingZone.inShootingZone){
			
			_ShooterCounter += Time.deltaTime;
			
			if (_ShooterCounter >= fireRate){
				GameObject bullet = Instantiate(flameBullet, transform.position, Quaternion.identity)as GameObject;
				_ShooterCounter = 0;
			}
		}
	}
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "avatar"){return;}
		
		_Manager.Death();
	}
	
	void DelegateActionTest(){
		
		_Manager.RemoveCollectibles(Color.red, requiredPayment, this.transform.position);
		StartCoroutine(StartDie(1f));
		Debug.Log("LOGTEST");
	}
	
	void Setup(){
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
		_AvatarT = GameObject.FindGameObjectWithTag("avatar").transform;
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		_Manager = ChromatoseManager.manager;
		_PayZoneScript = GetComponentInChildren<MiniBoss_PayZone>();
		_ShootingZone = GetComponentInChildren<MiniBossShootingZone>();
		
		if(GetComponentsInChildren<tk2dAnimatedSprite>() != null){
			myFlames = GetComponentsInChildren<tk2dAnimatedSprite>();
		}
		StartCoroutine(SetAnim());
	}
	
	void Die(){
		Destroy(this.gameObject);
	}
	
	IEnumerator SetAnim(){
		yield return new WaitForSeconds(0.5f);
		_MainAnim.SetSprite("bossFlame1");
		_MainAnim.Play("bossFlame1");
	}
	IEnumerator StartDie(float delai){
		yield return new WaitForSeconds(delai);
		_CanDie = true;
	}
}