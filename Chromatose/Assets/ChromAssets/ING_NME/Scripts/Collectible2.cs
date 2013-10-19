using UnityEngine;
using System.Collections;
using ChromaConst;

public class Collectible2 : MonoBehaviour {

	public enum _ColorCollectible{
		Red, White, Blue
	}
	
	public _ColorCollectible colorCollectible;
	
	private ChromatoseManager _Manager;
	private Avatar _AvatarScript;
	private tk2dAnimatedSprite _MainAnim;
	
	private Color myColor;
	private Vector2 _RandomVelocity;
	private bool _Effect = false;
		public bool effect{
			get{return _Effect;}
			set{_Effect = value;}
		}
	private bool popped = false;
	private bool _StartCalculated = false;
	private bool _Retour = false;
	private bool _PickForced = false;
	
	private float startTime;
	private float journeyTime = 5.0f;
	private float random1;
	private float random2;
	private float random3;
	
	private string idleAnimWhite = "wColl_idle";
	private string takeAnimWhite = "wColl_pickedUp";
	private string loseAnimWhite = "wColl_lose";
	private string idleAnimRed = "rColl_idle";
	private string takeAnimRed = "rColl_pickedUp";
	private string loseAnimRed = "rColl_idle";
	private string idleAnimBlue = "bColl_idle";
	private string takeAnimBlue = "bColl_pickedUp";
	private string loseAnimBlue = "bColl_lose";
	
	private Vector2 randomVelocity = Vector2.zero;
	private Vector3 randomPos = Vector3.zero;
	private Vector3 _RedCollectorPos = Vector3.zero;
		public Vector3 redCollectorPos{
			get{return _RedCollectorPos;}
			set{_RedCollectorPos = value;}
		}
	private Vector3 bossSpotForBullet;
	
	void Start () {
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
		float rndOffsetTime = Random.Range(0f, 1f);
		
		switch(colorCollectible){
		case _ColorCollectible.White:
			myColor = Color.white;
			_MainAnim.Play(idleAnimWhite, rndOffsetTime);
			break;
		case _ColorCollectible.Red:
			myColor = Color.red;
			_MainAnim.Play(idleAnimRed, rndOffsetTime);
			
			break;
		case _ColorCollectible.Blue:
			myColor = Color.blue;
			_MainAnim.Play(idleAnimBlue, rndOffsetTime);
			
			break;
		}
		
		Setup ();	
		
		StartCoroutine(ForceToBePick());
	}
	
