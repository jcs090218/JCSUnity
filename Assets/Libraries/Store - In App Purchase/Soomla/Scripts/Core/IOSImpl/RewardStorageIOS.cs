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
using System.Runtime.InteropServices;

namespace Soomla {
	
	public class RewardStorageIOS : RewardStorage {

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern long rewardStorage_GetLastGivenTimeMillis(string rewardId);
		[DllImport ("__Internal")]
		private static extern int rewardStorage_GetTimesGiven(string rewardId);
		[DllImport ("__Internal")]
		private static extern void rewardStorage_SetTimesGiven(string rewardId, 
		                                                             [MarshalAs(UnmanagedType.Bool)] bool up, 
		                                                             [MarshalAs(UnmanagedType.Bool)] bool notify);
		[DllImport ("__Internal")]
		private static extern int rewardStorage_GetLastSeqIdxGiven(string rewardId);
		[DllImport ("__Internal")]
		private static extern void rewardStorage_SetLastSeqIdxGiven(string rewardId, int idx);
		
		/// <summary>
		/// Get last id of given reward from a <c>SequenceReward</c>
		/// </summary>
		/// <param name="reward">to query</param>
		/// <returns>last index of sequence reward given</returns>
		override protected int _getLastSeqIdxGiven(SequenceReward seqReward) {
			return rewardStorage_GetLastSeqIdxGiven(seqReward.ID);
		}
		
		/// <summary>
		/// Set last id of given reward from a <c>SequenceReward</c>
		/// </summary>
		/// <param name="reward">to set last id for</param>
		/// <param name="reward">the last id to to mark as given</param>
		override protected void _setLastSeqIdxGiven(SequenceReward seqReward, int idx) {
			rewardStorage_SetLastSeqIdxGiven(seqReward.ID, idx);
		}

		override protected void _setTimesGiven(Reward reward, bool up, bool notify) {
			rewardStorage_SetTimesGiven(reward.ID, up, notify);
		}
		
		override protected int _getTimesGiven(Reward reward) {
			int times = rewardStorage_GetTimesGiven(reward.ID);
			SoomlaUtils.LogDebug("SOOMLA/UNITY RewardStorageIOS", string.Format("reward {0} given={1}", reward.ID, times));
			return times;
		}

		override protected DateTime _getLastGivenTime(Reward reward) {
			long lastTime = rewardStorage_GetLastGivenTimeMillis(reward.ID);
			TimeSpan time = TimeSpan.FromMilliseconds(lastTime);
			return new DateTime(time.Ticks);
		}
#endif
	}
}