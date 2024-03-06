/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, March 2024
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSDK.MetaPerson.Photon
{
	public class CustomAvatarPreview : AvatarPreview
	{
		protected override void OnEnable()
		{
			modelUrl = PlayerPrefs.HasKey(StringConstants.PlayerCustomAvatarLinkPrefKey) ? PlayerPrefs.GetString(StringConstants.PlayerCustomAvatarLinkPrefKey) : "";
			bool hasCustomAvatar = !string.IsNullOrEmpty(modelUrl);

			playButton.interactable = hasCustomAvatar;

			avatarNameText.text = modelName;
			avatarNameText.gameObject.SetActive(!hasCustomAvatar);

			progressSlider.value = loadingProgress;
			progressSlider.gameObject.SetActive(!isAvatarLoaded && hasCustomAvatar);

			createAvatarButton.gameObject.SetActive(!hasCustomAvatar);
			customizeAvatarButton.gameObject.SetActive(hasCustomAvatar);
		}
	}
}
