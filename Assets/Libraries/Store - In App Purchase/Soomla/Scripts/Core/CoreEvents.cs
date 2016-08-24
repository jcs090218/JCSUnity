/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Soomla.Singletons;
#if UNITY_WP8 && !UNITY_EDITOR
using SoomlaWpCore.util;
using SoomlaWpCore.events;
#endif

namespace Soomla {

	/// <summary>
	/// This class provides functions for event handling.
	/// </summary>
	public class CoreEvents : CodeGeneratedSingleton {

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern void soomlaCore_Init(string secret, [MarshalAs(UnmanagedType.Bool)] bool debug);
#endif

		private const string TAG = "SOOMLA CoreEvents";

		public static CoreEvents Instance = null;		

		protected override bool DontDestroySingleton
        {
            get { return true; }
        }

		public static void Initialize() {
			if (Instance == null) {
				Instance = GetSynchronousCodeGeneratedInstance<CoreEvents>();
				SoomlaUtils.LogDebug(TAG, "Initializing CoreEvents and Soomla Core ...");
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				
				using(AndroidJavaClass jniStoreConfigClass = new AndroidJavaClass("com.soomla.SoomlaConfig")) {
					jniStoreConfigClass.SetStatic("logDebug", CoreSettings.DebugMessages);
				}
				
				// Initializing SoomlaEventHandler
				using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.core.unity.SoomlaEventHandler")) {
					jniEventHandler.CallStatic("initialize");
				}
				
				// Initializing Soomla Secret
				using(AndroidJavaClass jniSoomlaClass = new AndroidJavaClass("com.soomla.Soomla")) {
					AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
					AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
					jniSoomlaClass.CallStatic("initialize", currentActivity, CoreSettings.SoomlaSecret);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				soomlaCore_Init(CoreSettings.SoomlaSecret, CoreSettings.DebugMessages);
#elif UNITY_WP8 && !UNITY_EDITOR
				SoomlaWpCore.SoomlaConfig.logDebug = CoreSettings.DebugMessages;
				SoomlaWpCore.Soomla.initialize(CoreSettings.SoomlaSecret);
				BusProvider.Instance.Register(CoreEvents.instance);
#endif
			}
        }


#if UNITY_WP8 && !UNITY_EDITOR
        [Subscribe]
        public void onRewardGiven(RewardGivenEvent _Event)
        {
            SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onRewardGiven:" + _Event.RewardId);
			string rewardId = _Event.RewardId;
			CoreEvents.OnRewardGiven(Reward.GetReward(rewardId));
        }
        [Subscribe]
        public void onRewardTaken(RewardTakenEvent _Event) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onRewardTaken:" + _Event.RewardId);
			string rewardId = _Event.RewardId;
			CoreEvents.OnRewardTaken(Reward.GetReward(rewardId));
		}
#endif
		/// <summary>
		/// Will be called when a reward was given to the user.
		/// </summary>
		/// <param name="message">Will contain a JSON representation of a <c>Reward</c> and a flag saying if it's a Badge or not.</param>
		public void onRewardGiven(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onRewardGiven:" + message);
			
			JSONObject eventJSON = new JSONObject(message);
			string rewardId = eventJSON["rewardId"].str;

			CoreEvents.OnRewardGiven(Reward.GetReward(rewardId));
			//CoreEvents.OnRewardGiven(new RewardGivenEvent(rewardId));
		}

		/// <summary>
		/// Will be called when a reward was given to the user.
		/// </summary>
		/// <param name="message">Will contain a JSON representation of a <c>Reward</c> and a flag saying if it's a Badge or not.</param>
		public void onRewardTaken(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onRewardTaken:" + message);
			
			JSONObject eventJSON = new JSONObject(message);
			string rewardId = eventJSON["rewardId"].str;
			
			CoreEvents.OnRewardTaken(Reward.GetReward(rewardId));
			//CoreEvents.OnRewardTaken(new RewardTakenEvent(rewardId));
		}

		/// <summary>
		/// Will be called on custom events. Used for internal operations.
		/// </summary>
		public void onCustomEvent(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onCustomEvent:" + message);

			JSONObject eventJSON = new JSONObject(message);
			string name = eventJSON["name"].str;
			Dictionary<string, string> extra = eventJSON["extra"].ToDictionary();

			CoreEvents.OnCustomEvent(name, extra);
			//CoreEvents.OnCustomEvent(new CustomEvent(name, extra));
		}

		public delegate void Action();

		//public static Action<RewardGivenEvent> OnRewardGiven = delegate {};
		public static Action<Reward> OnRewardGiven = delegate {};
		//public static Action<RewardTakenEvent> OnRewardTaken = delegate {};
		public static Action<Reward> OnRewardTaken = delegate {};
		//public static Action<CustomEvent> OnCustomEvent = delegate {};
		public static Action<string, Dictionary<string, string>> OnCustomEvent = delegate {};



	}
}
