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
using System;

namespace Soomla {

	/// <summary>
	/// This class provides Log functions that output debug and error messages.
	/// </summary>
	public static class SoomlaUtils {

        private static bool isDebugBuild = false;
        private static bool isDebugBuildSet = false;
        /// <summary>
		/// Creates Log Debug message according to given tag and message.
		/// </summary>
		/// <param name="tag">The name of the class whose instance called this function.</param>
		/// <param name="message">Debug message to output to log.</param>
		public static void LogDebug(string tag, string message)
		{
            if (!isDebugBuildSet)
            {
                try //Debug.isDebugBuild can fail on WP8 if it is not called from the Main Thread
                {
                    isDebugBuild = Debug.isDebugBuild;
                }
                catch (Exception e)
                {
                    isDebugBuild = true;
                    Debug.Log(string.Format("{0} {1}", tag, e.Message));
                }
                isDebugBuildSet = true;
            }
            if (isDebugBuild && CoreSettings.DebugUnityMessages)
            {
				Debug.Log(string.Format("{0} {1}", tag, message));
			}
		}

		/// <summary>
		/// Creates Log Error message according to given tag and message.
		/// </summary>
		/// <param name="tag">The name of the class whose instance called this function..</param>
		/// <param name="message">Error message to output to log.</param>
		public static void LogError(string tag, string message) {
			Debug.LogError(string.Format("{0} {1}", tag, message));
		}

		public static void LogWarning(string tag, string message) {
			Debug.LogWarning(string.Format("{0} {1}", tag, message));
		}

		/// <summary>
		/// Returns the class name to be used in serialization/deserialization process
		/// in Soomla
		/// </summary>
		/// <param name="target">The target to get class name for</param>
		/// <returns>The class name of the provided instance</returns>
		public static string GetClassName(object target) {
			return target.GetType().Name;
		}
	}
}

