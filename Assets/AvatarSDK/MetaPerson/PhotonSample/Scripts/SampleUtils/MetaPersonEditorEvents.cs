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

namespace AvatarSDK.MetaPerson.Photon
{
	[Serializable]
	public class MetaPersonEditorEvent
	{
		public string eventName;
		public string source;
	}

	[Serializable]
	public class ModelExportedEvent : MetaPersonEditorEvent
	{
		public string url;
		public string gender;
		public string avatarCode;
	}

	[Serializable]
	public class ActionAvailabilityEvent : MetaPersonEditorEvent
	{
		public string actionName;
		public bool isAvailable;
	}
}
