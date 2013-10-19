using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum _AvatarTypeEnum{
	avatar, shavatar
}

[System.Serializable]
public class Avatar : MainManager{
	
	private _AvatarTypeEnum avatarType = _AvatarTypeEnum.avatar;
	public _AvatarTypeEnum avaTypeAccess { get { return avatarType; } set { avatarType = value; } }

	private MainManager _MainManager;
	private tk2dSprite spriteInfo;
	
	private Color partColor = Color.white;
	public Color AvatarColor{
		get{return partColor;}
	}
	private bool debug = false;
	
	private Color _AvatarColor;
	private Color _CurColor;
		public Color curColor{
			get{return _CurColor;}
			set{_CurColor = value;}
		}
	
	private bool bubbleGrowed = false, bubbleShrinked = false;
	
	private bool _Colored = false;
	private bool _CantPlaySpeedFX = false;
	private float _ColorCounter = 0;
	private int _SpriteIndex = 1;
	private string _ColorFadeString = "";
	private string _PlayerFadeString = "";
	
	private float loseRate = 6f;
	private float loseTimer = 0f;
	
	protected Vector2 velocity;
	private Vector2 previousVelocity;
	protected int direction;
	protected float[] angles = new float[16];
	
	public static int curEnergy = 50;				//MY HP (ENERGY)
	//public static TankStates[,] tankStates;
	
	[System.NonSerializedAttribute]
	public Movement movement;
	protected float basicTurnSpeed;
	protected float basicMaxSpeed;
	protected float basicAccel;
	
		//AVATAR COLLECTION
	public tk2dSpriteCollectionData normalCollection;
	public tk2dSpriteCollectionData paleCollection;
	public tk2dSpriteCollectionData afterImageCollection;
	public tk2dSpriteCollectionData particleCollection;
	public tk2dSpriteCollectionData coloredCollection;
	
		//SHAVATAR COLLECTION
	public tk2dSpriteCollectionData shavaNormalCollection;
	public tk2dSpriteCollectionData shavaPaleCollection;
	public tk2dSpriteCollectionData shavaAfterImageCollection;
	public tk2dSpriteCollectionData shavaParticleCollection;
	public tk2dSpriteCollectionData shavaColoredCollection;
	
	public tk2dAnimatedSprite givePart;
	
	public tk2dSpriteAnimation partAnimations;
	
	private Vector3 _InitPos = Vector3.zero;
		public Vector3 initPos{
			get{return _InitPos;}
			set{_InitPos = value;}
		}
	
	private LoseAllColourParticle loseAllColourPart;
	private List<MovementLines> accelParts = new List<MovementLines>();
	private TurboParticle turboPart = null;
	private List<LoseColourParticle> loseColourPart = new List<LoseColourParticle>();
		private int loseColourPartDrop = 10;
		private int loseColourPartCounter = 0;
		private int partDropMin = 5;
		private int partDropMax = 13;
	
	private List<GiveColourParticle> giveColourParts = new List<GiveColourParticle>();

	public float accelPartTimer = 0f;
	public float accelPartTiming = 0.3f;
	public float accelPartTimingBase = 0.3f;
	
	private Eye travisMcGee;
	private Eye petitChu;
	private SpeechBubble bubble;

	//inputs. Up, left and right will also work, but getW seems intuitive to me
	/*
	protected bool getW;
	protected bool getA;
	protected bool getS;	//This is there for solidarity
	protected bool getD;
	*/
	
	private bool getForward;	public bool getforward { get { return getForward; } set { getForward = value; } }
	private bool getLeft;		public bool getleft { get { return getLeft; } set { getLeft = value; } }
	private bool getRight;		public bool getright { get { return getRight; } set { getRight = value; } }
	
	private bool getSpace;	public bool getspace { get { return getSpace; } set { getSpace = value; } }
	/*
	private bool _SpaceBarActive;
		public bool spaceBarActive{
			get{return _SpaceBarActive;}
			set{_SpaceBarActive = value;}
		}
	*/
	private int currentSubimg;
	private string spritePrefix = "Player";
	private int rotCounter = 0;
	private int rotAnimTiming = 10;
	
	private int noRotSubimg = 1;
	private int ccw1 = 2;
	private int ccw2 = 3;
	private int cw1 = 4;
	private int cw2 = 5;
	
	private string intForBoss = "";
	public string requieredPayment{
		get{return intForBoss;}
		set{intForBoss = value;}
	}
	
	private bool hurt;				//other properties! Getting hurt and stuff
	public bool Hurt{
		get{ return hurt;}
		set{ hurt = value;}
	}
	private int hurtTimer = 0;
	private int hurtTiming = 60;
	private int blinkOffAt = 20;
	private int blinkOnAt = 10;
	private List<GameObject> blobsHit = new List<GameObject>();
	private bool invisible = false;
	public bool SetInvisible{
		set{ invisible = value; }
	}
	public int Invisible{
		get{ return invisible ? 0 : 1;}
	}
	private bool inBlueLight = false;
	public bool InBlueLight{
		get { return inBlueLight; }
		set { inBlueLight = value; }
	}
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<---------Variable Diff. TimeTrial--------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	private bool _TimeTrialActivated = false;
	
	
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<----------Variable Diff. NoDeath---------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	private bool _NoDeathModeActivated = false;
	
	
	
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<--------------Speed boosts!!-------------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	private GameObject[] speedBoosts;
	private Rect[] speedBoostAreas;
	private bool canControl = true;
	private bool _OnLoseColorFX = false;
	private int speedBoostDist = 55;
	private float speedBoostMod = 2.0f;
	private float speedBoostCur = 1f;
	private int speedBoostCounter = 0;
	private int speedBoostMax = 50;
		
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<-----------Check for bubbles!------------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	private bool onRedCol;
	public bool OnRedCol{
		get { return onRedCol; }
		set { onRedCol = value; }
	}
	private bool onRedWell;
	public bool OnRedWell{
		get { return onRedWell; }
		set { onRedWell = value; }
	}
	private bool hasChangedColour = false;
	private bool atDestructible = false;
	public bool AtDestructible{
		get { return atDestructible; }
		set { atDestructible = value; }
	}
	private bool hasDestroyed = false;
	public bool HasDestroyed{
		get { return hasDestroyed; }
		set { hasDestroyed = false; }
	}
	private bool wantsToRelease = false;
	public bool WantsToRelease{
		get { return wantsToRelease; }
		set { wantsToRelease = value; }
	}
	private bool _WantFightBoss = false;
	public bool wantFightBoss{
		get{return _WantFightBoss;}
		set{_WantFightBoss = value;}
	}
	
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<-----------Dependent objects!!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	[System.NonSerializedAttribute]			//my transform and renderer and material info
	public Transform t;
	
