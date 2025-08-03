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
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AvatarSDK.MetaPerson.Photon
{
	public class LauncherSceneHandler : MonoBehaviour
	{
		public InputField nameInputField;

		public List<AvatarPreview> avatars;

		private int currentAvatarIdx = 0;

		private void Start()
		{
			string defaultName = string.Empty;
			if (PlayerPrefs.HasKey(StringConstants.PlayerNamePrefKey))
			{
				defaultName = PlayerPrefs.GetString(StringConstants.PlayerNamePrefKey);
				nameInputField.text = defaultName;
			}

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
			PlayerPrefs.SetString(StringConstants.PlayerNamePrefKey, value);
		}

		public void OnPlayButtonClick()
		{
			PlayerPrefs.SetString(StringConstants.AvatarLinkPropertyName, avatars[currentAvatarIdx].modelUrl);
			PlayerPrefs.SetString(StringConstants.AvataGenderPrefKey, avatars[currentAvatarIdx].avatarGender.AvatarGenderToString());
			SceneManager.LoadScene(StringConstants.GameSceneName);
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
	}
}