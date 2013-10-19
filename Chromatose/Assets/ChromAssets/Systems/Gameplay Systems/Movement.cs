using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<-----------DEFINING MY VARIABLES!----------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	public bool humanControlled;
	public Transform target;
		
	public Thruster thruster = new Thruster();		//my class variables! :D
	public Rotator rotator = new Rotator();
	public Collider2d collider2d = new Collider2d();
	
	private bool _TooMuchCollide = false;
	
	private float collideTimer = 0f;
	private float collideTiming = 0.5f;
	private List<Collider> collidedWith = new List<Collider>();
	[System.NonSerializedAttribute]
	public Transform t;
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<------------DEFINING MY CLASSES!------------>
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	[System.Serializable]
	public class Thruster{			//This is where I do all of my Translation
		
		public float maxSpeed = 100;
		public float MaxSpeed{
			get { return maxSpeed; }
			}
		public float accel = 5;
		[System.NonSerialized]
		public float magnitude;
		[System.NonSerialized]
		public Vector2 velocity;

	}
	
	[System.Serializable]
	public class Rotator{			//ROTATOR CLASS OH YEAH
		
		public bool rotates;
		public float rotationRate;
		[System.NonSerialized]
		public float rotationIncrement = 22.5f;
		[System.NonSerialized]
		public float rotationTimer = 0;
		[System.NonSerialized]
		public bool prevClockwise;
				
	}
	
	void OnCollisionEnter(Collision collision){			//COLLISION CLASS! I'MA COLLIDE YOUR FACE!
		
		if (collision.gameObject.tag == "collision"){
			int rndNb = Random.Range(0,3);
			switch(rndNb){
			case 0:
				MusicManager.soundManager.PlaySFX(8);
				break;
			case 1:
				MusicManager.soundManager.PlaySFX(9);
				break;
			case 2:
				MusicManager.soundManager.PlaySFX(10);
				break;
			}
			ContactPoint point = collision.contacts[0];
			if (!collidedWith.Contains(collision.collider)){
				collideTimer = -0.1f;
				collidedWith.Add(collision.collider);
			}
			if (collideTimer <= 0){
				thruster.velocity = collider2d.Collide(point, thruster.velocity);
				collideTimer = collideTiming;
			}
		}
	}
	
	void OnCollisionStay(Collision collision){
		if (collision.gameObject.tag != "collision") return;
		ContactPoint point = collision.contacts[0];
		
		//Debug.Log(collision.contacts.Length);
		
		if(collision.contacts.Length > 10)_TooMuchCollide = true;
		else _TooMuchCollide = false;
		
		if (GetComponent<Avatar>() != null){
			t.position += new Vector3(point.normal.x, point.normal.y, 0);
			thruster.velocity += (Vector2)point.normal * 20;
			/*
			gameObject.SendMessage("Push", thruster.velocity.magnitude + thruster.accel);		//this tried to push the avatar away to a knockbackTrigger
			gameObject.SendMessage("Jolt", 1f);														//but it felt weird and was very easy to break
			*/
		}
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<-----------TRANSLATION FUNCTIONS!----------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	public Vector2 Displace(bool thrust){
		if(collideTimer > 10)return Vector2.zero;
		
		if(t != null){
			float zRotR = t.rotation.eulerAngles.z * Mathf.Deg2Rad;
			return Displace(thrust, zRotR);
		}
		else{
			t = GetComponent<Transform>();
			return Displace(thrust);
		}
	}
	
	public Vector2 Displace(bool thrust, float angle){								//Displacement : Thrust, accel, stuff
		if(collideTimer > 10)return Vector2.zero;
		if (thrust){
			
			
			Vector2 displacement = new Vector2(Mathf.Cos(angle) * thruster.accel, Mathf.Sin(angle) * thruster.accel);
			thruster.velocity += displacement;	
			//Debug.Log(thruster.velocity);
			if (thruster.velocity.magnitude > thruster.maxSpeed){
				thruster.velocity = thruster.velocity.normalized * thruster.maxSpeed;
			}
		}
		return thruster.velocity * Time.deltaTime;
	}
	
	public void SlowToStop(){
		if (thruster.velocity != Vector2.zero){
			thruster.velocity = Vector2.Lerp(thruster.velocity, Vector3.zero, 0.02f);
			t.position += (Vector3)Displace(false);
		}
	}
	
	public void SetVelocity(Vector2 newVel){
		thruster.velocity = newVel;
	}
	
	public Vector2 GetVelocity(){
		return thruster.velocity;
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<------------ROTATION FUNCTIONS!!------------>
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	
	public Vector3 Rotate (bool clockwise){								//Rotate me; rotate me, my friend.
		
		if (!rotator.rotates){
			return Vector3.zero;
		}
		
		
		if (clockwise == rotator.prevClockwise){
			rotator.rotationTimer += Time.deltaTime;
		}
		else{
			rotator.rotationTimer = 1;
		}
		
		rotator.prevClockwise = clockwise;
		
		if (rotator.rotationTimer >= rotator.rotationRate){
			rotator.rotationTimer = 0;
			Vector3 rotAmount;
			if (clockwise){
				rotAmount = new Vector3(0, 0, -rotator.rotationIncrement);
			}
			else{
				
				rotAmount = new Vector3(0, 0, rotator.rotationIncrement);
			}
			
			return rotAmount;
			
		}
		else{
			return Vector3.zero;
		}
		
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<-------------COLLIDING ALL DAY!------------->
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
	
	
	public class Collider2d{
		
		private float frictionFactor = 4;
		
		
		public Vector2 Collide(ContactPoint point, Vector2 velocity){
			
			Vector2 normal = point.normal;
			//Debug.Log(point.normal);
			normal = new Vector2(-normal.y, normal.x);
			Vector2 newVel = 2 * normal * (normal.x * velocity.x + normal.y * velocity.y) - velocity;
			
			
			newVel /= frictionFactor;
			/*
			 *     xN = sin(degtorad(157.5));
				    yN = cos(degtorad(157.5));
				    if (canCol3){
				        tempx = x1 / magnitude;
				        tempy = y1 / magnitude;
				        
				        x1 = 2 * xN * (xN * tempx + yN * tempy) - tempx;
				        x1 *= magnitude / 2;
				        y1 = 2 * yN * (xN * tempx + yN * tempy) - tempy;
				        y1 *= magnitude / 2;
				        
				        canCol3 = false;
				    }  
				    else{
				        x += xN * timer3 / eighthPushModifier;
				        y -= yN * timer3 / eighthPushModifier;
				    }
			*/
			return newVel;
		
		}
	}
	
	//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
	//<------BORING UNITY'S BORING FUNCTIONS!------>
	//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>				
	
	
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
	}
	
	// Update is called once per frame, but doesn't want to call my other ones. I'm gonna make it!
	void Update () {
		if (collideTimer > 0){
			collideTimer -= Time.deltaTime;
			if (collideTimer <= 0){
				collidedWith.Clear();
			}
		}
	}
	
	
	public void SetNewMoveStats(float newMax, float newAccel, float newRotRate){
		thruster.maxSpeed = newMax;
		thruster.accel = newAccel;
		rotator.rotationRate = newRotRate;
		return;
	}
	
}
