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
using System.Runtime.InteropServices;
using UnityEngine;

namespace Soomla {
	
	public class KeyValueStorageIOS : KeyValueStorage {

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern void keyValStorage_GetValue(string key, out IntPtr outResult);
		[DllImport ("__Internal")]
		private static extern void keyValStorage_SetValue(string key, string val);
		[DllImport ("__Internal")]
		private static extern void keyValStorage_DeleteKeyValue(string key);
		[DllImport ("__Internal")]
		private static extern void keyValStorage_GetEncryptedKeys(out IntPtr outResult);
		[DllImport ("__Internal")]
		private static extern void keyValStorage_Purge();

		override protected string _getValue(string key) {
			IntPtr p = IntPtr.Zero;
			keyValStorage_GetValue(key, out p);
			string val = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			return val;
		}
		
		override protected void _setValue(string key, string val) {
			keyValStorage_SetValue(key, val);
		}
		
		override protected void _deleteKeyValue(string key) {
			keyValStorage_DeleteKeyValue(key);
		}

		override protected List<string> _getEncryptedKeys() {
            IntPtr p = IntPtr.Zero;
            keyValStorage_GetEncryptedKeys(out p);
            string val = Marshal.PtrToStringAnsi(p);
            Marshal.FreeHGlobal(p);
            return new List<string>(val.Split(','));
		}

		override protected void _purge () {
			keyValStorage_Purge();
		}
#endif
	}
}