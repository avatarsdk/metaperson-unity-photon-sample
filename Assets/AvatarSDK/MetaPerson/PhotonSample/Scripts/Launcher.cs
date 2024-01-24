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

namespace AvatarSDK.MetaPerson.Photon
{
	public class Launcher : MonoBehaviourPunCallbacks
	{
		public int maxPlayersPerRoom = 4;

		public GameObject controlPanel;

		public GameObject progressLabel;

		public Dropdown avatarsDropdown;

		public InputField nameInputField;

		private string gameVersion = "1";

		private bool isConnecting = false;

		private void Start()
		{
			string defaultName = string.Empty;
			if (PlayerPrefs.HasKey(StringConstants.PlayerNamePrefKey))
			{
				defaultName = PlayerPrefs.GetString(StringConstants.PlayerNamePrefKey);
				nameInputField.text = defaultName;
			}

			PhotonNetwork.NickName = defaultName;
		}

		public void Connect()
		{
			progressLabel.SetActive(true);
			controlPanel.SetActive(false);

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

			progressLabel.SetActive(false);
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

		private void StoreSelectedAvatarLink()
		{
			if (PhotonNetwork.InRoom)
			{
				ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
				customProperties[StringConstants.AvatarLinkPropertyName] = AvatarsList.avatarsLinks[avatarsDropdown.value];
				PhotonNetwork.SetPlayerCustomProperties(customProperties);
			}
		}
	}
}