	private GameObject outline;				//my after-image (outline) info (and refill)
	private GameObject outlinePointer;
	private int teleportCost = 2;
	private bool hasOutline = false;
	public bool HasOutline{
		get { return hasOutline; }
	}
	
	private GameObject[] allTheFaders;
	
	[System.NonSerializedAttribute]
	public Texture avatarOutlineTexture;
											//Keeping track of where I get knocked back to
	private Transform myKnockTarget;
	
	
#region PARTICLE CLASSES
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Particle classes!!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	#region CLASS DEATHANIM
	public class DeathAnim {
		tk2dAnimatedSprite anim;
		public GameObject go;
		Transform t;
		Avatar avatar;
		public DeathAnim (){
			go = new GameObject("Death Animation!");
			t = go.transform;
			avatar = GameObject.FindWithTag("avatar").GetComponent<Avatar>();
			t.position = avatar.transform.position;
			t.rotation = avatar.transform.rotation;
			tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(go, avatar.particleCollection, 0);
			anim = go.GetComponent<tk2dAnimatedSprite>();
			anim.anim = avatar.partAnimations;
		}
		public void PlayDeath(tk2dAnimatedSprite.AnimationCompleteDelegate deleMethod){
			anim.color = Color.white;
			anim.Play("clip_avatarDeath");
			anim.animationCompleteDelegate = deleMethod;
			anim.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			avatar.CannotControlFor(0.5f);
		}
	}
	#endregion

	#region CLASS EYE
	private class Eye {
		private tk2dSprite spriteInfo;
		private GameObject go;
		private Transform t;
		private Transform avatarT;
		private Vector3 offset = new Vector3(-5, 0, -1);
		private MeshRenderer mRender;
		
		private float timer = 0;
		private float leftCounter = 0;
		private float rightCounter = 0;
		private float timeRate = 10;
		private float moveRate = 1;
		
		private tk2dSpriteCollectionData avaEye;
		private tk2dSpriteCollectionData shavaEye;
		
		public Eye (Transform avatarT, tk2dSpriteCollectionData spriteDataAva, tk2dSpriteCollectionData spriteDataShava, bool shava){
			
			if(!shava){
				go = new GameObject("AvatarEye");
				tk2dSprite.AddComponent<tk2dSprite>(go, spriteDataAva, "eye");
			}
			else{
				go = new GameObject("ShavatarEye");
				tk2dSprite.AddComponent<tk2dSprite>(go, spriteDataShava, "eye");
			}
			
			
			mRender = go.GetComponent<MeshRenderer>();
			t = go.transform;
			spriteInfo = go.GetComponent<tk2dSprite>();
			this.avatarT = avatarT;
			t.parent = avatarT;
			t.localPosition = offset;
			t.localRotation = Quaternion.identity;
			/*
			go.AddComponent<eyeColorChange>();
			go.GetComponent<eyeColorChange>().avaScript = avatarT.GetComponent<Avatar>();
			go.GetComponent<eyeColorChange>().avaEye = spriteDataAva;
			go.GetComponent<eyeColorChange>().shavaEye = spriteDataShava;*/
		}
		
		public void RenderEye(bool activeEye){
				mRender.enabled = activeEye;
		}
		
		public void EyeFollow() {
			//Debug.Log("OO");
			
			timer += Time.deltaTime;
			
			if(Input.GetKey(KeyCode.Q)){
				if(leftCounter < timeRate){
					t.localPosition = new Vector3(-5.5f, 0.75f, -1);
					leftCounter += moveRate;
				}
				else if(leftCounter >= timeRate){
					t.localPosition = new Vector3(-6, 1.5f, -1);
				}
				timer = 0;
			}
			else if(Input.GetKey(KeyCode.W)){
				if(rightCounter < timeRate){
					t.localPosition = new Vector3(-5.5f, -1.5f, -1);
					rightCounter += moveRate;
				}
				else if(leftCounter >= timeRate){
					t.localPosition = new Vector3(-6, -3f, -1);
				}
				timer = 0;
				
				
			}
			else if(Input.GetKey(KeyCode.O)){
				if(leftCounter >= 1){
					t.localPosition = new Vector3(-5.5f, 0.75f, -1);
					leftCounter -= moveRate;
				}
				else if(rightCounter >= 1){
					t.localPosition = new Vector3(-5.5f, -1.5f, -1);
					rightCounter -= moveRate;
				}
				else{				
					t.localPosition = new Vector3(-5, 0, -1);
				}
				timer = 0;
			}
			
			if(timer >= 3){
				if(leftCounter >= 1){
					t.localPosition = new Vector3(-5.5f, 0.75f, -1);
					leftCounter -= moveRate;
				}
				else if(rightCounter >= 1){
					t.localPosition = new Vector3(-5.5f, -1.5f, -1);
					rightCounter -= moveRate;
				}
				else{				
					t.localPosition = new Vector3(-5, 0, -1);
				}
			}
		}
		
