/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, January 2024
*/

using Fusion;
using UnityEngine;

namespace AvatarSDK.MetaPerson.Photon
{
	public class CameraController : NetworkBehaviour
	{
		public float distance = 7.0f;

		public float height = 3.0f;

		public Vector3 centerOffset = Vector3.zero;

		public float mouseSensitivity = 10f;

		private Transform cameraTransform;

		private void Start()
		{
			if (HasStateAuthority)
			{
				cameraTransform = Camera.main.transform;
				PlaceCameraBehindOfTarget();
			}
		}

		private void LateUpdate()
		{
			if (HasStateAuthority)
			{
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
					PlaceCameraBehindOfTarget();

				if (Input.GetMouseButton(0))
				{
					float mouseX = Input.GetAxis("Mouse X");
					float mouseY = Input.GetAxis("Mouse Y");

					cameraTransform.RotateAround(transform.position, Vector3.up, mouseX * mouseSensitivity);
				}
			}
		}

		private void PlaceCameraBehindOfTarget()
		{
			Vector3 cameraOffset = new Vector3(0, height, -distance);
			cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);
			cameraTransform.LookAt(this.transform.position + centerOffset);
		}
	}
}