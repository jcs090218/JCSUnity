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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Soomla {
	
	public class KeyValueStorageAndroid : KeyValueStorage {

#if UNITY_ANDROID && !UNITY_EDITOR

		override protected string _getValue(string key) {
			string val = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.KeyValueStorage")) {
				val = jniRewardStorage.CallStatic<string>("getValue", key);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return val;
		}
		
		override protected void _setValue(string key, string val) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.KeyValueStorage")) {
				jniRewardStorage.CallStatic("setValue", key, val);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		override protected void _deleteKeyValue(string key) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.KeyValueStorage")) {
				jniRewardStorage.CallStatic("deleteKeyValue", key);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		override protected List<string> _getEncryptedKeys() {
            string val = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.core.unity.SoomlaBridge")) {
                val = jniRewardStorage.CallStatic<string>("keyValStorage_GetEncryptedKeys");
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
            return new List<string>(val.Split(','));
		}

		override protected void _purge() {
			AndroidJNI.PushLocalFrame(100);
			using (AndroidJavaClass jniRewardStorage = new AndroidJavaClass("com.soomla.data.KeyValueStorage")) {
				jniRewardStorage.CallStatic("purge");
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
#endif
	}
}