		public void Blend(float r, float g, float b){
			float red = r;
			float green = g;
			float blue = b;
			
			float lowest = Mathf.Min(r, g, b);
			Color blendColor;
			int colourIndex = (r == 255? 1 : 0) + (g == 255? 2 : 0) + (b == 255? 4 : 0);
			switch (colourIndex){
			case 0:
				blendColor = Color.white;
				break;
			case 1:
				blendColor = Color.red;
				break;
			case 2:
				blendColor = Color.green;
				break;
			case 3:
				blendColor = new Color(1f, 1f, 0, 1f);
				break;
			case 4:
				blendColor = Color.blue;
				break;
			case 5:
				blendColor = Color.magenta;
				break;
			case 6:
				blendColor = Color.cyan;
				break;
			case 7:
				blendColor = Color.white;
				break;
			default:
				blendColor = Color.white;
				Debug.LogWarning("I'm making it white but I really don't understand how you got here tbh");
				break;
			}
			if (lowest > 220){
				if (lowest % 10 < 4){
					blendColor = Color.white;
				}
			}
			spriteInfo.color = blendColor;
			
		}
	}
	#endregion
	
	#region CLASS MOVEMENT
	private class MovementLines {
		float baseSpeed = 50f;
		float fadeRate = 0.05f;
		float fadeAfter = 0.7f;
		Vector3 velocity;
		
		int offset = 55;
		
		GameObject go;
		Transform t;
		tk2dAnimatedSprite spriteInfo;
		
		
		public MovementLines(Transform avatarT, Vector3 direction, float speedModifier, tk2dSpriteCollectionData spriteData, tk2dSpriteAnimation anim){
			velocity = direction.normalized * baseSpeed * speedModifier * Time.deltaTime;
			go = new GameObject("MovementLine");
			t = go.transform;
			t.position = avatarT.position + direction.normalized * offset + Vector3.forward;
			
			t.rotation = avatarT.rotation;
			tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(go, spriteData, "part_avatarAccel0001");
			spriteInfo = go.GetComponent<tk2dAnimatedSprite>();
			spriteInfo.anim = anim;
			spriteInfo.clipId = spriteInfo.GetClipIdByName("clip_avatarAccel");
			spriteInfo.Play();
			spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		}
		
		public bool Main(){
			if (t)
				t.Translate(velocity, Space.World);
			
			if (fadeAfter > 0){
				fadeAfter -= Time.deltaTime;
			}
			else{
				spriteInfo.color = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, spriteInfo.color.a - fadeRate);
				if (spriteInfo.color.a <= 0){
					GameObject.Destroy(go);
					return true;
				}
			}	
			return false;
		}
	}
	#endregion
	
	#region CLASS TURBOPARTICLE
	private class TurboParticle {
		GameObject go = new GameObject("TurboPart");
		Transform t;
		Transform avatarT;
		tk2dAnimatedSprite spriteInfo;
		
		public TurboParticle(tk2dSpriteCollectionData colData, tk2dSpriteAnimation animData, Transform avatarT){
			t = go.transform;
			this.avatarT = avatarT;
			tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(go, colData, "part_avatarTurbo0001");
			spriteInfo = go.GetComponent<tk2dAnimatedSprite>();
			spriteInfo.anim = animData;
			spriteInfo.clipId = spriteInfo.GetClipIdByName("clip_avatarTurbo");
			spriteInfo.Play();
			spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		}
		
		public void Go(){
			t.position = avatarT.position;
			go.SetActive(true);
		}
		
		public void Stop(){
			if(go){
				go.SetActive(false);
			}
		}
	}
	#endregion
	
	#region CLASS GIVECOLOURPARTICLE
	private class GiveColourParticle {
		List<GameObject> gos = new List<GameObject>();
		Transform target;
		Transform origin;
		//tk2dAnimatedSprite spriteInfo;
		int shotsLeft = 4;
		float timer = 0f;
		float frequency = 0.07f;
		float speed = 400f;
		tk2dSpriteCollectionData colData;
		
		
		public GiveColourParticle(tk2dSpriteCollectionData colData, Transform target, Transform origin){
			this.colData = colData;
			this.target = target;
			this.origin = origin;
		}
		
		public bool Main(){
			timer += Time.deltaTime;
			if (timer >= frequency && shotsLeft > 0){
				Shoot();
				shotsLeft --;
				timer = 0;
			}
			
			GameObject toDestroy = null;
			foreach (GameObject go in gos){
				
				go.transform.rotation = Quaternion.LookRotation(Vector3.forward, target.position - go.transform.position);
				go.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
				if ((go.transform.position - target.position).magnitude < 10){
					toDestroy = go;
				}
			}
			if (toDestroy != null){
				gos.Remove(toDestroy);
				Destroy(toDestroy);
				if (gos.Count == 0)
					return true;
			}
			return false;
		}
		
		public void Shoot(){
			
				GameObject newGuy = new GameObject("colourGivePart" + shotsLeft);
				gos.Add(newGuy);
				tk2dSprite.AddComponent<tk2dSprite>(newGuy, colData, "part_avatarGiveColor");
				
				tk2dSprite spriteInfo = newGuy.GetComponent<tk2dSprite>();
				
				newGuy.transform.position = origin.position + Vector3.forward;
				spriteInfo.color = Color.red;
		}
	}
	#endregion
	
	#region CLASS REFILLCOLOURPARTICLE
					//NOT BEING USED ATM
	private class RefillColourParticle {
		List<GameObject> gos = new List<GameObject>();
		Transform avatarT;
		//tk2dAnimatedSprite spriteInfo;
		int incrementMin = 42;
		int incrementMax = 47;
		int maxParts = 8;
		int offset = 25;
		tk2dSpriteCollectionData colData;
		tk2dSpriteAnimation animData;
		
		
		public RefillColourParticle(tk2dSpriteCollectionData colData, tk2dSpriteAnimation animData, Transform avatarT){
			this.colData = colData;
			this.animData = animData;
			this.avatarT = avatarT;
		}
		
		public void Emit(Color colour){
			for (int i = 0; i < maxParts; i ++){
				GameObject newGuy = new GameObject("RefillPart" + i);
				gos.Add(newGuy);
				tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(newGuy, colData, "part_avatarRefillColor0001");
				
				tk2dAnimatedSprite spriteInfo = newGuy.GetComponent<tk2dAnimatedSprite>();
				
				spriteInfo.anim = animData;
				 
				spriteInfo.Play(spriteInfo.GetClipByName("clip_avatarRefillColour"), 0);
				spriteInfo.color = colour;
				spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
				
				newGuy.transform.position = avatarT.position;
				newGuy.transform.rotation = Quaternion.identity;
				newGuy.transform.Rotate(new Vector3(0, 0, i * Random.Range(incrementMin, incrementMax)));
				newGuy.transform.Translate(Vector3.right * offset);
				
				if (i == 0){
					spriteInfo.animationCompleteDelegate = End;
				}
					
			}
		}
		
		public void End(tk2dAnimatedSprite sprite, int ClipId){
			foreach (GameObject go in gos){
				GameObject.Destroy(go);
			}
		}
	}
	#endregion
	
	#region CLASS LOASEALLCOLOURPARTICLE
	public class LoseAllColourParticle {
		GameObject go = new GameObject("LoseColourPart");
		Transform t;
		tk2dAnimatedSprite spriteInfo;
		int offset = 17;
		float fadeRate = 0.05f;
		float fadeAfter = 1.2f;
		
		public LoseAllColourParticle(tk2dSpriteCollectionData colData, tk2dSpriteAnimation anim, Transform avatarT, Color blendColor){
			t = go.transform;
			t.position = avatarT.position + (-avatarT.right) * offset + (Vector3)Random.insideUnitCircle * offset + Vector3.forward;
			
			
			string spriteName = "part_avatarLosingAllColor0001";
			tk2dAnimatedSprite.AddComponent<tk2dAnimatedSprite>(go, colData, spriteName);
			spriteInfo = go.GetComponent<tk2dAnimatedSprite>();
			spriteInfo.anim = anim;
			spriteInfo.Play(spriteInfo.GetClipByName("clip_avatarLoseAllColour"), 0);
			spriteInfo.color = blendColor;
			spriteInfo.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
		}
		public bool Fade(){
			if (fadeAfter > 0){
				fadeAfter -= Time.deltaTime;
			}
			else{
				Color newCol = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, spriteInfo.color.a - fadeRate);
				spriteInfo.color = newCol;
				if (spriteInfo.color.a <= 0){
					GameObject.Destroy(go);
					return true;
				}
			}	
			return false;
		}
	}
	#endregion
	
	#region CLASS LOSECOLOURPARTICLE
	private class LoseColourParticle {
		GameObject go = new GameObject("LoseColourPart");
		Transform t;
		tk2dSprite spriteInfo;
		int offset = 17;
		float fadeRate = 0.05f;
		float fadeAfter = 1.2f;
		int maxNumber = 13;
		
		
		public LoseColourParticle(tk2dSpriteCollectionData colData, Transform avatarT, Color blendColor){
			t = go.transform;
			t.position = avatarT.position + (-avatarT.right) * offset + (Vector3)Random.insideUnitCircle * offset + Vector3.forward;
			
			
			int index = Random.Range(1, maxNumber);
			string spriteName = "part_avatarLosingColor00" + (index < 10? "0" + index.ToString() : index.ToString());
			tk2dSprite.AddComponent<tk2dSprite>(go, colData, spriteName);
			spriteInfo = go.GetComponent<tk2dSprite>();
			
			spriteInfo.color = blendColor;
		}
		
		public bool Fade(){
			if (fadeAfter > 0){
				fadeAfter -= Time.deltaTime;
			}
			else{
				spriteInfo.color = new Color(spriteInfo.color.r, spriteInfo.color.g, spriteInfo.color.b, spriteInfo.color.a - fadeRate);
				if (spriteInfo.color.a <= 0){
					GameObject.Destroy(go);
					return true;
				}
			}	
			return false;
		}
	}
	#endregion
	
