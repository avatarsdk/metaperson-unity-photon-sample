/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, July 2025
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fusion;

namespace AvatarSDK.MetaPerson.Photon
{
	public class PlayerMovementController : NetworkBehaviour
	{
		public Animator animator = null;

		public float moveSpeed = 1.5f;

		private bool isMoving = false;

		[Networked, OnChangedRender(nameof(OnAnimationChanged))]
		public string AnimationStateName { get; set; }

		void Start()
		{
			if (!animator)
				animator = GetComponent<Animator>();
			if (!animator)
			{
				Debug.LogError("PlayerMovementController is Missing Animator Component", this);
			}
		}

		private void LateUpdate()
		{
			if (animator != null && HasStateAuthority)
			{
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					if (!isMoving)
					{
						SwitchAnimation("Walk");
						isMoving = true;
					}

					Vector3 currentRotation = transform.eulerAngles;
					Vector3 cameraRotation = Camera.main.transform.eulerAngles;
					transform.rotation = Quaternion.Euler(currentRotation.x, cameraRotation.y, currentRotation.z);
					transform.position += transform.forward * Runner.DeltaTime * moveSpeed;
				}
				else
				{
					if (isMoving)
					{
						SwitchAnimation("Idle");
						isMoving = false;
					}
				}
			}
		}

		private void SwitchAnimation(string animationStateName)
		{
			animator.Play(animationStateName);
			if (HasStateAuthority)
				AnimationStateName = animationStateName;
		}

		private void OnAnimationChanged()
		{
			animator.Play(AnimationStateName);
		}
	}
}