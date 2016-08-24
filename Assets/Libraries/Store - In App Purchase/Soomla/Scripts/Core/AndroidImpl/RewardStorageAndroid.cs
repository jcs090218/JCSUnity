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

namespace Soomla {
	
	public class RewardStorageAndroid : RewardStorage {

#if UNITY_ANDROID && !UNITY_EDITOR
		
		override protected int _getLastSeqIdxGiven(SequenceReward reward) {
			int idx = -1;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.RewardStorage")) {
				idx = jniRewardStorage.CallStatic<int>("getLastSeqIdxGiven", reward.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return idx;
		}
		
		override protected void _setLastSeqIdxGiven(SequenceReward reward, int idx) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.RewardStorage")) {
				jniRewardStorage.CallStatic("setLastSeqIdxGiven", reward.ID, idx);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		override protected void _setTimesGiven(Reward reward, bool up, bool notify) {

			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.RewardStorage")) {
				jniRewardStorage.CallStatic("setTimesGiven", reward.ID, up, notify);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		override protected int _getTimesGiven(Reward reward) {
			int times = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.RewardStorage")) {
				times = jniRewardStorage.CallStatic<int>("getTimesGiven", reward.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return times;
		}
		
		override protected DateTime _getLastGivenTime(Reward reward) {
			long lastTime = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.RewardStorage")) {
				lastTime = jniRewardStorage.CallStatic<long>("getLastGivenTimeMillis", reward.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);

			TimeSpan time = TimeSpan.FromMilliseconds(lastTime);
			return new DateTime(time.Ticks);
		}


#endif
	}
}
