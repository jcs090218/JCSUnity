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
using System.Collections.Generic;

namespace Soomla
{
	public class SoomlaPostBuildTools {
	#if UNITY_EDITOR
		private static Dictionary<string, ISoomlaPostBuildTool> Tools = new Dictionary<string, ISoomlaPostBuildTool>();

		public static void AddTool(string module, ISoomlaPostBuildTool tool) {
			string key = FindToolKey(module);
			if (key == null) {
				Tools.Add(module, tool);
			}
			else {
				Tools[key] = tool;
			}
		}

		public static ISoomlaPostBuildTool GetTool (string targetModule) {
			string key = FindToolKey(targetModule);
			if (key == null) {
				return null;
			}
			else {
				return Tools[key];
			}
		}

		private static string FindToolKey(string targetModule) {
			foreach (var entry in Tools) {
				if (targetModule.ToLower().StartsWith(entry.Key.ToLower())) {
					return entry.Key;
				}
			}

			return null;
		}
	#endif
	}

}