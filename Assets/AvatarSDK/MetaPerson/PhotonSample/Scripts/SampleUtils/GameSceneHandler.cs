/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, July 2025
*/

using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AvatarSDK.MetaPerson.Photon
{
	public class GameSceneHandler : MonoBehaviour
	{
		public Button backButon;

		public Text connectingText;

		private NetworkRunner networkRunner;

		public void OnConnected(NetworkRunner networkRunner)
		{
			this.networkRunner = networkRunner;
			connectingText.gameObject.SetActive(false);
			backButon.gameObject.SetActive(true);
		}

		public void OnBackButtonClicked()
		{
			networkRunner.Shutdown();
			SceneManager.LoadScene(StringConstants.LauncherSceneName);
		}
	}
}
