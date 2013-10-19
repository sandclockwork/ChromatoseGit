using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ChromaStats;

public class MainMenu : MonoBehaviour {
	/*
	public enum _MenuWindowsEnum{
		MainMenu, LevelSelectionWindows, CreditWindows, OptionWindows, LoadingScreen
	}
	public enum _OptionWindowsEnum{
		MainOption, Sound, GameMode, Stats, DeleteSavegame,
	}
	
	
	
	public MainMenuButton mainMenuButtton = new MainMenuButton();
	private StatsDisplay _StatsDisplay = new StatsDisplay();
	
	public _MenuWindowsEnum _MenuWindows;
	private _OptionWindowsEnum _OptionWindows;
	
	public enum _GameTypeEnum{
		FreeVersion, FullRelease
	}
	
	private _GameTypeEnum _GameType;
	
	private UnitySingleton _Singleton;
	
	public GUISkin _SkinMenuAvecBox;
	public GUISkin _SkinMenuSansBox;
	public GUISkin _SkinMenuAvecPetitBox;
	
	public GUISkin _StartButtonSkin, _CreditButtonSkin, _FbookButtonSkin, _TwitterButtonSkin, _BackButtonSkin, _GreenlightButton;
	public Texture _LoadingText, _AvatarLoadingLoop1, _AvatarLoadingLoop2, _BulleLoadLoop1, _BulleLoadLoop2;
	
	private Color _FontColor;
	private float _FontMultiplier = 1;

	private AsyncOperation _lvlLoad;
	
	private bool[] _SlotSelected = {false, false, false};
	private bool[] _DeleteSelected = {false, false, false};
	
	private float _Alphatiser = 0;
	
	private MainManager _MainManager;
	private bool _FirstStart;
		public bool firstStart{
			get{return _FirstStart;}
			set{_FirstStart = value;}
		}
	
	//VARIABLE SOUND
	private bool _MusicMute = false;
	private bool _SFXMute = false;
	private float _MusicVolume = Mathf.Clamp(80, 0, 100);
	private float _SFXVolume = Mathf.Clamp(80, 0, 100);
	
	//VARIABLE D'INTERFACE DYNAMIC
	private float _RotStartButton = 0;
	private bool _RotUp = false;
	private int _LoadCounter = 0;
	
	//VARIABLE TRIAL (VERSION DEMO)
	private bool _ADADAD = true;
	
	//VARIABLE GAMEMODE
	private bool _ExtraModeUnlocked = false;
	private bool _TimeTrialActive = false;
	private bool _NoDeathModeActive = false;
	/*
	//VARIABLE LOADING SCREEN
	private float _LoadProgress = 0;
	private AsyncOperation async = null;
	private bool _loop2 = false;
	private float loopingCounter = 0;*/
	/*
	[System.Serializable]
	public class MainMenuButton{
		
		public Texture mainMenuBG;
		public Texture optionBG;
		public Texture blackBG;
		
		public Texture fbookIcon;
		public Texture twitterIcon;
		
		public Texture emptyProgressBar;
		public Texture progressLine;
		
		public static Rect _MainMenuBGRect;
		
		public Texture credits;
		
		void Start(){
			_MainMenuBGRect = new Rect(0, 0, Screen.width, Screen.height);
			
		}
	}
	
	[System.Serializable]
	public class StatsDisplay{
		//VARIABLE STATS
		private int _DeadNPCCount = 0;
		private int _whiteCollCollected = 0;
		private int _blueCollCollected = 0;
		private int _redCollCollected = 0;
		private int _comicThumbsCollected = 0;
		
		private int _DeathCounter = 0;
		private float _TotalPlayTime = 0;
		private int _AchievementSucceed = 0;
		
		private int _TotalWhiteCollInLevel = 100;
		private int _TotalBlueCollInLevel = 20;
		private int _TotalRedCollInLevel = 35;
		private int _TotalComicThumbsInLevel = 40;
		private int _TotalAchievementCount = 10;
		
		private string _DeadNPCString;
			public string DeadNPCString{
				get{return _DeadNPCString;}
			}
		private string _whiteCollString;
			public string whiteCollString{
				get{return _whiteCollString;}
			}
		private string _blueCollString;
			public string blueCollString{
				get{return _blueCollString;}
			}
		private string _redCollString;
			public string redCollString{
				get{return _redCollString;}
			}
		private string _AchievementString;
			public string achievementString{
				get{return _AchievementString;}
			}
		private string _DeathCountString;
			public string DeathCountString{
				get{return _DeathCountString;}
			}
		private string _TotalPlaytimeString;
			public string totalPlaytimeString{
				get{return _TotalPlaytimeString;}
			}
		private string _ComicThumbsString;
			public string comicThumbsString{
				get{return _ComicThumbsString;}
			}
		
		public void SetUpStats(){
	
			_DeadNPCString = _DeadNPCCount.ToString();
			
			_whiteCollString = _whiteCollCollected + "/" + _TotalWhiteCollInLevel;
			_blueCollString = _blueCollCollected + "/" + _TotalBlueCollInLevel;
			_redCollString = _redCollCollected + "/" + _TotalRedCollInLevel;
			_AchievementString = _AchievementSucceed + "/" + _TotalAchievementCount;
			_ComicThumbsString = _comicThumbsCollected + "/" + _TotalComicThumbsInLevel;
			
			_DeathCountString = _DeathCounter.ToString();
			
			float min;
			float sec;
			min = Mathf.Floor(_TotalPlayTime/60f);		
			sec = Mathf.RoundToInt(_TotalPlayTime % 60f);
			
			_TotalPlaytimeString = min + "min" + sec + "sec";
			_TotalPlaytimeString = string.Format("{00:00}:{1:00}", min, sec);
		}
	}
	
	
	void Start () {
		
		_StatsDisplay.SetUpStats();
		
		_FontColor = new Color(173, 173, 173);
		
		_Singleton = UnitySingleton.Instance;
		
		//DontDestroyOnLoad(this.gameObject);
		
		if(!_Singleton.FULLRELEASE){
			_GameType = _GameTypeEnum.FreeVersion;
		}
		else{
			_GameType = _GameTypeEnum.FullRelease;
		}
		
	}
	
	void Update () {
	//Debug.Log(LevelSerializer.SavedGames);
		
		RotateStartButton();
		
		
		if(_MainManager == null){
			_MainManager = GameObject.FindObjectOfType(typeof(MainManager))as MainManager;
		}
	
		_FontMultiplier = (Screen.width/12.8f)/100;
		//_LoadProgress = _lvlLoad.progress;
		//Debug.Log(_LoadProgress);
		
		#region Switch Update (in Case)
		switch(_MenuWindows){
		case _MenuWindowsEnum.MainMenu:
			
			break;
		case _MenuWindowsEnum.LevelSelectionWindows:
			
			break;
		case _MenuWindowsEnum.OptionWindows:
			
			break;
		case _MenuWindowsEnum.CreditWindows:
			
			break;
		case _MenuWindowsEnum.LoadingScreen:
		
			if(_LoadCounter < 200){
				_LoadCounter++;
			}
			else{
				_LoadCounter = 0;
			}
			
			break;
		}	
		#endregion
	}
	
	

	
	
	void RotateStartButton(){
		if(_RotStartButton >= 4f){
			_RotUp = false;
		}
		else if(_RotStartButton < -8f){
			_RotUp = true;
		}
			
		if(!_RotUp){
			_RotStartButton -= 0.075f;
		}
		else{
			_RotStartButton += 0.075f;
		}
	}
	
	void OnGUI(){
		GUI.skin = _SkinMenuSansBox;
		GUI.skin.button.fontSize = 60;
		GUI.skin.textArea.fontSize = 60;
		GUI.skin.toggle.fontSize = 44;
		
		float horizRatio = Screen.width / 1280.0f;
		float vertiRatio = Screen.height / 720.0f;
		
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(horizRatio, vertiRatio, 1f));

		Matrix4x4 matrixBackup = GUI.matrix;
		
		
		
		switch (_GameType){

#region FullRelease Menu
		case _GameTypeEnum.FullRelease:
				
			switch (_MenuWindows){
				
			#region Main Windows
			case _MenuWindowsEnum.MainMenu:
				
				//BACKGROUND
				GUI.DrawTexture(new Rect(0, 0, 1280, 720), mainMenuButtton.mainMenuBG);
				
				
				if(!_FirstStart){
					//RESUME BUTTON
					GUIUtility.RotateAroundPivot(17.5f + _RotStartButton, new Vector2(435*horizRatio, 360*vertiRatio));
					GUI.skin.button.fontSize = 76;
					if(GUI.Button(new Rect(335, 510, 280, 85), "RESUME")){
						_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
					}
					GUI.matrix = matrixBackup; 
				}
				else{
					//START BUTTON
					GUIUtility.RotateAroundPivot(17.5f + _RotStartButton, new Vector2(435*horizRatio, 360*vertiRatio));
					GUI.skin.button.fontSize = 76;
					if(GUI.Button(new Rect(335, 510, 280, 85), "START")){
						_MenuWindows = _MenuWindowsEnum.LevelSelectionWindows;
						//_MenuWindows = _MenuWindowsEnum.LoadingScreen;
						//_lvlLoad = Application.LoadLevelAsync(Application.loadedLevel + 2);
					}
					GUI.matrix = matrixBackup; 
				}	
				
				GUI.skin.button.fontSize = 48;
				GUIUtility.RotateAroundPivot(8f, new Vector2(920*horizRatio, 310*vertiRatio));
				if(GUI.Button(new Rect(880, 490, 190, 60), "OPTIONS")){
					_MenuWindows = _MenuWindowsEnum.OptionWindows;
				}
				GUI.matrix = matrixBackup; 
				
				GUI.skin.button.fontSize = 32;
				GUIUtility.RotateAroundPivot(-8f, new Vector2(50*horizRatio, 380*vertiRatio));
				if(GUI.Button(new Rect(25, 560, 220, 50), "> CREDITS <")){
					
				}
				GUI.matrix = matrixBackup; 
				
				//FBOOK & TWITTER
				GUI.skin = _SkinMenuAvecPetitBox;
			
				if(GUI.Button (new Rect(25, 25, 80, 80), mainMenuButtton.fbookIcon)){
					Application.OpenURL("https://www.facebook.com/FabulamGames?fref=ts");
				}
				if(GUI.Button (new Rect(125, 25, 80, 80), mainMenuButtton.twitterIcon)){
					Application.OpenURL("https://twitter.com/Chromatosegame");
				}
				
				
				//BUY IT ON STEAM
				if(_ADADAD){
					GUI.skin = _SkinMenuAvecBox;
					if(GUI.Button(new Rect(850, 20, 400, 60), "Visit Our Site !")){
						Application.OpenURL("http://store.steampowered.com/");
					}
				}
				
				//QUITBUTTON
				if(GUI.Button(new Rect(850, 90, 400, 60), "EXIT GAME")){
					Application.Quit();
				}
				
				break;
				#endregion
				
			#region LevelSelection Windows
			case _MenuWindowsEnum.LevelSelectionWindows:
				
				GUI.skin = null;
				GUI.skin.button.fontSize = 22;
				if(GUI.Button(new Rect(250, 50, 300, 50), "TUTO - BLANC 1")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(1));
				}
				if(GUI.Button(new Rect(250, 125, 300, 50), "NIV 1 - ROUGE 1")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(2));
				}
				if(GUI.Button(new Rect(250, 200, 300, 50), "NIV 2 - BLANC 2")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(3));
				}
				if(GUI.Button(new Rect(250, 275, 300, 50), "NIV 3 - ROUGE 2")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(4));
				}
				if(GUI.Button(new Rect(250, 350, 300, 50), "NIV 4 - BLANC 3")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(5));
				}
				
				
				
				
				if(GUI.Button(new Rect(600, 50, 300, 50), "NIV 5 - ROUGE/BLEU 3")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(6));
				}
				if(GUI.Button(new Rect(600, 125, 300, 50), "NIV 6 - BLANC 4")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(7));
				}
				if(GUI.Button(new Rect(600, 200, 300, 50), "NIV 7 - BLEU 4")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(8));
				}
				if(GUI.Button(new Rect(600, 275, 300, 50), "NIV 8 - ROUGE/BLEU 5")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(9));
				}
				if(GUI.Button(new Rect(600, 350, 300, 50), "NIV 9 - ROUGE 6")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(10));
				}
				
				
				
				if(GUI.Button(new Rect(250, 425, 650, 50), "BOSS FINAL")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(11));
				}
				
				if(GUI.Button(new Rect(800, 600, 650, 50), "GYM DU CHU")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					StartCoroutine(LoadALevel(11));
				}
				
				break;
				#endregion
				
			#region Option Windows
			case _MenuWindowsEnum.OptionWindows:
				
				Rect inMenuRect = new Rect(190, 165, 740, 395);
				
				//BACKGROUND
				GUI.DrawTexture(new Rect(0, 0, 1280, 720), mainMenuButtton.optionBG);
				
				switch(_OptionWindows){
					#region MainOption
				case _OptionWindowsEnum.MainOption:
					
					Rect optionRect = new Rect(190, 165, 740, 395);
					
					GUI.skin = _SkinMenuSansBox;
					GUI.BeginGroup(optionRect);
						if(GUI.Button(new Rect(optionRect.width*0.26f, optionRect.height*0.1f, optionRect.width*0.66f, optionRect.height*0.20f), "- SOUND -")){
							_OptionWindows = _OptionWindowsEnum.Sound;
						}
						if(GUI.Button(new Rect(optionRect.width*-0.03f, optionRect.height*0.34f, optionRect.width*0.66f, optionRect.height*0.20f), "- GAME MODE -")){
							_OptionWindows = _OptionWindowsEnum.GameMode;
						}
						if(GUI.Button(new Rect(125, optionRect.height*0.58f, optionRect.width*0.66f, optionRect.height*0.20f), "- STATS -")){
							_OptionWindows = _OptionWindowsEnum.Stats;
						}
						if(GUI.Button(new Rect(optionRect.width*0.05f, optionRect.height*0.80f, optionRect.width*0.92f, optionRect.height*0.20f), "- DELETE SAVEGAME -")){
							_OptionWindows = _OptionWindowsEnum.DeleteSavegame;
						}
						
					GUI.EndGroup();
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_MenuWindows = _MenuWindowsEnum.MainMenu;
					}
					
					break;
					#endregion
					
					#region SoundOption
				case _OptionWindowsEnum.Sound:
					Rect soundRect = new Rect(190, 110, 740, 400);
					
					GUI.BeginGroup(soundRect);
						GUI.skin.button.fontSize = 32;
						GUI.TextArea(new Rect(soundRect.width*0.25f, soundRect.height*0.21f, soundRect.width*0.5f, soundRect.height*0.2f), "- SOUND -");
						
						//Text & Slider
						GUI.skin.textArea.fontSize = 42;
						GUI.TextArea(new Rect(soundRect.width*0.025f, soundRect.height*0.51f, soundRect.width*0.15f, soundRect.height*0.15f), "MUSIC");
						_MusicVolume = GUI.HorizontalSlider(new Rect(soundRect.width*0.23f, soundRect.height*0.51f, soundRect.width*0.55f, soundRect.height*0.18f), _MusicVolume, 100, 0);
					
						GUI.TextArea(new Rect(soundRect.width*0.05f, soundRect.height*0.76f, soundRect.width*0.15f, soundRect.height*0.15f), "SFX");
						_SFXVolume = GUI.HorizontalSlider(new Rect(soundRect.width*0.23f, soundRect.height*0.76f, soundRect.width*0.55f, soundRect.height*0.18f), _SFXVolume, 100, 0);
					
						//Mute Button
						_MusicMute = GUI.Toggle(new Rect(soundRect.width*0.8f, soundRect.height*0.51f, soundRect.width*0.2f, soundRect.height*0.15f), _MusicMute, "MUTE");
						_SFXMute = GUI.Toggle(new Rect(soundRect.width*0.8f, soundRect.height*0.76f, soundRect.width*0.2f, soundRect.height*0.15f), _SFXMute, "MUTE");
					GUI.EndGroup();
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
					
					#region GameModeOption
				case _OptionWindowsEnum.GameMode:
					
					GUI.BeginGroup(inMenuRect);
						GUI.skin.textArea.fontSize = 60;
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.15f, inMenuRect.width*0.6f, inMenuRect.height*0.21f), "- GAMEMODE -");
					
						//TIME TRIAL MODE & NO DEATH ACTIVATION
						if(!_ExtraModeUnlocked){
							GUI.TextArea(new Rect(inMenuRect.width*0.02f, inMenuRect.height*0.52f, inMenuRect.width*0.9f, inMenuRect.height*0.4f), " Finish the Game to Unlock More Chromatose Extra Mode");
						
						}
						else{
							GUI.skin.textArea.fontSize = 40;
						
							GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.52f, inMenuRect.width*0.6f, inMenuRect.height*0.25f), "TIME TRIAL CHALLENGE");
							GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.74f, inMenuRect.width*0.6f, inMenuRect.height*0.25f), "NO DEATH CHALLENGE");
							_TimeTrialActive = GUI.Toggle(new Rect(inMenuRect.width*0.7f, inMenuRect.height*0.52f, inMenuRect.width*0.15f, inMenuRect.height*0.2f),_TimeTrialActive ,"Active");
							_NoDeathModeActive = GUI.Toggle(new Rect(inMenuRect.width*0.7f, inMenuRect.height*0.74f, inMenuRect.width*0.15f, inMenuRect.height*0.2f), _NoDeathModeActive, "Active");
							
						}
					GUI.EndGroup();
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
					
					#region StatsOption
				case _OptionWindowsEnum.Stats:
					
					
					GUI.BeginGroup(inMenuRect);
						GUI.skin.textArea.fontSize = 60;
						
						//STATIC STRING
						GUI.TextArea(new Rect(inMenuRect.width*0.24f, inMenuRect.height*0.01f, inMenuRect.width*0.6f, inMenuRect.height*0.21f), "- STATS -");
					
						GUI.skin.textArea.fontSize = 30;
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.2f, inMenuRect.width*0.3f, inMenuRect.height*0.21f), "DEAD NPC");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "WHITE COLLECTIBLES");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.4f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "RED COLLECTIBLES");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.5f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "BLUE COLLECTIBLES");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "COMIC THUMBS");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "ACHIEVEMENT");
					
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL DEATH");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL PLAYTIME");
					
						//STATISTIC STRING
						GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.2f, inMenuRect.width*0.3f, inMenuRect.height*0.21f), _StatsDisplay.DeadNPCString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.whiteCollString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.4f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.redCollString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.5f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.blueCollString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.comicThumbsString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.achievementString);
						GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.DeathCountString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.totalPlaytimeString);
					GUI.EndGroup();
					
					//HIGHSCORE BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(470, 605, 385, 80), "- HIGHSCORES -")){
						
					}
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
					
					#region DeleteOption
				case _OptionWindowsEnum.DeleteSavegame:
					
					GUI.BeginGroup(inMenuRect);
						GUI.skin.textArea.fontSize = 52;
						GUI.TextArea(new Rect(inMenuRect.width*0.05f, inMenuRect.height*0.14f, inMenuRect.width*0.9f, inMenuRect.height*0.2f), "- DELETE THE ACTUAL -");
						GUI.skin.textArea.fontSize = 46;
						GUI.TextArea(new Rect(inMenuRect.width*0.3f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.2f), "- SAVED GAME -");
						GUI.skin.textArea.fontSize = 55;
						GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.51f, inMenuRect.width*0.7f, inMenuRect.height*0.28f), "? ARE YOU SURE ?");
						
						GUI.skin.button.fontSize = 60;
						if(GUI.Button(new Rect(inMenuRect.width*0.12f, inMenuRect.height*0.73f, 250, 80), "YES")){
							File.Delete(Application.persistentDataPath + "/" + "Chromasave");
							_FirstStart = true;
							_MenuWindows = _MenuWindowsEnum.MainMenu;
							_OptionWindows = _OptionWindowsEnum.MainOption;
							Debug.Log("GAME DELETER");
						}
						if(GUI.Button(new Rect(inMenuRect.width*0.4f, inMenuRect.height*0.73f, 250, 80), "NO")){
							_OptionWindows = _OptionWindowsEnum.MainOption;
						}
					
					GUI.EndGroup();
					
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
				}
				
				break;
				#endregion
	
			#region Loading Screen
			case _MenuWindowsEnum.LoadingScreen:
				
				Rect inLoadRect = new Rect(217.5f, 165, 740, 380);
				
				//BACKGROUND
				GUI.DrawTexture(new Rect(0, 0, 1280, 720), mainMenuButtton.blackBG);
				
				//_LoadProgress = Application.GetStreamProgressForLevel(Application.loadedLevel + 2);
				
				GUI.BeginGroup(inLoadRect);
					GUI.skin.textArea.fontSize = 62;
					GUI.TextArea(new Rect(inLoadRect.width*0.41f, inLoadRect.height*0.25f,inLoadRect.width, inLoadRect.height*0.4f), "Loading");
					
				//TODO Gerer la Progress Bar differement selon WebPlayer/StandAlone
					//PROGRESS BAR
					//GUI.DrawTexture(new Rect(inLoadRect.width*0.25f, inLoadRect.height*0.5f,inLoadRect.width*0.5f, inLoadRect.height*0.2f), mainMenuButtton.emptyProgressBar);
					//GUI.DrawTexture(new Rect(inLoadRect.width*0.25f, inLoadRect.height*0.5f,inLoadRect.width*0.48f * _LoadProgress, inLoadRect.height*0.18f), mainMenuButtton.progressLine);
					
					if(_LoadCounter > 50){GUI.TextArea(new Rect(inLoadRect.width*0.605f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
					else if(_LoadCounter > 100){GUI.TextArea(new Rect(inLoadRect.width*0.625f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
					else if(_LoadCounter > 150){GUI.TextArea(new Rect(inLoadRect.width*0.645f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
				GUI.EndGroup();
				
				break;
				#endregion
				
			#region Credits Windows
			case _MenuWindowsEnum.CreditWindows:
				
				break;
				#endregion
			
			}
				
			break;
#endregion			
			
#region FreeVersion Menu
		case _GameTypeEnum.FreeVersion:
			
			
			switch (_MenuWindows){
				
			#region Main Windows
			case _MenuWindowsEnum.MainMenu:
				
				//BACKGROUND
				GUI.DrawTexture(new Rect(0, 0, 1280, 960), mainMenuButtton.mainMenuBG);
				
				
				//START BUTTON
				GUI.skin = _StartButtonSkin;
				GUI.skin.button.fontSize = 76;
				if(GUI.Button(new Rect(195, 408, 400, 400), "")){
					_MenuWindows = _MenuWindowsEnum.LoadingScreen;
					//Application.LoadLevelAsync(1);
					StartCoroutine(LoadALevel(1));
				}
				GUI.matrix = matrixBackup; 

				GUI.skin = _CreditButtonSkin;
				GUI.skin.button.fontSize = 76;
				if(GUI.Button(new Rect(664, 408, 400, 400), "")){
					_MenuWindows = _MenuWindowsEnum.CreditWindows;
				}
				GUI.matrix = matrixBackup; 
				
				//FBOOK & TWITTER
				GUI.skin = _TwitterButtonSkin;			
				if(GUI.Button (new Rect(10, 20, 97, 97), "")){
					Application.OpenURL("https://twitter.com/Chromatosegame");
				}
				
				GUI.skin = _FbookButtonSkin;
				if(GUI.Button (new Rect(88, 20, 97, 97), "")){
					Application.OpenURL("https://www.facebook.com/FabulamGames?fref=ts");
				}
				
				
				//BUY IT ON STEAM
				if(_ADADAD){
					GUI.skin = _GreenlightButton;
					if(GUI.Button(new Rect(920, 20f, 335, 137), "")){
						//Application.OpenURL("http://steamcommunity.com/sharedfiles/filedetails/?id=174349688");
						Application.ExternalEval("window.open('http://steamcommunity.com/sharedfiles/filedetails/?id=174349688','Chromatose Greenlight Page')");
					}
				}
								
				break;
				#endregion
				
			#region LevelSelection Windows
			case _MenuWindowsEnum.LevelSelectionWindows:
			
				
				
				break;
				#endregion
				
			#region Option Windows
			case _MenuWindowsEnum.OptionWindows:
				
				Rect inMenuRect = new Rect(190, 165, 740, 395);
				
				//BACKGROUND
				GUI.DrawTexture(new Rect(0, 0, 1280, 720), mainMenuButtton.optionBG);
				
				switch(_OptionWindows){
					#region MainOption
				case _OptionWindowsEnum.MainOption:
					
					Rect optionRect = new Rect(190, 165, 740, 395);
					
					GUI.skin = _SkinMenuSansBox;
					GUI.BeginGroup(optionRect);
						if(GUI.Button(new Rect(optionRect.width*0.26f, optionRect.height*0.1f, optionRect.width*0.66f, optionRect.height*0.20f), "- SOUND -")){
							_OptionWindows = _OptionWindowsEnum.Sound;
						}
						if(GUI.Button(new Rect(optionRect.width*-0.03f, optionRect.height*0.34f, optionRect.width*0.66f, optionRect.height*0.20f), "- GAME MODE -")){
							_OptionWindows = _OptionWindowsEnum.GameMode;
						}
						if(GUI.Button(new Rect(125, optionRect.height*0.58f, optionRect.width*0.66f, optionRect.height*0.20f), "- STATS -")){
							_OptionWindows = _OptionWindowsEnum.Stats;
						}
						if(GUI.Button(new Rect(optionRect.width*0.05f, optionRect.height*0.80f, optionRect.width*0.92f, optionRect.height*0.20f), "- DELETE SAVEGAME -")){
							_OptionWindows = _OptionWindowsEnum.DeleteSavegame;
						}
						
					GUI.EndGroup();
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_MenuWindows = _MenuWindowsEnum.MainMenu;
					}
					
					break;
					#endregion
					
					#region SoundOption
				case _OptionWindowsEnum.Sound:
					Rect soundRect = new Rect(190, 110, 740, 400);
					
					GUI.BeginGroup(soundRect);
						GUI.skin.button.fontSize = 32;
						GUI.TextArea(new Rect(soundRect.width*0.25f, soundRect.height*0.21f, soundRect.width*0.5f, soundRect.height*0.2f), "- SOUND -");
						
						//Text & Slider
						GUI.skin.textArea.fontSize = 42;
						GUI.TextArea(new Rect(soundRect.width*0.025f, soundRect.height*0.51f, soundRect.width*0.15f, soundRect.height*0.15f), "MUSIC");
						_MusicVolume = GUI.HorizontalSlider(new Rect(soundRect.width*0.23f, soundRect.height*0.51f, soundRect.width*0.55f, soundRect.height*0.18f), _MusicVolume, 100, 0);
					
						GUI.TextArea(new Rect(soundRect.width*0.05f, soundRect.height*0.76f, soundRect.width*0.15f, soundRect.height*0.15f), "SFX");
						_SFXVolume = GUI.HorizontalSlider(new Rect(soundRect.width*0.23f, soundRect.height*0.76f, soundRect.width*0.55f, soundRect.height*0.18f), _SFXVolume, 100, 0);
					
						//Mute Button
						_MusicMute = GUI.Toggle(new Rect(soundRect.width*0.8f, soundRect.height*0.51f, soundRect.width*0.2f, soundRect.height*0.15f), _MusicMute, "MUTE");
						_SFXMute = GUI.Toggle(new Rect(soundRect.width*0.8f, soundRect.height*0.76f, soundRect.width*0.2f, soundRect.height*0.15f), _SFXMute, "MUTE");
					GUI.EndGroup();
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
					
					#region GameModeOption
				case _OptionWindowsEnum.GameMode:
					
					GUI.BeginGroup(inMenuRect);
						GUI.skin.textArea.fontSize = 60;
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.15f, inMenuRect.width*0.6f, inMenuRect.height*0.21f), "- GAMEMODE -");
					
						//TIME TRIAL MODE & NO DEATH ACTIVATION
						if(!_ExtraModeUnlocked){
							GUI.TextArea(new Rect(inMenuRect.width*0.02f, inMenuRect.height*0.52f, inMenuRect.width*0.9f, inMenuRect.height*0.4f), " Finish the Game to Unlock More Chromatose Extra Mode");
						
						}
						else{
							GUI.skin.textArea.fontSize = 40;
						
							GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.52f, inMenuRect.width*0.6f, inMenuRect.height*0.25f), "TIME TRIAL CHALLENGE");
							GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.74f, inMenuRect.width*0.6f, inMenuRect.height*0.25f), "NO DEATH CHALLENGE");
							_TimeTrialActive = GUI.Toggle(new Rect(inMenuRect.width*0.7f, inMenuRect.height*0.52f, inMenuRect.width*0.15f, inMenuRect.height*0.2f),_TimeTrialActive ,"Active");
							_NoDeathModeActive = GUI.Toggle(new Rect(inMenuRect.width*0.7f, inMenuRect.height*0.74f, inMenuRect.width*0.15f, inMenuRect.height*0.2f), _NoDeathModeActive, "Active");
							
						}
					GUI.EndGroup();
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
					
					#region StatsOption
				case _OptionWindowsEnum.Stats:
					
					
					GUI.BeginGroup(inMenuRect);
						GUI.skin.textArea.fontSize = 60;
						
						//STATIC STRING
						GUI.TextArea(new Rect(inMenuRect.width*0.24f, inMenuRect.height*0.01f, inMenuRect.width*0.6f, inMenuRect.height*0.21f), "- STATS -");
					
						GUI.skin.textArea.fontSize = 30;
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.2f, inMenuRect.width*0.3f, inMenuRect.height*0.21f), "DEAD NPC");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "WHITE COLLECTIBLES");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.4f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "RED COLLECTIBLES");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.5f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "BLUE COLLECTIBLES");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "COMIC THUMBS");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "ACHIEVEMENT");
					
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL DEATH");
						GUI.TextArea(new Rect(inMenuRect.width*0.15f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), "TOTAL PLAYTIME");
					
						//STATISTIC STRING
						GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.2f, inMenuRect.width*0.3f, inMenuRect.height*0.21f), _StatsDisplay.DeadNPCString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.whiteCollString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.4f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.redCollString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.5f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.blueCollString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.6f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.comicThumbsString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.7f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.achievementString);
						GUI.TextArea(new Rect(inMenuRect.width*0.675f, inMenuRect.height*0.8f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.DeathCountString);
						GUI.TextArea(new Rect(inMenuRect.width*0.65f, inMenuRect.height*0.9f, inMenuRect.width*0.55f, inMenuRect.height*0.21f), _StatsDisplay.totalPlaytimeString);
					GUI.EndGroup();
					
					//HIGHSCORE BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(470, 605, 385, 80), "- HIGHSCORES -")){
						
					}
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
					
					#region DeleteOption
				case _OptionWindowsEnum.DeleteSavegame:
					
					GUI.BeginGroup(inMenuRect);
						GUI.skin.textArea.fontSize = 52;
						GUI.TextArea(new Rect(inMenuRect.width*0.05f, inMenuRect.height*0.14f, inMenuRect.width*0.9f, inMenuRect.height*0.2f), "- DELETE THE ACTUAL -");
						GUI.skin.textArea.fontSize = 46;
						GUI.TextArea(new Rect(inMenuRect.width*0.3f, inMenuRect.height*0.3f, inMenuRect.width*0.55f, inMenuRect.height*0.2f), "- SAVED GAME -");
						GUI.skin.textArea.fontSize = 55;
						GUI.TextArea(new Rect(inMenuRect.width*0.1f, inMenuRect.height*0.51f, inMenuRect.width*0.7f, inMenuRect.height*0.28f), "? ARE YOU SURE ?");
						
						GUI.skin.button.fontSize = 60;
						if(GUI.Button(new Rect(inMenuRect.width*0.12f, inMenuRect.height*0.73f, 250, 80), "YES")){
							File.Delete(Application.persistentDataPath + "/" + "Chromasave");
							_FirstStart = true;
							_MenuWindows = _MenuWindowsEnum.MainMenu;
							_OptionWindows = _OptionWindowsEnum.MainOption;
							Debug.Log("GAME DELETER");
						}
						if(GUI.Button(new Rect(inMenuRect.width*0.4f, inMenuRect.height*0.73f, 250, 80), "NO")){
							_OptionWindows = _OptionWindowsEnum.MainOption;
						}
					
					GUI.EndGroup();
					
					
					//BACK BUTTON
					GUI.skin.button.fontSize = 48;
					if(GUI.Button(new Rect(125, 605, 300, 80), "- BACK -")){
						_OptionWindows = _OptionWindowsEnum.MainOption;
					}
					
					break;
					#endregion
				}
				
				break;
				#endregion
	
			#region Loading Screen
			case _MenuWindowsEnum.LoadingScreen:
				
				Rect inLoadRect = new Rect(217.5f, 165, 740, 380);
				
				//BACKGROUND
				GUI.DrawTexture(new Rect(0, 0, 1280, 960), _LoadingText);
				
				//Debug.Log(async.progress);
				
				
				//IMAGE AVATAR
				loopingCounter += Time.deltaTime;
				if(loopingCounter > 1){
					loopingCounter = 0;
					_loop2 = !_loop2;
				}
				if(!_loop2){
					GUI.DrawTexture(new Rect(333, 610, 225, 225), _AvatarLoadingLoop1);
				}
				else{
					GUI.DrawTexture(new Rect(333, 610, 225, 225), _AvatarLoadingLoop2);
				}
				
				
				if(!_loop2){
					GUI.DrawTexture(new Rect(505, 480, 410, 410), _BulleLoadLoop1);
				}
				else{
					GUI.DrawTexture(new Rect(505, 480, 410, 410), _BulleLoadLoop2);
				}
				
				
									
				GUI.skin = _GreenlightButton;
					if(GUI.Button(new Rect(578, 615, 273, 120), "")){
						//Application.OpenURL("http://steamcommunity.com/sharedfiles/filedetails/?id=174349688");
						Application.ExternalEval("window.open('http://steamcommunity.com/sharedfiles/filedetails/?id=174349688','Chromatose Greenlight Page')");
					}
				
			
				
				
				GUI.BeginGroup(inLoadRect);
				
				
				//GUI.DrawTexture(new Rect(130, 262, 590, inLoadRect.height*0.2f), mainMenuButtton.emptyProgressBar);
							
				
				//TODO Gerer la Progress Bar differement selon WebPlayer/StandAlone
					//PROGRESS BAR
				if(async != null){
					GUI.DrawTexture(new Rect(150, 278, 6 * async.progress * 100f, inLoadRect.height*0.10f), mainMenuButtton.progressLine);
					GUI.DrawTexture(new Rect(130, 262, 590, inLoadRect.height*0.2f), mainMenuButtton.emptyProgressBar);
				}	
								
				/*
					if(_LoadCounter > 50){GUI.TextArea(new Rect(inLoadRect.width*0.605f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
					else if(_LoadCounter > 100){GUI.TextArea(new Rect(inLoadRect.width*0.625f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}
					else if(_LoadCounter > 150){GUI.TextArea(new Rect(inLoadRect.width*0.645f, inLoadRect.height*0.25f,inLoadRect.width*0.2f, inLoadRect.height*0.3f), ".");}*/
				/*GUI.EndGroup();
				
				break;
				#endregion
				
			#region Credits Windows
			case _MenuWindowsEnum.CreditWindows:
				GUI.DrawTexture(new Rect(0, 0, 1280, 960), mainMenuButtton.credits);
				GUI.skin = _BackButtonSkin;
				if(GUI.Button(new Rect(280, 700, 180, 180), "")){
					_MenuWindows = _MenuWindowsEnum.MainMenu;
				}
				
				break;
				#endregion
			}
			break;
#endregion			
		}
	}
	
	private IEnumerator LoadALevel(int levelInt){
		
		async = Application.LoadLevelAsync(levelInt);
		
		yield return async;
	}	                */
}
