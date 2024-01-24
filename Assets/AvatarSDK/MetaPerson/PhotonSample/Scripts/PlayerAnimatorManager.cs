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
using System.Collections;
using Photon.Pun;

namespace AvatarSDK.MetaPerson.Photon
{
	public class PlayerAnimatorManager : MonoBehaviourPun
	{
		public float directionDampTime = 0.25f;

		public Animator animator = null;

		void Start()
		{
			if (!animator)
				animator = GetComponent<Animator>();
			if (!animator)
			{
				Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
			}
		}

		void Update()
		{
			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			{
				return;
			}

			if (!animator)
			{
				return;
			}

			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetButtonDown("Jump"))
				{
					animator.SetTrigger("Jump");
				}
			}

			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			if (v < 0)
			{
				v = 0;
			}
			animator.SetFloat("Speed", h * h + v * v);
			animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
		}
	}
}