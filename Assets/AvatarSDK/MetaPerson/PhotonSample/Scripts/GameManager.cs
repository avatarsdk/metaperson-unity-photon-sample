/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, January 2024
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace AvatarSDK.MetaPerson.Photon
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		private class TransformData
		{
			public Vector3 position;
			public Quaternion rotation;
		}

		public GameObject metaPersonPrefab;

		void Start()
		{
			if (!PhotonNetwork.IsConnected)
			{
				SceneManager.LoadScene(StringConstants.LauncherSceneName);
				return;
			}

			if (metaPersonPrefab == null)
			{
				Debug.LogError("metaPersonPrefab isn't set!");
			}
			else
			{
				if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
				{
					Debug.Log("GameManager: instantiating local MetaPerson prefab from Start");
					TransformData pos = GetInitialAvatarPosition();
					PhotonNetwork.Instantiate(metaPersonPrefab.name, pos.position, pos.rotation, 0);
				}
			}
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("GameManager: OnJoinedRoom");

			if (PlayerManager.LocalPlayerInstance == null)
			{
				Debug.LogFormat("GameManager: instantiating local MetaPerson prefab from OnJoinedRoom}");
				TransformData pos = GetInitialAvatarPosition();
				PhotonNetwork.Instantiate(metaPersonPrefab.name, pos.position, pos.rotation, 0);
			}
		}

		public override void OnLeftRoom()
		{
			SceneManager.LoadScene(StringConstants.LauncherSceneName);
		}

		public override void OnPlayerEnteredRoom(Player other)
		{
			Debug.LogFormat("GameManager: OnPlayerEnteredRoom - {0}", other.NickName);
		}

		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.LogFormat("GameManager: OnPlayerLeftRoom - {0}", other.NickName);
		}

		public void LeaveRoom()
		{
			ClearSelectedAvatarLink();
			PhotonNetwork.LeaveRoom();
		}

		private void ClearSelectedAvatarLink()
		{
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
			customProperties[StringConstants.AvatarLinkPropertyName] = string.Empty;
			PhotonNetwork.SetPlayerCustomProperties(customProperties);
		}

		private TransformData GetInitialAvatarPosition()
		{
			System.Random random = new System.Random(DateTime.Now.Millisecond);
			double angleRad = random.NextDouble() * Math.PI * 2.0f;

			TransformData transformData = new TransformData();
			transformData.position = new Vector3(-(float)Math.Sin(angleRad) * 2.0f, 0.1f, -(float)Math.Cos(angleRad) * 2.0f);
			transformData.rotation = Quaternion.Euler(0, (float)(angleRad * 180.0f / Math.PI), 0);
			return transformData;
		}
	}
}