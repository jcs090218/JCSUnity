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

namespace Soomla
{
	public class RewardStorage
	{

		protected const string TAG = "SOOMLA RewardStorage"; // used for Log error messages

		static RewardStorage _instance = null;
		static RewardStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new RewardStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new RewardStorageIOS();
                    #elif UNITY_WP8 && !UNITY_EDITOR
					_instance = new RewardStorageWP();
                    #else
                    _instance = new RewardStorage();
					#endif
				}
				return _instance;
			}
		}
			

		public static void SetRewardStatus(Reward reward, bool give) {
			SetRewardStatus(reward, give, true);
		}

		public static void SetRewardStatus(Reward reward, bool give, bool notify) {
			instance._setTimesGiven(reward, give, notify);
		}

		public static bool IsRewardGiven(Reward reward) {
			return GetTimesGiven(reward) > 0;
		}

		public static int GetTimesGiven(Reward reward) {
			return instance._getTimesGiven(reward);
		}

		public static DateTime GetLastGivenTime(Reward reward) {
			return instance._getLastGivenTime(reward);
		}

		/// <summary>
		/// Retrieves the index of the last reward given in a sequence of rewards.
		/// </summary>
		public static int GetLastSeqIdxGiven(SequenceReward reward) {
			return instance._getLastSeqIdxGiven(reward);
		}

		public static void SetLastSeqIdxGiven(SequenceReward reward, int idx) {
			instance._setLastSeqIdxGiven(reward, idx);
		}

		virtual protected int _getLastSeqIdxGiven(SequenceReward seqReward) {
#if UNITY_EDITOR
			string key = keyRewardIdxSeqGiven(seqReward.ID);
			string val = PlayerPrefs.GetString (key);
			if (string.IsNullOrEmpty(val)) {
				return -1;
			}
			return int.Parse(val);
#else
			return 0;
#endif
		}

		virtual protected void _setLastSeqIdxGiven(SequenceReward seqReward, int idx) {
#if UNITY_EDITOR
			string key = keyRewardIdxSeqGiven(seqReward.ID);
			PlayerPrefs.SetString (key, idx.ToString());
#endif
		}

		virtual protected void _setTimesGiven(Reward reward, bool up, bool notify) {
#if UNITY_EDITOR
			int total = _getTimesGiven(reward) + (up ? 1 : -1);
			if(total<0) total = 0;

			string key = keyRewardTimesGiven(reward.ID);
			PlayerPrefs.SetString(key, total.ToString());

			if (up) {
				key = keyRewardLastGiven(reward.ID);
				PlayerPrefs.SetString(key, (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString());
			}

			if (notify) {
				if (up) {
					CoreEvents.OnRewardGiven(reward);
					//CoreEvents.OnRewardGiven(new RewardGivenEvent(reward));
				} else {
					CoreEvents.OnRewardTaken(reward);
					//CoreEvents.OnRewardTaken(new RewardTakenEvent(reward));
				}
			}
#endif
		}

		virtual protected int _getTimesGiven(Reward reward) {
#if UNITY_EDITOR
			string key = keyRewardTimesGiven(reward.ID);
			string val = PlayerPrefs.GetString (key);
			if (string.IsNullOrEmpty(val)) {
				return 0;
			}
			return int.Parse(val);
#else
			return 0;
#endif
		}

		virtual protected DateTime _getLastGivenTime(Reward reward) {
#if UNITY_EDITOR
			string key = keyRewardLastGiven(reward.ID);
			string val = PlayerPrefs.GetString (key);
			if (string.IsNullOrEmpty(val)) {
				return default(DateTime);
			}
			long timeMillis = Convert.ToInt64(val);
			TimeSpan time = TimeSpan.FromMilliseconds(timeMillis);
			return new DateTime(time.Ticks);
#else
			return default(DateTime);
#endif
		}

		/** keys **/
#if UNITY_EDITOR
		private static string keyRewards(string rewardId, string postfix) {
			return CoreSettings.DB_KEY_PREFIX + "rewards." + rewardId + "." + postfix;
		}
		
		private static string keyRewardIdxSeqGiven(string rewardId) {
			return keyRewards(rewardId, "seq.idx");
		}

		private static string keyRewardTimesGiven(string rewardId) {
			return keyRewards(rewardId, "timesGiven");
		}

		private static string keyRewardLastGiven(string rewardId) {
			return keyRewards(rewardId, "lastGiven");
		}
#endif
	}
}

