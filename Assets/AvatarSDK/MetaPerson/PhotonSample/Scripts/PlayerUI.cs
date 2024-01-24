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

namespace AvatarSDK.MetaPerson.Photon
{
	public class PlayerUI : MonoBehaviour
	{
		public Text playerNameText;

		public Text avatarLoadingText;

		public Vector3 screenOffset = new Vector3(0f, 30f, 0f);

		private PlayerManager target;

		private float characterControllerHeight = 0f;
		private Transform targetTransform;
		private Renderer targetRenderer;
		private CanvasGroup canvasGroup;
		private Vector3 targetPosition;

		void Update()
		{
			if (target == null)
			{
				Destroy(this.gameObject);
				return;
			}
		}

		void Awake()
		{
			transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
			canvasGroup = this.GetComponent<CanvasGroup>();
		}

		void LateUpdate()
		{
			if (targetRenderer != null)
			{
				canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
			}

			if (targetTransform != null)
			{
				targetPosition = targetTransform.position;
				targetPosition.y += characterControllerHeight;
				this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
			}
		}

		public void SetTarget(PlayerManager target)
		{
			this.target = target;
			if (playerNameText != null)
			{
				playerNameText.text = target.photonView.Owner.NickName;
			}

			targetTransform = this.target.GetComponent<Transform>();
			targetRenderer = this.target.GetComponent<Renderer>();
			CharacterController characterController = target.GetComponent<CharacterController>();
			if (characterController != null)
			{
				characterControllerHeight = characterController.height;
			}
		}

		public void DisableLoadingText()
		{
			avatarLoadingText.gameObject.SetActive(false);
		}
	}
}