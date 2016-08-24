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
	
	public class KeyValueStorageWP : KeyValueStorage {

#if UNITY_WP8 && !UNITY_EDITOR

		override protected string _getValue(string key) {
			string val = null;
            val = SoomlaWpCore.data.KeyValueStorage.GetValue(key);
			return val;
		}
		
		override protected void _setValue(string key, string val) {
            SoomlaWpCore.data.KeyValueStorage.SetValue(key,val);
		}
		
		override protected void _deleteKeyValue(string key) {
			SoomlaWpCore.data.KeyValueStorage.DeleteKeyValue(key);
		}

        public List<string> _getEncryptedKeys() {
            // TODO: Implement me
			return new List<string>();
		}

#endif
	}
}
