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
	private bool alreadyShooten = false;
	private bool _OnDie = false;
	public int shotCount = 0;
	

	void Start () {
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
		_PayZoneScript = GetComponentInChildren<MiniBoss_PayZone>();
		_ShootingZone = GetComponentInChildren<MiniBossShootingZone>();
		
		StartCoroutine(SetAnim());
		
		StartCoroutine(Setup());
	}
	
	void Update () {
		
		if(_OnDie){
			_MainAnim.color -= new Color (0, 0, 0, 0.01f);
			if(_MainAnim.color.a <= 0){
				Destroy(this.gameObject);
			}
		}
	
		if(_ShootingZone != null){
			if(_PayZoneScript.inPayZone){
				HUDManager.hudManager.UpdateAction(Actions.Release, DelegateActionTest);
				_AvatarScript.requieredPayment = requiredPayment.ToString();
				_AvatarScript.wantFightBoss = true;
			}
			
			if(_CanDie){
				_FadingCounter -= _FadeRate;
				_MainAnim.color = new Color(0, 0, 0 ,_FadingCounter);
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
		
		if(shotCount >= requiredPayment){
			StartCoroutine(StartDie(2f));
			Debug.Log("MiniBoss devrait Die");
		}
	}
	
	public void ResetMiniBoss(){
		_KnockBack = 50f;
		_FadeRate = 0.005f;
		_FadingCounter = 1;
		_ShooterCounter = 0;
	
		beingExtinguished = false;
		_CanDie = false;
		alreadyShooten = false;
		StopAllCoroutines();
		shotCount = 0;
	}
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "avatar"){return;}
		
		_Manager.Death();
	}
	
	void DelegateActionTest(){
		if(StatsManager.redCollDisplayed >= requiredPayment){
			if(!alreadyShooten){
				_Manager.lastBoss = this.gameObject;
				_Manager.ShootRedCollOnMini(requiredPayment, transform.position);
				alreadyShooten = true;
			}
		}
	}
	
	IEnumerator Setup(){
		yield return new WaitForSeconds(0.1f);
		_AvatarT = GameObject.FindGameObjectWithTag("avatar").transform;
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		_Manager = ChromatoseManager.manager;
	}
	
	void Die(){
		_AvatarScript.WantsToRelease = false;
		_MainAnim.Play("miniBossDie", 0f);
		_OnDie = true;
		//StartCoroutine(DelaiToDestroy ());
		_MainAnim.animationCompleteDelegate = DestroyThis;
	}
	
	void DestroyThis(tk2dAnimatedSprite sprite, int clipId){
		//StartCoroutine(DelaiToDestroy());
		Destroy(this.gameObject);
	}
	
	IEnumerator SetAnim(){
		yield return new WaitForSeconds(0.5f);
		_MainAnim.SetSprite("bossFlame1");
		_MainAnim.Play("bossFlame1");
	}
	IEnumerator StartDie(float delai){
		yield return new WaitForSeconds(delai);
		Die ();
		Debug.Log("MiniBossDie");
	}
	IEnumerator DelaiToDestroy(){
		yield return new WaitForSeconds(1.0f);
		Destroy(this.gameObject);
	}
	IEnumerator SecondDie(){
		yield return new WaitForSeconds(0.5f);
		_AvatarScript.WantsToRelease = false;
		_MainAnim.Play("miniBossDie", 0f);
		_MainAnim.animationCompleteDelegate = DestroyThis;
	}
}
