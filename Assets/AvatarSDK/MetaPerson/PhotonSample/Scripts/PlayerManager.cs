/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, January 2024
*/

using AvatarSDK.MetaPerson.Loader;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSDK.MetaPerson.Photon
{
	public class PlayerManager : MonoBehaviourPunCallbacks
	{
		public static GameObject LocalPlayerInstance;

		public GameObject playerUiPrefab;

		public CameraWork cameraWork;

		public MetaPersonLoader metaPersonLoader;

		private PlayerUI playerUI;

		private bool isDestroyed = false;

		void Awake()
		{
			if (photonView.IsMine)
				LocalPlayerInstance = gameObject;
		}

		void Start()
		{
			if (photonView.IsMine || !PhotonNetwork.IsConnected)
			{
				cameraWork.OnStartFollowing();
			}

			GameObject playerUIObject = Instantiate(playerUiPrefab);
			playerUI = playerUIObject.GetComponent<PlayerUI>();
			if (playerUI != null)
				playerUI.SetTarget(this);

			string avatarLink = photonView.Owner.CustomProperties["AvatarLink"] as string;
			if (!string.IsNullOrEmpty(avatarLink))
			{
				StartAvatarLoading(avatarLink);
			}
		}

		private void OnDestroy()
		{
			isDestroyed = true;
		}

		public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
		{
			if (targetPlayer == photonView.Owner)
			{
				if (changedProps.ContainsKey(StringConstants.AvatarLinkPropertyName))
				{
					string avatarLink = changedProps[StringConstants.AvatarLinkPropertyName] as string;
					if (!string.IsNullOrEmpty(avatarLink))
					{
						StartAvatarLoading(avatarLink);
					}
				}
			}
		}

		private async void StartAvatarLoading(string avatarLink)
		{
			Debug.LogFormat("PlayerManager: StartAvatarLoading - {0}", avatarLink);

			metaPersonLoader.avatarObject = new GameObject("Loaded Avatar");
			metaPersonLoader.avatarObject.SetActive(false);
			bool isAvatarLoaded = await metaPersonLoader.LoadModelAsync(avatarLink);
			if (isAvatarLoaded)
			{
				if (isDestroyed)
				{
					Destroy(metaPersonLoader.avatarObject);
					return;
				}

				MetaPersonUtils.ReplaceAvatar(metaPersonLoader.avatarObject, gameObject);

				if (playerUI != null)
					playerUI.DisableLoadingText();
			}
			else
				Debug.LogError("Unable to load avatar");
		}
	}
}
