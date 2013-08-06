using UnityEngine;
using System.Collections;

public class TransitionTrigger : MonoBehaviour {
	
	public enum transitionVers{
		 NextLevel, LocalTarget, StaticComic, DynamicComic, TimeTrial
	}
	public transitionVers _TransitEnum;
	
	private Avatar _AvatarScript;
	private Transform avatarT;
	private ChromatoseManager _Manager;
	
	private bool _Popped = false;
	private bool _Lightning = false;
	private bool _OnStaticComic = false;
	private bool _FadeIn = false;
	private bool _FadeOut = false;
	
	private float _LightCounter = 0f;
	
	private delegate void TriggerMethod();
	private TriggerMethod myTrigger;
	
	public Texture blackBox;
	public Transform localTarget;
	public Texture staticComic;			//<--Devra peut-etre etre changer dependant comment il faut transitionner vers sa
	public Transform dynamicComicTarget;
	
	void Start () {
		
		_Manager = ChromatoseManager.manager;
		_AvatarScript = GameObject.FindGameObjectWithTag("avatar").GetComponent<Avatar>();
		avatarT = GameObject.FindWithTag("avatar").transform;
		
		
		
		if(_Manager.TimeTrialMode) {
			_TransitEnum = transitionVers.TimeTrial;
		}
		
		switch (_TransitEnum){
		case transitionVers.NextLevel:
			
			myTrigger = NextLevel;
			
			break;
		case transitionVers.LocalTarget:
			
			if(localTarget != null){
					myTrigger = ToStaticComic;
				}
				else{
					myTrigger = NextLevel;
				}
			
			break;
		case transitionVers.TimeTrial:
			
			myTrigger = FinishTimeTrialChallenge;
			
			break;
		case transitionVers.StaticComic:
			
			Debug.Log("Transition vers StaticComic non-prete");
				myTrigger = NextLevel;
			
				/*if(staticComic != null){
					myTrigger = ToStaticComic;
				}
				else{
					myTrigger = NextLevel;
				}*/
			
			break;
		case transitionVers.DynamicComic:
			
				if(dynamicComicTarget != null){
					myTrigger = ToDynamicComic;
				}
				else{
					myTrigger = NextLevel;
				}
			
			break;
		}
	}	

	void Update () {
		
		
		switch (_TransitEnum){
		case transitionVers.NextLevel:
			
			if (!_Popped) return;
		
			if(_LightCounter < 1){_LightCounter += 0.02f;}
			else{
				myTrigger();
			}
			
			break;
		case transitionVers.LocalTarget:
			
			if (!_Popped) return;
				
			if (_LightCounter < 1 && _FadeIn){_LightCounter += 0.02f;}
			if (_LightCounter > 0 && _FadeOut){_LightCounter -= 0.02f;}
			if (_LightCounter > 1){_FadeIn = false; _FadeOut = true; myTrigger();}
				
			break;
		case transitionVers.TimeTrial:
			
			if (!_Popped) return;
			myTrigger();
			
			break;
		case transitionVers.StaticComic:
			
			if (!_Popped) return;
			_OnStaticComic = true;
			
			break;
		case transitionVers.DynamicComic:
			
			if (!_Popped) return;
			
			if (_LightCounter < 1 && _FadeIn){_LightCounter += 0.02f;}
			if (_LightCounter > 0 && _FadeOut){_LightCounter -= 0.02f;}
			if (_LightCounter > 1){_FadeIn = false; _FadeOut = true; myTrigger();}
			
			break;
		}
		
		
		
		

	}
	
	void OnTriggerEnter(Collider other){
		if (other.name == "Avatar"){
			_FadeIn = true;
			_Popped = true;
		}
	}
	
	void OnGUI(){
		
		if(!_OnStaticComic){
		
			if (!_Popped) return;
			
			GUI.color = new Color(0, 0, 0, _LightCounter);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
		
		}
		else{
			//Affiche le Comic dans le GUI Directment, pensez a faire le lien avec les bouton et le reste du HUD
			Rect comicRect = new Rect(0, 0, staticComic.width, staticComic.height);
			GUI.DrawTexture(comicRect, staticComic);
		}
	}
	
	/// <summary>
	/// Appelle la Transition de l'exterieur
	/// </summary>
	public void ExternalCall(){
		myTrigger();
	}
	public void ResetBool(){
		_Popped = false;
		_FadeIn = true;
		_FadeOut = false;
	}
	
	
#region Methodes de Transition
	
	void NextLevel(){
		_AvatarScript.LoseAllColour();
		if(_AvatarScript.HasOutline){
			_AvatarScript.CancelOutline();
		}
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	void ToTarget(){
		avatarT.position = localTarget.position;
		avatarT.rotation = localTarget.rotation;
		avatarT.SendMessage ("SetVelocity", Vector2.zero);
		
		_AvatarScript.CallFromFar();
		_AvatarScript.LoseAllColour();		
		if(_AvatarScript.HasOutline){
			_AvatarScript.CancelOutline();
		}
		
	}
	
	void FinishTimeTrialChallenge(){
		if(_Manager.StopChallenge()){
			//Faire afficher la fenetre de reussite du TimeTrial
			_Manager.DisplayWinWindows = true;
		}
		StartCoroutine(DelaiToDisplay());
	}
	
	void ToStaticComic(){
		
	}
	
	void ToDynamicComic(){
		avatarT.position = dynamicComicTarget.position;
		avatarT.rotation = dynamicComicTarget.rotation;
		avatarT.SendMessage ("SetVelocity", Vector2.zero);
		
		_AvatarScript.LoseAllColour();		
		if(_AvatarScript.HasOutline){
			_AvatarScript.CancelOutline();
		}
	}
#endregion	
	
#region CoRoutine	
	IEnumerator DelaiToDisplay(){
		yield return new WaitForSeconds(0.5f);
		_Manager.DisplayScore = true;
	}
}
#endregion