/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, July 2025
*/

using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSDK.MetaPerson.Photon
{
	public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
	{
		private class TransformData
		{
			public Vector3 position;
			public Quaternion rotation;
		}

		public GameObject malePlayerPrefab;

		public GameObject femalePlayerPrefab;

		public void PlayerJoined(PlayerRef player)
		{
			if (player == Runner.LocalPlayer)
			{
				TransformData pos = GetInitialAvatarPosition();

				AvatarGender avatarGender = AvatarGender.Male;
				if (PlayerPrefs.HasKey(StringConstants.AvataGenderPrefKey))
					avatarGender = PlayerPrefs.GetString(StringConstants.AvataGenderPrefKey).StringToAvatarGender();
				else
					Debug.LogWarning("Avatar gender wasn't specified");

				GameObject playerPrefab = GetPlayerPrefab();
				var spawnedPlayer = Runner.Spawn(playerPrefab, pos.position, pos.rotation);

				if (PlayerPrefs.HasKey(StringConstants.AvatarLinkPropertyName))
				{
					string avatarUrl = PlayerPrefs.GetString(StringConstants.AvatarLinkPropertyName);
					MetaPersonLoaderNetwork metaPersonLoaderNetwork = spawnedPlayer.gameObject.GetComponent<MetaPersonLoaderNetwork>();
					if (metaPersonLoaderNetwork != null)
					{
						metaPersonLoaderNetwork.AvatarURL = PlayerPrefs.GetString(StringConstants.AvatarLinkPropertyName);
						
						if (avatarGender == AvatarGender.Female)
						{
							metaPersonLoaderNetwork.AvatarRootHeightCorrectionRequired = true;
							metaPersonLoaderNetwork.HipsTargetYPosition = 0.9379275f;
						}
					}
				}

				if (PlayerPrefs.HasKey(StringConstants.PlayerNamePrefKey))
				{
					PlayerUI playerUI = spawnedPlayer.GetComponentInChildren<PlayerUI>();
					if (playerUI != null)
						playerUI.PlayerName = PlayerPrefs.GetString(StringConstants.PlayerNamePrefKey);
				}
			}
		}

		private TransformData GetInitialAvatarPosition()
		{
			System.Random random = new System.Random(DateTime.Now.Millisecond);
			double angleRad = random.NextDouble() * Math.PI * 2.0f;

			TransformData transformData = new TransformData();
			transformData.position = new Vector3(-(float)Math.Sin(angleRad) * 2.0f, 0.0f, -(float)Math.Cos(angleRad) * 2.0f);
			transformData.rotation = Quaternion.Euler(0, (float)(angleRad * 180.0f / Math.PI), 0);
			return transformData;
		}

		private GameObject GetPlayerPrefab()
		{
			AvatarGender avatarGender = AvatarGender.Male;
			if (PlayerPrefs.HasKey(StringConstants.AvataGenderPrefKey))
				avatarGender = PlayerPrefs.GetString(StringConstants.AvataGenderPrefKey).StringToAvatarGender();
			else
				Debug.LogWarning("Avatar gender wasn't specified");
			return avatarGender == AvatarGender.Male ? malePlayerPrefab : femalePlayerPrefab;
		}
	}
}