	// Update is called once per frame
	void Update () {
	
		
		if(_PickForced && _Effect){
			_Effect = false;
		}
		
		if(_Effect && myColor == Color.red){
			
			if(!_StartCalculated){
				Setup ();
				startTime = Time.time;
				random1 = Random.Range(0.35f, 0.5f);
				random2 = Random.Range(0.5f, 1.25f);
				random3 = Random.Range(1f, 100f);
				
				Vector3 CollectorTempPos = GameObject.FindGameObjectWithTag("spotForBullet").transform.position;
				Vector2 randomVelocity1 = Random.insideUnitCircle.normalized * Random.Range(10, 70);
				Vector2 randomVelocity2 = Random.insideUnitCircle.normalized * Random.Range(15, 140);					
				bossSpotForBullet = CollectorTempPos + (Application.loadedLevel == 12? (Vector3)randomVelocity2: (Vector3)randomVelocity1);
				
				Transform _AvatarT = GameObject.FindGameObjectWithTag("avatar").transform;
				randomVelocity = Random.insideUnitCircle.normalized * Random.Range(40, 65);
				randomPos = _AvatarT.position + (Vector3)randomVelocity;
				
				_StartCalculated = true;
			}
			
			if(!_Retour){
				Vector3 center;
				if(Application.loadedLevel != 12){
					center = (gameObject.transform.position + _RedCollectorPos) * random1;
				}
				else{
					center = (gameObject.transform.position + GameObject.FindGameObjectWithTag("Boss").transform.position) * random1;
				}
		        center -= new Vector3(0, random2, 0);
		        Vector3 riseRelCenter = gameObject.transform.position - center;
		        Vector3 setRelCenter = _RedCollectorPos - center;
		        float fracComplete = (Time.time - startTime) / journeyTime;
		        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
		        transform.position += center;
			}
			else{
				
				Vector3 center = (bossSpotForBullet + _RedCollectorPos) * random1;
		        center -= new Vector3(0, random2, 0);
		        Vector3 riseRelCenter = _RedCollectorPos - center;
		        Vector3 setRelCenter = bossSpotForBullet - center;
		        float fracComplete = (Time.time - startTime) / journeyTime;
		        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
	        	transform.position += center;
				
			}
		}
		
		if(Vector3.Distance(gameObject.transform.position, _RedCollectorPos) < 15 && !_Retour){
			_Retour = true;
		}
		if(Vector3.Distance(gameObject.transform.position, Application.loadedLevel != 12? randomPos: bossSpotForBullet) < 15 && _Retour && _Effect){
			_Effect = false;
			Debug.Log("Redevient Coll");
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "avatar" || _Effect)return;
		if(_AvatarScript!=other.GetComponent<Avatar>() || _AvatarScript == null){
			_AvatarScript = other.GetComponent<Avatar>();
		}
		
		if(!ChromatoseManager.manager.CollAlreadyAdded){
			switch(colorCollectible){
			case _ColorCollectible.White:
				TakeCollectible();
				break;
			case _ColorCollectible.Red:
				_AvatarScript.OnRedCol = true;
				
				if(_AvatarScript.curColor == Color.red){
					TakeCollectible();
				}
				break;
			case _ColorCollectible.Blue:
				if(_AvatarScript.curColor == Color.blue){
					TakeCollectible();
				}			
				break;
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag != "avatar" || _Effect)return;
		
		if(_AvatarScript == null){
			_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		}
		
		switch(colorCollectible){
		case _ColorCollectible.Red:
			_AvatarScript.OnRedCol = false;
			break;
		}
		
		HUDManager.hudManager.OffAction();
	}
	
	IEnumerator Setup(){
		yield return new WaitForSeconds(0.5f);
		_Manager = ChromatoseManager.manager;
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
				
	}
	
	void TakeCollectible(){
		if(!popped){
			ChromatoseManager.manager.AddCollectible(myColor);
			_MainAnim = GetComponent<tk2dAnimatedSprite>();
			
			switch(colorCollectible){
			case _ColorCollectible.White:
				_MainAnim.Play(takeAnimWhite);
				if(!_Effect){
					MusicManager.soundManager.PlaySFX(54);
				}
				break;
			case _ColorCollectible.Red:
				if(!_Effect){
					MusicManager.soundManager.PlaySFX(47);
				}
				_MainAnim.Play(takeAnimRed);
				break;
			case _ColorCollectible.Blue:
				if(!_Effect){
					MusicManager.soundManager.PlaySFX(4);
				}
				_MainAnim.Play(takeAnimBlue);
				break;
			}
			_MainAnim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			_MainAnim.animationCompleteDelegate = Die;
			popped = true;
		}
	}
	
	public void PayEffect(){
		_MainAnim = GetComponent<tk2dAnimatedSprite>();
		switch(colorCollectible){
		case _ColorCollectible.White:
			_MainAnim.SetSprite("wColl_lose");
			_MainAnim.Play("wColl_lose");
			_MainAnim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			_MainAnim.animationCompleteDelegate = Die;
			break;
		case _ColorCollectible.Red:
			_MainAnim.Play(loseAnimRed);
			
			
			break;
		case _ColorCollectible.Blue:
			_MainAnim.SetSprite("bColl_pickedUp");
			_MainAnim.Play("bColl_pickedUp");
			_MainAnim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			_MainAnim.animationCompleteDelegate = Die;
			break;
		}	
	}
	
	
	public void Die(tk2dAnimatedSprite sprite, int index){
		switch(colorCollectible){
		case _ColorCollectible.Red:
			_AvatarScript.OnRedCol = false;
			break;
		}
		
		StartCoroutine(DelaiToDie());
		//transform.position = new Vector3(transform.position.x, transform.position.y, -3000);
	}
	
	IEnumerator DelaiToDie(){
		yield return new WaitForSeconds(0.7f);
		this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -3000);
	}	
	IEnumerator ForceToBePick(){
		yield return new WaitForSeconds(10f);
		_PickForced = true;
	}
}
