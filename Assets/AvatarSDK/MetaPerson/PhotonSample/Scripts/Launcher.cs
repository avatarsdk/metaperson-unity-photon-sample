/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, January 2024
*/

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AvatarSDK.MetaPerson.Photon
{
	public class Launcher : MonoBehaviourPunCallbacks
	{
		public int maxPlayersPerRoom = 4;

		public GameObject controlPanel;

		public GameObject connectingLabel;

		public InputField nameInputField;

		public List<AvatarPreview> avatars;

		private string gameVersion = "1";

		private bool isConnecting = false;

		private int currentAvatarIdx = 0;

		private void Start()
		{
			string defaultName = string.Empty;
			if (PlayerPrefs.HasKey(StringConstants.PlayerNamePrefKey))
			{
				defaultName = PlayerPrefs.GetString(StringConstants.PlayerNamePrefKey);
				nameInputField.text = defaultName;
			}
			PhotonNetwork.NickName = defaultName;

			if (!PlayerPrefs.HasKey(StringConstants.PlayerCustomAvatarLinkPrefKey))
				OnNextAvatarButtonClick();
		}

		public void SetPlayerName(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Debug.LogError("Player Name is null or empty");
				return;
			}
			PhotonNetwork.NickName = value;
			PlayerPrefs.SetString(StringConstants.PlayerNamePrefKey, value);
		}

		public void OnPlayButtonClick()
		{
			connectingLabel.SetActive(true);
			controlPanel.SetActive(false);

			Connect();
		}

		public void OnNextAvatarButtonClick()
		{
			avatars[currentAvatarIdx].gameObject.SetActive(false);
			currentAvatarIdx++;
			if (currentAvatarIdx >= avatars.Count)
				currentAvatarIdx = 0;
			avatars[currentAvatarIdx].gameObject.SetActive(true);
		}

		public void OnPrevAvatarButtonClick()
		{
			avatars[currentAvatarIdx].gameObject.SetActive(false);
			currentAvatarIdx--;
			if (currentAvatarIdx < 0)
				currentAvatarIdx = avatars.Count - 1;
			avatars[currentAvatarIdx].gameObject.SetActive(true);
		}

		public void OpenAvatarSelectionScene()
		{
			SceneManager.LoadScene(StringConstants.AvatarSelectionSceneName);
		}

		public override void OnConnectedToMaster()
		{
			Debug.Log("Launcher: OnConnectedToMaster");

			if (isConnecting)
			{
				PhotonNetwork.JoinRandomRoom();
				isConnecting = false;
			}
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			Debug.LogWarningFormat("Launcher: OnDisconnected, reason: {0}", cause);

			connectingLabel.SetActive(false);
			controlPanel.SetActive(true);

			isConnecting = false;
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("Launcher: OnJoinRandomFailed");

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("Launcher: OnJoinedRoom");
			StoreSelectedAvatarLink();
			PhotonNetwork.LoadLevel(StringConstants.GameSceneName);
		}

		private void Connect()
		{
			if (PhotonNetwork.IsConnected)
			{
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				isConnecting = PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = gameVersion;
			}
		}

		private void StoreSelectedAvatarLink()
		{
			if (PhotonNetwork.InRoom)
			{
				ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
				customProperties[StringConstants.AvatarLinkPropertyName] = avatars[currentAvatarIdx].modelUrl;
				PhotonNetwork.SetPlayerCustomProperties(customProperties);
			}
		}
	}
}