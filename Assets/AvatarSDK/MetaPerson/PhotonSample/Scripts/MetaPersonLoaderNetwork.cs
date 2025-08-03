/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, July 2025
*/

using AvatarSDK.MetaPerson.Loader;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSDK.MetaPerson.Photon
{
	public class MetaPersonLoaderNetwork : NetworkBehaviour
	{
		public MetaPersonLoader metaPersonLoader;

		public event Action modelLoaded;

		private bool isDestroyed = false;

		[Networked, Capacity(128)]
		public string AvatarURL { get; set; }

		[Networked]
		public bool AvatarRootHeightCorrectionRequired { get; set; } = false;

		[Networked]
		public float HipsTargetYPosition { get; set; }

		void Start()
		{
			if (!string.IsNullOrEmpty(AvatarURL))
				StartAvatarLoading(AvatarURL);
		}

		private void OnDestroy()
		{
			isDestroyed = true;
		}

		private async void StartAvatarLoading(string avatarLink)
		{
			Debug.LogFormat("PlayerManager: StartAvatarLoading - {0}", avatarLink);

			GameObject loadedAvatarObject = new GameObject("Loaded Avatar");
			metaPersonLoader.avatarObject = loadedAvatarObject;
			metaPersonLoader.avatarObject.SetActive(false);
			bool isAvatarLoaded = await metaPersonLoader.LoadModelAsync(avatarLink);
			if (isAvatarLoaded)
			{
				if (!isDestroyed)
				{
					if (AvatarRootHeightCorrectionRequired)
						CorrectAvatarRootHeight(loadedAvatarObject.transform);
					MetaPersonUtils.ReplaceAvatar(metaPersonLoader.avatarObject, gameObject);
				}

				Destroy(loadedAvatarObject);

				modelLoaded?.Invoke();
			}
			else
				Debug.LogError("Unable to load avatar");
		}

		private void CorrectAvatarRootHeight(Transform loadedAvatarTransform)
		{
			Transform avatarRootTransform = FindTransformByName(transform, "AvatarRoot");
			Transform hipsTransform = FindTransformByName(loadedAvatarTransform, "Hips");
			if (avatarRootTransform == null || hipsTransform == null)
			{
				Debug.LogWarningFormat("Unable to correct female position");
				return;
			}

			avatarRootTransform.localPosition += new Vector3(0, hipsTransform.position.y - HipsTargetYPosition, 0);
		}

		private Transform FindTransformByName(Transform root, string targetName)
		{
			var queue = new Queue<Transform>();
			queue.Enqueue(root);

			while (queue.Count > 0)
			{
				var current = queue.Dequeue();

				if (current.name == targetName)
				{
					return current;
				}

				foreach (Transform child in current)
				{
					queue.Enqueue(child);
				}
			}

			return null;
		}
	}
}
