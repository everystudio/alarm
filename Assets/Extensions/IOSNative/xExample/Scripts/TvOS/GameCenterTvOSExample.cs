using UnityEngine;
using System.Collections;

public class GameCenterTvOSExample : MonoBehaviour {



	private int hiScore = 200;


	private string TEST_LEADERBOARD_1 = "your.ios.leaderbord1.id";
	private string TEST_LEADERBOARD_2 = "combined.board.1";



	private string TEST_ACHIEVEMENT_1_ID = "your.achievement.id1.here";
	private string TEST_ACHIEVEMENT_2_ID = "your.achievement.id2.here";


	private static bool IsInitialized = false;
	private static long LB2BestScores = 0;


	void Start () {
		GameCenterManager.OnAuthFinished += OnAuthFinished;
		GameCenterManager.OnScoreSubmitted += OnScoreSubmitted;

		GameCenterManager.OnAchievementsProgress += HandleOnAchievementsProgress;
		GameCenterManager.OnAchievementsReset += HandleOnAchievementsReset;
		GameCenterManager.OnAchievementsLoaded += OnAchievementsLoaded;

		//Achievement registration. If you skip this step GameCenterManager.achievements array will contain only achievements with reported progress 
		GameCenterManager.RegisterAchievement (TEST_ACHIEVEMENT_1_ID);
		GameCenterManager.RegisterAchievement (TEST_ACHIEVEMENT_2_ID);





		//Initializing Game Center class. This action will trigger authentication flow
		GameCenterManager.Init();
	}
	
	void OnAuthFinished (ISN_Result res) {
		if (res.IsSucceeded) {
			IOSNativePopUpManager.showMessage("Player Authed ", "ID: " + GameCenterManager.Player.Id + "\n" + "Alias: " + GameCenterManager.Player.Alias);
			GameCenterManager.LoadLeaderboardInfo(TEST_LEADERBOARD_1);
		} else {
			IOSNativePopUpManager.showMessage("Game Center ", "Player authentication failed");
		}
	}


	public void ShowAchivemnets() {
		Debug.Log("ShowAchivemnets");
		GameCenterManager.ShowAchievements();


	}

	public void SubmitAchievement() {
		Debug.Log("SubmitAchievement");
		GameCenterManager.SubmitAchievement(GameCenterManager.GetAchievementProgress(TEST_ACHIEVEMENT_1_ID) + 2.432f, TEST_ACHIEVEMENT_1_ID);

	}
		
	public void ResetAchievements() {
		Debug.Log("ResetAchievements");
		GameCenterManager.ResetAchievements();
	}
		

	public void ShowLeaderboards() {
		Debug.Log("ShowLeaderboards");
		GameCenterManager.ShowLeaderboards ();
	}


	public void ShowLeaderboardByID() {
		Debug.Log("ShowLeaderboardByID");
		GameCenterManager.OnFriendsListLoaded += (ISN_Result obj) => {
			Debug.Log("Loaded: " + GameCenterManager.FriendsList.Count + " fiends");
		};
		GameCenterManager.RetrieveFriends();
	}


	public void ReportScore() {
		Debug.Log("ReportScore");
		hiScore++;

		GameCenterManager.ReportScore(hiScore, TEST_LEADERBOARD_1, 17);
	}

	void OnScoreSubmitted (GK_LeaderboardResult result) {
		
		if(result.IsSucceeded) {
			GK_Score score = result.Leaderboard.GetCurrentPlayerScore(GK_TimeSpan.ALL_TIME, GK_CollectionType.GLOBAL);
			IOSNativePopUpManager.showMessage("Leaderboard " + score.LongScore, "Score: " + score.LongScore + "\n" + "Rank:" + score.Rank);
		}
	}



	private void OnAchievementsLoaded(ISN_Result result) {

		ISN_Logger.Log("OnAchievementsLoaded");
		ISN_Logger.Log(result.IsSucceeded);

		if(result.IsSucceeded) {
			ISN_Logger.Log ("Achievements were loaded from iOS Game Center");

			foreach(GK_AchievementTemplate tpl in GameCenterManager.Achievements) {
				ISN_Logger.Log (tpl.Id + ":  " + tpl.Progress);
			}
		}

	}

	void HandleOnAchievementsReset (ISN_Result obj){
		ISN_Logger.Log ("All Achievements were reset");
	}


	private void HandleOnAchievementsProgress (GK_AchievementProgressResult result) {
		if(result.IsSucceeded) {
			GK_AchievementTemplate tpl = result.Achievement;
			ISN_Logger.Log (tpl.Id + ":  " + tpl.Progress.ToString());
		}
	}


}
