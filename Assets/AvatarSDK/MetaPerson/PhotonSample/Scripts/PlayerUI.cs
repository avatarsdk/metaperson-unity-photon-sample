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
using System.Collections;
using Fusion;

namespace AvatarSDK.MetaPerson.Photon
{
	public class PlayerUI : NetworkBehaviour
	{
		public Text playerNameText;

		public Text avatarLoadingText;

		public Vector3 screenOffset = new Vector3(0f, 30f, 0f);

		public GameObject target;

		public  float avatarHeight = 2.0f;
		
		private Transform targetTransform;
		private Renderer targetRenderer;
		private CanvasGroup canvasGroup;
		private Vector3 targetPosition;

		[Networked, Capacity(32)]
		public string PlayerName { get; set; }

		private void Start()
		{
			transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
			canvasGroup = this.GetComponent<CanvasGroup>();

			if (playerNameText != null)
			{
				playerNameText.text = PlayerName;
			}

			targetTransform = target.GetComponent<Transform>();
			targetRenderer = target.GetComponent<Renderer>();

			MetaPersonLoaderNetwork metaPersonLoaderNetwork = target.GetComponent<MetaPersonLoaderNetwork>();
			if (metaPersonLoaderNetwork != null)
				metaPersonLoaderNetwork.modelLoaded += DisableLoadingText;
			else
				DisableLoadingText();
		}

		void LateUpdate()
		{
			if (target == null)
			{
				Destroy(gameObject);
				return;
			}

			if (targetRenderer != null)
			{
				canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
			}

			if (targetTransform != null)
			{
				targetPosition = targetTransform.position;
				targetPosition.y += avatarHeight;
				transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
			}
		}

		private void DisableLoadingText()
		{
			avatarLoadingText.gameObject.SetActive(false);
		}
	}
}