#endregion
	
	void OnLevelWasLoaded(){
		switch(Application.loadedLevel){
		case (1):
			StatsManager.spaceBarActive = false;
			break;
		}
	}

	void Start (){	
		
		_MainManager = MainManager._MainManager;
		
		t = this.transform;
		t.rotation = Quaternion.identity;
				
		movement = GetComponent<Movement>();
		curColor = Color.white;
		//_TimeTrialActivated = manager.TimeTrialMode;
		//_NoDeathModeActivated = manager.NoDeathMode;

		basicTurnSpeed = movement.rotator.rotationRate;
		basicAccel = movement.thruster.accel;
		basicMaxSpeed = movement.thruster.maxSpeed;
		
		_InitPos = transform.position;
		
		for (int i = 0; i < angles.Length; i++){
			angles[i] = i * 22.5f;
		}
		
												//initializing my particle objects, as necessary
		spriteInfo = GetComponent<tk2dSprite>();
		turboPart = new TurboParticle(particleCollection, partAnimations, t);
		accelPartTiming = basicAccel * Time.deltaTime;
		
		/*
		outlinePointer = new GameObject("OutlinePointer");		//make my outline pointer thing	TO DO remove this and other references to it. Martine hates it a lot... <_<
		tk2dSprite.AddComponent<tk2dSprite>(outlinePointer, spriteInfo.Collection, outlinePointerName);
		outlinePointer.renderer.enabled = false;*/
		//outlinePointer.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		
		speedBoosts = GameObject.FindGameObjectsWithTag("SpeedBoost");
		speedBoostAreas = new Rect[speedBoosts.Length];
		int counter = 0;
		foreach (GameObject sb in speedBoosts){
			List<float> xs = new List<float>();
			List<float> ys = new List<float>();
			Transform[] nodes = sb.GetAllComponentsInChildren<Transform>();
			foreach (Transform n in nodes){
				xs.Add(n.transform.position.x);
				ys.Add(n.transform.position.y);
			}
		
			xs.Sort();
			ys.Sort();
			int offset = 125;
			speedBoostAreas[counter] = new Rect(xs[0] - offset, ys[0] - offset, xs[xs.Count - 1] - xs[0] + offset * 2, ys[ys.Count - 1] - ys[0] + offset * 2);		//make a rectangle encasing the speed boost areas
			
			counter ++;
		}
		
													//I WILL NOW FIND THE NEAREST KNOCKBACK TARGET!
		myKnockTarget = VectorFunctions.FindClosestOfTag(t.position, "knockTarget", 1000000);
		
		allTheFaders = GameObject.FindGameObjectsWithTag("spriteFader");
		
		//MAKE ME AN EYE BABY
		travisMcGee = new Eye(t, particleCollection, shavaParticleCollection, false);
		petitChu = new Eye(t, particleCollection, shavaParticleCollection, true);
		bubble = new SpeechBubble (t, particleCollection);
		spriteInfo.Collection = normalCollection;
		StartCoroutine(LateCPCreation(1.0f));
		
	}
	
	void AfterImage(){
		if (!hasOutline){
			outline = new GameObject("Outline");
			outline.transform.rotation = t.rotation;
			outline.transform.position = t.position;
			
			switch(avatarType){
			case _AvatarTypeEnum.avatar:
				if(!_Colored){
					tk2dSprite.AddComponent<tk2dSprite>(outline, afterImageCollection, spriteInfo.CurrentSprite.name);
				}
				else{
					tk2dSprite.AddComponent<tk2dSprite>(outline, afterImageCollection, _PlayerFadeString);
				}
				break;
			case _AvatarTypeEnum.shavatar:
				if(!_Colored){
					tk2dSprite.AddComponent<tk2dSprite>(outline, shavaAfterImageCollection, spriteInfo.CurrentSprite.name);
				}
				else{
					tk2dSprite.AddComponent<tk2dSprite>(outline, shavaAfterImageCollection, _PlayerFadeString);
				}
				break;
			}
			
			hasOutline = true;
			
			foreach (GameObject go in allTheFaders){
				go.SendMessage("SaveStateForTP", SendMessageOptions.DontRequireReceiver);
			}
			//spriteInfo.SwitchCollectionAndSprite()
		}
		else{
			
			t.position = outline.transform.position;
			t.rotation = outline.transform.rotation;
			MusicManager.soundManager.PlaySFX(17);
			
			Destroy(outline);
			hasOutline = false;
			//velocity = Vector2.zero;				//TEST For now I like the idea of keeping your current movement for when you go back. 
			//movement.SetVelocity(velocity);		// But should we have you facing  the same direction?
			
			foreach (GameObject go in allTheFaders){
				go.SendMessage("LoadStateForTP", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void Update(){
												//Self-made checkpoints! Or whatever you want to call it		
		if(Input.GetKeyDown(KeyCode.Space) && StatsManager.spaceBarActive){
			AfterImage();
		}
	}
	
	void FixedUpdate(){
		
	
		switch(avatarType){
		case _AvatarTypeEnum.avatar:
			travisMcGee.EyeFollow();
			travisMcGee.RenderEye(true);
			petitChu.RenderEye(false);
			break;
		case _AvatarTypeEnum.shavatar:
			petitChu.EyeFollow();
			petitChu.RenderEye(true);
			travisMcGee.RenderEye(false);
			break;
		}
		
		
		
		
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<--------------Color Fading!--------------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		if(_Colored){
			
			if(!_OnLoseColorFX){
				MusicManager.soundManager.PlaySFX(12, true);
				_OnLoseColorFX = true;
			}
			
			loseTimer += Mathf.Min(velocity.magnitude, basicMaxSpeed * Time.deltaTime);
			
			if (loseTimer >= loseRate){
				loseTimer = 0f;
				//colour.r = _CurColor == Color.red ? colour.r : colour.r - 1;
				//colour.b = _CurColor == Color.blue ? colour.b : colour.b - 1;
				loseColourPartCounter ++;
				//Debug.Log("Je passe ici");
				
			}
			if (loseColourPartCounter >= loseColourPartDrop){
				loseColourPartCounter = 0;
				loseColourPartDrop = Random.Range(partDropMin, partDropMax);
				loseColourPart.Add(new LoseColourParticle(particleCollection, t, _CurColor));
				
			}
		
		//colour.r = Mathf.Clamp(colour.r, 0, 255);
		//colour.b = Mathf.Clamp(colour.b, 0, 255);
			
			//DESCENDRE OU MONTER LA VITESSE DU FADE ICI
			_ColorCounter += Time.deltaTime * velocity.magnitude * 0.45f;
			
			
			if(_ColorCounter > 1){
				_SpriteIndex++;
				_ColorCounter = 0;
			}
			if(_SpriteIndex >= 20){
				_Colored = false;
				
				switch(avatarType){
				case _AvatarTypeEnum.avatar:
					spriteInfo.Collection = normalCollection;
					break;
				case _AvatarTypeEnum.shavatar:
					spriteInfo.Collection = shavaNormalCollection;
					break;
				}
				
				_SpriteIndex = 1;
				_CurColor = Color.white;
			}
			
			if(!getLeft && !getRight){	
				if(currentSubimg == ccw2){
					_PlayerFadeString = "Player2";
					spriteInfo.SetSprite("Player2_"+_ColorFadeString+_SpriteIndex);
				}
				else if(currentSubimg == cw2){
					_PlayerFadeString = "Player4";
					spriteInfo.SetSprite("Player4_"+_ColorFadeString+_SpriteIndex);
				}
				else{
					//NoRotation Sprite
					_PlayerFadeString = "Player1";
					spriteInfo.SetSprite("Player1_"+_ColorFadeString+_SpriteIndex);
					currentSubimg = noRotSubimg;
				}
			}
			else{
				if(getLeft){
					
					if(currentSubimg != ccw1 && currentSubimg != ccw2){
						_PlayerFadeString = "Player2";
						spriteInfo.SetSprite("Player2_"+_ColorFadeString+_SpriteIndex);
						currentSubimg = ccw1;
					}
					else{_PlayerFadeString = "Player3";
						spriteInfo.SetSprite("Player3_"+_ColorFadeString+_SpriteIndex);
						currentSubimg = ccw2;
					}
				}
				else{
					if(currentSubimg != cw1 && currentSubimg != cw2){
						_PlayerFadeString = "Player4";
						spriteInfo.SetSprite("Player4_"+_ColorFadeString+_SpriteIndex);
						currentSubimg = cw1;
					}
					else{_PlayerFadeString = "Player5";
						spriteInfo.SetSprite("Player5_"+_ColorFadeString+_SpriteIndex);
						currentSubimg = cw2;
					}
				}
			}
		}
				
		else if(!_Colored && HasOutline){
			
			switch(avatarType){
			case _AvatarTypeEnum.avatar:
				spriteInfo.Collection = paleCollection;
				break;
			case _AvatarTypeEnum.shavatar:
				spriteInfo.Collection = shavaPaleCollection;
				break;
			}
			
			//spriteInfo.SetSprite(spriteInfo.CurrentSprite.name);
			
		}
		else{
			
			if(_OnLoseColorFX){
				MusicManager.soundManager.StopSFX(12);
				_OnLoseColorFX = false;
			}
		
			
			switch(avatarType){
				case _AvatarTypeEnum.avatar:
				if(spriteInfo.Collection != normalCollection){
					_CurColor = Color.white;
					spriteInfo.Collection = normalCollection;
					currentSubimg = noRotSubimg;
					spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
				}
				break;
				case _AvatarTypeEnum.shavatar:
				if(spriteInfo.Collection != shavaNormalCollection){
					_CurColor = Color.white;
					spriteInfo.Collection = shavaNormalCollection;
					currentSubimg = noRotSubimg;
					spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
				}
				break;
			}
		}
		
		
		
		

								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<---------Check for speed boosts!!--------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		int rectCounter = 0;
		bool detectedSB = false;
		
		if (gameObject.tag == "avatar"){
			foreach (Rect rect in speedBoostAreas){
				
				if (rect.Contains((Vector2)t.position)){
					foreach (Transform node in speedBoosts[rectCounter].GetComponentsInChildren<Transform>()){
						if (node == speedBoosts[rectCounter].transform) continue;
						if (Vector2.Distance((Vector2)t.position, (Vector2)node.position) < speedBoostDist){
							detectedSB = true;
						}
					}
				}
				
				rectCounter ++;
			}
		}
		
		if (detectedSB){
			if(!_CantPlaySpeedFX){
				MusicManager.soundManager.PlaySFX(50);
				_CantPlaySpeedFX = true;
				StartCoroutine(PlaySpeedFXBool());
			}
			speedBoostCounter = Mathf.Min(speedBoostCounter + 1, speedBoostMax);
			turboPart.Go();
			movement.SetNewMoveStats(Mathf.Min(basicMaxSpeed + speedBoostMod * speedBoostCounter, basicMaxSpeed * speedBoostMod), basicAccel * speedBoostMod, basicTurnSpeed / speedBoostMod * 2);
		}
		else if (speedBoostCounter > 0){
			speedBoostCounter --;
			movement.SetNewMoveStats(Mathf.Max(basicMaxSpeed + speedBoostMod * speedBoostCounter, basicMaxSpeed), basicAccel, basicTurnSpeed);
			turboPart.Go();
		}
		else{
			movement.SetNewMoveStats(basicMaxSpeed, basicAccel, basicTurnSpeed);
			turboPart.Stop();
		}
		
			
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Handling Movement!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		
						//Translating the inputs to movement functions
		TranslateInputs(speedBoostCur);
		
		
		
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Other fun things!!------------>
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		
		if (hurt && Time.timeScale > 0){		//Am I hurt? Blink the sprite appropriately
			hurtTimer ++;
			
			if (hurtTimer % blinkOnAt == 0){
				
				invisible = false;
			}
			
			if (hurtTimer % blinkOffAt == 0){
				invisible = true;
			}
			
			if (hurtTimer >= hurtTiming){
				hurt = false;
				invisible = false;
				hurtTimer = 0;
			}
		}
		

															//drop particles where necessary
		if (getForward && ((!getLeft && !getRight) || (getLeft && getRight))){
			accelPartTimer += Time.deltaTime;
			if (accelPartTimer >= accelPartTiming){
				accelParts.Add(new MovementLines(t,-t.right, 1f, particleCollection, partAnimations));
				accelPartTimer = 0;
				accelPartTiming = velocity.magnitude *accelPartTimingBase;
			}
		}
		else{
			accelPartTimer = 0f;
			accelPartTiming = basicAccel * Time.deltaTime;
		}
		
															//running the main method on each particle
		MovementLines lineToRemove = null;
		
		foreach (MovementLines m in accelParts){
			bool removeLine = m.Main();
			if (removeLine)
				lineToRemove = m;
		}
		
		if (lineToRemove != null){
			accelParts.Remove(lineToRemove);
		}
		
		LoseColourParticle blotchToRemove = null;
		
		foreach (LoseColourParticle m in loseColourPart){
			try{
				bool removeBlotch = m.Fade();
				if (removeBlotch)
					blotchToRemove = m;
			}
			catch{
				Debug.Log("There was an error and I caught it!");
			}
			
		}
		
		if (blotchToRemove != null){
			accelParts.Remove(lineToRemove);
		}
		
		if (loseAllColourPart != null){
			bool dead = loseAllColourPart.Fade();
			if (dead){
				loseAllColourPart = null;
			}
		}
		
		
		foreach (GiveColourParticle part in giveColourParts){
			part.Main();
		}
		

		#region BUBBLE UPDATE
								//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
								//<------------Update my bubble!------------->
								//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		//switch(Application.loadedLevelName){
		//case ("Tutorial"):
			if (onRedCol){
				if(!bubbleGrowed){
					bubble.ShowBubbleFor("bubbleAvatar_grow", 0.3f);
					StartCoroutine(BubbleGrowed());
				}
				else if(bubbleGrowed){
					
					bubble.ShowBubbleFor("avatarBubble_redColor1", 0.3f);
					onRedCol = true;
				}
			}
			
			if (AvatarColor != Color.white && !hasChangedColour){
				hasChangedColour = true;
			}
			if (onRedWell && !hasChangedColour){
				bubble.ShowBubbleFor("avatarBubble_P1", 0.4f);
				onRedWell = true;
			}
			
			if (atDestructible && !hasDestroyed){
				bubble.ShowBubbleFor(AvatarColor==Color.red? "avatarBubble_P1" : "avatarBubble_redColor2", 0.2f);
				atDestructible = true;
			}
			
			if (wantsToRelease){
				bubble.ShowBubbleFor("avatarBubble_fire1", 0.3f);
				wantsToRelease = true;
			}
			
			if (_WantFightBoss){
				bubble.ShowBubbleFor("avatarBubble_" + intForBoss + "redcol", 0.3f);
				_WantFightBoss = false;
			}
			/*
			break;
		case ("Module1_Scene1"):
			if (onRedCol){					// && manager.GetCollectibles(Couleur.red) == 0){
				bubble.ShowBubbleFor("avatarBubble_redColor1", 0.3f);
				onRedCol = false;
			}
			
			if (AvatarColor != Color.white && !hasChangedColour){
				hasChangedColour = true;
			}
			if (onRedWell && !hasChangedColour){
				bubble.ShowBubbleFor("avatarBubble_P1", 0.4f);
				onRedWell = false;
			}
			
			if (atDestructible && !hasDestroyed){
				bubble.ShowBubbleFor(AvatarColor==Color.red? "avatarBubble_P1" : "avatarBubble_redColor2", 0.2f);
				atDestructible = false;
			}
			
			if (wantsToRelease){
				bubble.ShowBubbleFor("avatarBubble_fire1", 0.3f);
				wantsToRelease = false;
			}
						
			break;
		case ("GYM_CHU"):
			if (onRedCol){					// && manager.GetCollectibles(Couleur.red) == 0){
				if(!bubbleGrowed){
					bubble.ShowBubbleFor("avatarBubble_grow2", 0.15f);
					StartCoroutine(BubbleGrowed());
				}
				else if(bubbleGrowed){
					bubble.ShowBubbleFor("avatarBubble_redColor1", 0.4f);
					//onRedCol = false;
				}
			}
			else if (AvatarColor != Color.white && !hasChangedColour){				
				hasChangedColour = true;
			}
			else if (onRedWell && !hasChangedColour){
				if(!bubbleGrowed){
					bubble.ShowBubbleFor("avatarBubble_grow2", 0.15f);
					StartCoroutine(BubbleGrowed());
				}
				else if(bubbleGrowed){
					bubble.ShowBubbleFor("avatarBubble_P1", 0.4f);
					//onRedWell = false;
				}
			}
			else if (atDestructible && !hasDestroyed && _CurColor != Color.red){
				if(!bubbleGrowed){
					bubble.ShowBubbleFor("avatarBubble_grow2", 0.15f);
					StartCoroutine(BubbleGrowed());
				}
				else if(bubbleGrowed){				
					bubble.ShowBubbleFor(AvatarColor==Color.red? "avatarBubble_P1" : "avatarBubble_redColor2", 0.2f);
					//atDestructible = false;
				}
			}
			else if (wantsToRelease){
				if(!bubbleGrowed){
					bubble.ShowBubbleFor("avatarBubble_grow2", 0.15f);
					StartCoroutine(BubbleGrowed());
				}
				else if(bubbleGrowed){
					bubble.ShowBubbleFor("avatarBubble_fire1", 0.3f);
					//wantsToRelease = false;
				}
			}
			else{
				if(bubbleGrowed){
					bubbleGrowed = false;
				}
			}
			
			break;
		}*/
		bubble.Main();
#endregion
		
		
	end:
		if (previousVelocity != velocity)
			previousVelocity = velocity;
		if (!Gone){
			t.position = new Vector3(t.position.x, t.position.y, 0);
		}
		
		
		if (debug)
			Debug.Log("Yea dawg, verily this be the end");
		
	}
	
	void TranslateInputs(float multiplier){
		
		
		
		if (Time.timeScale == 0) return;
		bool gonnaRotate = false;
		bool clockwise = false;
		bool gonnaThrust = false;
		if (getForward){
			gonnaThrust = true;
		}
		
		if (getLeft){
			rotCounter --;
			gonnaRotate = true;
			clockwise = false;
		}
		
		if (getRight){
			rotCounter ++;
			if (getLeft){
				gonnaRotate = false;
				getLeft = false;
				getRight = false;
			}
			else{
				gonnaRotate = true;
				clockwise = true;
			}
		}
		
		rotCounter = Mathf.Clamp(rotCounter, -rotAnimTiming, rotAnimTiming);
		
		
		if (!getLeft && !getRight && currentSubimg != noRotSubimg){
			int newCounter = Mathf.Abs(rotCounter) - 1;
			rotCounter = Mathf.Clamp(rotCounter, -newCounter, newCounter);
			if (rotCounter == 0){
				currentSubimg = noRotSubimg;
				spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
			}
		}
		
		if (rotCounter > 0 && rotCounter < rotAnimTiming && currentSubimg != cw1){				//turn animation: clockwise
			currentSubimg = cw1;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		if (rotCounter >= rotAnimTiming && currentSubimg != cw2){
			currentSubimg = cw2;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		if (rotCounter < 0 && rotCounter > -rotAnimTiming && currentSubimg != ccw1){			//turn animation: counter-clockwise
			currentSubimg = ccw1;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		if (rotCounter <= -rotAnimTiming && currentSubimg != ccw2){
			currentSubimg = ccw2;
			spriteInfo.SetSprite(spritePrefix + currentSubimg.ToString());
		}
		
		
		if (gonnaRotate){																		//applying rotation and translation
			t.Rotate(this.movement.Rotate(clockwise));
			//t.rotation.eulerAngles = movement.rotator.Rotate(t.rotation.eulerAngles, clockwise);
		}
		velocity = this.movement.Displace(gonnaThrust);
		t.position += new Vector3(velocity.x, velocity.y, 0) * multiplier;
		
	}
	void TranslateInputs(){
		TranslateInputs(1f);
	}
	
															//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
															//<---------Knockback Management!--------->
															//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	void OnTriggerEnter(Collider box){
		if (box.tag == "knockTarget"){
			myKnockTarget = box.transform;
		}
	}
	
	
															//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
															//<--------Get/Setter functions----------->
															//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	
	public void CannotControlFor(float t){
		_CanControl = false;
		canControl = false;
		getForward = false;
		getLeft = false;
		getRight = false;
		Invoke("CanControl", t);
	}
	//Overriding the methods
	public void CannotControlFor(bool needTimes, float t){
		_CanControl = false;
		canControl = false;
		getForward = false;
		getLeft = false;
		getRight = false;
		if(needTimes){
			Invoke("CanControl", t);
		}
	}

	
	public void CanControl(){
		_CanControl = true;
		canControl = true;
		getForward = true;
		getLeft = true;
		getRight = true;
	}
	public void InverseControlRight(){
		canControl = !canControl;
		_CanControl = !_CanControl;
		getForward = !getForward;
		getLeft = !getLeft;
		getRight = !getRight;
	}
	
	public void Trigger(){
		
	}
	
	public void CancelOutline(){
		Destroy(outline);
		hasOutline = false;
	}

	
	public void LoseAllColourHidden(){
		if(_CurColor == Color.white)return;
		EmptyingBucket();
		_Colored = false;
		_CurColor = Color.white;
	}
	
	public void GiveColourTo(Transform target, Transform origin){
		giveColourParts.Add(new GiveColourParticle(particleCollection, target, origin));
	}
	

	
	void CreateCP(){
			GameObject _NewCP = Resources.Load("pre_checkpoint")as GameObject;
			GameObject _NewLevelCP = Instantiate(_NewCP, this.transform.position, Quaternion.identity)as GameObject;
			
			_NewLevelCP.transform.position = new Vector3(_NewLevelCP.transform.position.x, _NewLevelCP.transform.position.y, 2);	
		
			_NewLevelCP.GetComponent<BoxCollider>().enabled = false;
			_NewLevelCP.GetComponent<MeshRenderer>().enabled = false;
			
			_NewLevelCP.GetComponent<Checkpoint>().CallOnStart(_NewLevelCP);
	}
	
	void CreatFirstCP(){
		if(!ChromatoseManager.manager.FirstLevelCPDone){
			GameObject _NewCP = Resources.Load("pre_checkpoint")as GameObject;
			GameObject _FirstLevelCP = Instantiate(_NewCP, this.transform.position, this.transform.rotation)as GameObject;
			
			_FirstLevelCP.transform.position = new Vector3(_FirstLevelCP.transform.position.x, _FirstLevelCP.transform.position.y, 2);
			
			_FirstLevelCP.GetComponent<BoxCollider>().enabled = false;
			_FirstLevelCP.GetComponent<MeshRenderer>().enabled = false;
			
			_FirstLevelCP.GetComponent<Checkpoint>().CallOnStart(_FirstLevelCP);
			
			ChromatoseManager.manager.FirstLevelCPDone = true;
		}
	}
	
	public void FillBucket(Color color){
		
		_CurColor = color;
		_SpriteIndex = 1;
	//	_OnLoseColorFX = true;
		
		if(color == Color.red){
			switch(avatarType){
			case _AvatarTypeEnum.avatar:
				spriteInfo.Collection = coloredCollection;
				break;
			case _AvatarTypeEnum.shavatar:
				spriteInfo.Collection = shavaColoredCollection;
				break;
			}
			spriteInfo.SetSprite("Player1_rouge1");
			_ColorFadeString = "rouge";
			_Colored = true;
		}
		else if(color == Color.blue){
			switch(avatarType){
			case _AvatarTypeEnum.avatar:
				spriteInfo.Collection = coloredCollection;
				break;
			case _AvatarTypeEnum.shavatar:
				spriteInfo.Collection = shavaColoredCollection;
				break;
			}
			spriteInfo.SetSprite("Player1_bleu1");
			_ColorFadeString = "bleu";
			_Colored = true;
		}
		else{
			Debug.Log("Pas le bon Setting");
		}	
	}
	
	public void EmptyingBucket(){
		//spriteInfo.Collection = normalCollection;
		_Colored = false;
		_CurColor = Color.white;
	}
	
	
	public void Jolt(float amount){
		if (!myKnockTarget){
			Debug.LogWarning("There's no knockback targets in this level! NOOOOO!");
			return;
		}
		Vector3 direction = myKnockTarget.position - t.position;
		t.position += direction.normalized * amount;
	}
	
	public void Push(float amount){
		
		if (!myKnockTarget){
			Debug.LogWarning("There's no knockback targets in this level! NOOOOO!");
			return;
		}
		Vector2 diff = (Vector2)myKnockTarget.position - (Vector2)t.position;
		movement.SetVelocity(diff.normalized * amount);
		Debug.Log("Diff is " + diff);
	}
	/// <summary>
	/// Calls from far. Appel la creation d'un checkpoint depuis l'exterieur
	/// </summary>
	public void CallFromFar(){
		StartCoroutine(CPCreationForRoom(1.5f));
	}

	
	
	
	
	// COROUTINE DU CHU YEEEE
	public IEnumerator LateCPCreation(float _wait){
		yield return new WaitForSeconds(_wait);
		CreatFirstCP();
		//ChromatoseManager.manager.SaveRoom();
	}
	public IEnumerator CPCreationForRoom(float _wait){
		yield return new WaitForSeconds(_wait);
		CreateCP();
		//ChromatoseManager.manager.SaveRoom();
	}
	
	IEnumerator PlaySpeedFXBool(){
		yield return new WaitForSeconds(1.0f);
		_CantPlaySpeedFX = false;
	}
	IEnumerator BubbleGrowed(){
		yield return new WaitForSeconds(0.3f);
		bubbleGrowed = true;
	}
	IEnumerator BubbleShrinked(){
		yield return new WaitForSeconds(0.15f);
		bubbleShrinked = true;
	}
}

