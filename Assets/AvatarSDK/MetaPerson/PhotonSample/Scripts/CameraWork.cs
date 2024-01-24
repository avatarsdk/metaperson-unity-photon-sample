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

namespace AvatarSDK.MetaPerson.Photon
{
	/// <summary>
	/// Camera work. Follow a target
	/// </summary>
	public class CameraWork : MonoBehaviour
	{
		public float distance = 7.0f;

		public float height = 3.0f;

		public Vector3 centerOffset = Vector3.zero;

		public bool followOnStart = false;

		public float smoothSpeed = 0.125f;

		private Transform cameraTransform;

		private bool isFollowing;

		private Vector3 cameraOffset = Vector3.zero;

		private void Start()
		{
			if (followOnStart)
			{
				OnStartFollowing();
			}
		}


		private void LateUpdate()
		{
			if (cameraTransform == null && isFollowing)
			{
				OnStartFollowing();
			}

			if (isFollowing)
			{
				Follow();
			}
		}

		public void OnStartFollowing()
		{
			cameraTransform = Camera.main.transform;
			isFollowing = true;
			Cut();
		}

		private void Follow()
		{
			cameraOffset.z = -distance;
			cameraOffset.y = height;

			cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

			cameraTransform.LookAt(this.transform.position + centerOffset);
		}

		private void Cut()
		{
			cameraOffset.z = -distance;
			cameraOffset.y = height;

			cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

			cameraTransform.LookAt(this.transform.position + centerOffset);
		}
	}
}