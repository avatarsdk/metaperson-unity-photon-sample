/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, March 2024
*/

using AvatarSDK.MetaPerson.Loader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarSDK.MetaPerson.Photon
{
	public class AvatarPreview : MonoBehaviour
	{
		public string modelName;

		public string modelUrl;

		public MetaPersonLoader metaPersonLoader;

		public Text avatarNameText;

		public Slider progressSlider;

		public Button playButton;

		public Button createAvatarButton;

		public Button customizeAvatarButton;

		public RuntimeAnimatorController animatorController;

		protected bool isAvatarLoaded = false;

		protected float loadingProgress = 0;

		protected virtual async void Start()
		{
			if (!string.IsNullOrEmpty(modelUrl))
			{
				isAvatarLoaded = await metaPersonLoader.LoadModelAsync(modelUrl, OnLoadingProgressChanged);
				if (isAvatarLoaded)
				{
					if (gameObject.activeSelf)
						progressSlider.gameObject.SetActive(false);

					HumanoidAnimatorBuilder humanoidAnimatorBuilder = new HumanoidAnimatorBuilder();
					humanoidAnimatorBuilder.AddHumanoidAnimator(metaPersonLoader.avatarObject);
					humanoidAnimatorBuilder.SetAnimatorController(animatorController, metaPersonLoader.avatarObject);
				}
				else
				{
					Debug.LogErrorFormat("Unable to load model from url: {0}", modelUrl);
				}
			}
		}

		protected virtual void OnEnable()
		{
			playButton.interactable = !string.IsNullOrEmpty(modelUrl);

			avatarNameText.text = modelName;
			avatarNameText.gameObject.SetActive(true);

			progressSlider.value = loadingProgress;
			progressSlider.gameObject.SetActive(!isAvatarLoaded);

			createAvatarButton.gameObject.SetActive(false);
			customizeAvatarButton.gameObject.SetActive(false);
		}

		private void OnLoadingProgressChanged(float p)
		{
			loadingProgress = p;
			if (gameObject.activeSelf)
				progressSlider.value = p;
		}
	}
}
