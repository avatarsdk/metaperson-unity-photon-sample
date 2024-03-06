/* Copyright (C) Itseez3D, Inc. - All Rights Reserved
* You may not use this file except in compliance with an authorized license
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* UNLESS REQUIRED BY APPLICABLE LAW OR AGREED BY ITSEEZ3D, INC. IN WRITING, SOFTWARE DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR
* CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED
* See the License for the specific language governing permissions and limitations under the License.
* Written by Itseez3D, Inc. <support@avatarsdk.com>, March 2024
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if VUPLEX_STANDALONE
using Vuplex.WebView;
#endif

namespace AvatarSDK.MetaPerson.Photon
{
#if VUPLEX_STANDALONE
	public class AvatarSelectionSceneHandler : MonoBehaviour
	{
		public string clientId;

		public string clientSecret;

		public GameObject webViewPlaceholder;

		public Text errorText;

		public Button exportButton;

		private CanvasWebViewPrefab canvasWebViewPrefab;

		private async void Start()
		{
			canvasWebViewPrefab = webViewPlaceholder.GetComponentInChildren<CanvasWebViewPrefab>();

			if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
			{
				errorText.text = "Account credentials are not provided! Go to developer dasboard: https://accounts.avatarsdk.com/developer/";
				webViewPlaceholder.SetActive(false);
			}
			else
			{
				canvasWebViewPrefab.LogConsoleMessages = true;

				await canvasWebViewPrefab.WaitUntilInitialized();

				await canvasWebViewPrefab.WebView.WaitForNextPageLoadToFinish();

				ConfigureJSApi();
			}
		}

		public void OnBackButtonClick()
		{
			SceneManager.LoadScene(StringConstants.LauncherSceneName);
		}

		public void OnExportButtonClick()
		{
			string javaScriptCode = @"
					let exportAvatarMessage = {
						'eventName': 'export_avatar'
					};
					window.postMessage(exportAvatarMessage, '*');
				";

			canvasWebViewPrefab.WebView.ExecuteJavaScript(javaScriptCode, OnJavaScriptExecuted);
		}

		private void ConfigureJSApi()
		{
			string playerAvatarCode = PlayerPrefs.HasKey(StringConstants.PlayerCustomAvatarCodePrefKey) ? PlayerPrefs.GetString(StringConstants.PlayerCustomAvatarCodePrefKey) : "";

			string javaScriptCode = @"
					const CLIENT_ID = '" + clientId + @"';
					const CLIENT_SECRET = '" + clientSecret + @"';
					const PLAYER_AVATAR_CODE = '" + playerAvatarCode + @"';

					function onWindowMessage(evt) {
						if (evt.type === 'message') {
							if (evt.data?.source === 'metaperson_creator') {
								let data = evt.data;
								let evtName = data?.eventName;
								if (evtName === 'unity_loaded') {
									onUnityLoaded(evt, data);
								} else if (evtName === 'model_exported' ||
										   evtName === 'action_availability_changed') {
									window.vuplex.postMessage(evt.data);
								}
							}
						}
					}

					function onUnityLoaded(evt, data) {
						let authenticationMessage = {
							'eventName': 'authenticate',
							'clientId': CLIENT_ID,
							'clientSecret': CLIENT_SECRET
						};
						window.postMessage(authenticationMessage, '*');

						let exportParametersMessage = {
							'eventName': 'set_export_parameters',
							'format': 'glb',
							'lod': 1,
							'textureProfile': '1K.jpg'
						};
						evt.source.postMessage(exportParametersMessage, '*');

						let uiParametersMessage = {
							'eventName': 'set_ui_parameters',
							'isExportButtonVisible' : false,
							'closeExportDialogWhenExportComlpeted' : true,
							'isLoginButtonVisible' : true
						};
						evt.source.postMessage(uiParametersMessage, '*');

						if (PLAYER_AVATAR_CODE) {
							let showAvatarMessage = {
								'eventName': 'show_avatar',
								'avatarCode': PLAYER_AVATAR_CODE
							};
							evt.source.postMessage(showAvatarMessage, '*');
						}
					}

					window.addEventListener('message', onWindowMessage);
				";

			canvasWebViewPrefab.WebView.MessageEmitted += OnWebViewMessageReceived;
			canvasWebViewPrefab.WebView.ExecuteJavaScript(javaScriptCode, OnJavaScriptExecuted);
		}

		private void OnJavaScriptExecuted(string executionResult)
		{
			Debug.LogFormat("JS execution result: {0}", executionResult);
		}

		private void OnWebViewMessageReceived(object sender, EventArgs<string> args)
		{
			Debug.LogFormat("Got WebView message: {0}", args.Value);

			try
			{
				MetaPersonEditorEvent metaPersonEditorEvent = JsonUtility.FromJson<ModelExportedEvent>(args.Value);
				if (metaPersonEditorEvent.source == "metaperson_creator")
				{
					if (metaPersonEditorEvent.eventName == "model_exported")
					{
						ModelExportedEvent modelExportedEvent = JsonUtility.FromJson<ModelExportedEvent>(args.Value);
						PlayerPrefs.SetString(StringConstants.PlayerCustomAvatarCodePrefKey, modelExportedEvent.avatarCode);
						PlayerPrefs.SetString(StringConstants.PlayerCustomAvatarLinkPrefKey, modelExportedEvent.url);
						SceneManager.LoadScene(StringConstants.LauncherSceneName);
					}
					else if (metaPersonEditorEvent.eventName == "action_availability_changed")
					{
						ActionAvailabilityEvent actionAvailabilityEvent = JsonUtility.FromJson<ActionAvailabilityEvent>(args.Value);
						if (actionAvailabilityEvent.actionName == "avatar_export")
						{
							exportButton.interactable = actionAvailabilityEvent.isAvailable;
						}
					}
				}
			}
			catch (Exception exc)
			{
				Debug.LogErrorFormat("Unable to parse message: {0}. Exception: {1}", args.Value, exc);
				errorText.text = exc.Message;
				webViewPlaceholder.SetActive(false);
			}
		}
	}
#else
	public class AvatarSelectionSceneHandler : MonoBehaviour
	{
		public string clientId;

		public string clientSecret;

		public GameObject webViewPlaceholder;

		public Text errorText;

		public Button exportButton;

		private void Start()
		{
			errorText.text = "This scene requires Vuplex WebView for Windows and macOS: https://store.vuplex.com/webview/windows-mac";
		}

		public void OnBackButtonClick()
		{
			SceneManager.LoadScene(StringConstants.LauncherSceneName);
		}

		public void OnExportButtonClick()
		{
			
		}
	}
#endif
}
