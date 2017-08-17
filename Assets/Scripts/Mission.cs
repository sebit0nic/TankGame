using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

[System.Serializable]
public class Mission {

	public int highscore;
	public int experienceReward;
	public bool completedOnce;
	public Vector3 playerStartPosition = new Vector3 (0, 0.8f, 0);
	[SerializeField]
	public PrimaryTask primaryTask;

	public void Init(GameManager gameManager, int loadedLevel, PlayerManager playerManager) {
		TextAsset textAsset = (TextAsset)Resources.Load ("Missions");
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (textAsset.text);
		XmlNode root = doc.FirstChild;
		XmlNode currentMission = null;

		if (root.HasChildNodes) {
			for (int i = 0; i < root.ChildNodes.Count; i++) {
				if (XmlConvert.ToInt32 (root.ChildNodes [i].Attributes.Item (0).Value) == loadedLevel) {
					currentMission = root.ChildNodes [i];
					break;
				}
			}
		}

		if (currentMission.HasChildNodes) {
			for (int i = 0; i < currentMission.ChildNodes.Count; i++) {
				switch (currentMission.ChildNodes [i].Name) {
				case "Highscore":
					highscore = XmlConvert.ToInt32 (currentMission.ChildNodes [i].InnerText);
					break;
				case "ExperienceReward":
					experienceReward = XmlConvert.ToInt32 (currentMission.ChildNodes [i].InnerText);
					break;
				case "CompletedOnce":
					completedOnce = XmlConvert.ToBoolean (currentMission.ChildNodes [i].InnerText);
					break;
				case "PrimaryTask":
					primaryTask.Init (gameManager, currentMission.ChildNodes [i]);
					break;
				case "PlayerPosition":
					XmlNode playerPosNode = currentMission.ChildNodes [i];
					playerStartPosition = new Vector3 (
						XmlConvert.ToSingle (playerPosNode.ChildNodes [0].InnerText),
						XmlConvert.ToSingle (playerPosNode.ChildNodes [1].InnerText),
						XmlConvert.ToSingle (playerPosNode.ChildNodes [2].InnerText));
					break;
				}
			}
		}

		playerManager.InitPosition (playerStartPosition);
	}

	public void Update() {
		primaryTask.Update ();
	}

	public void NotifyEnemyDestroyed() {
		primaryTask.NotifyEnemyDestroyed ();
	}
}
