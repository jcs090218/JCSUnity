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
using System.Runtime.InteropServices;
using System;

namespace Soomla {

	/// <summary>
	/// This is the parent class of all entities in the application.
	/// Almost every entity in your virtual economy will be an entity. There are many types
	/// of entities, each one will extend this class. Each one of the various types extends
	/// <c>SoomlaEntity</c> and adds its own behavior to it.
	/// </summary>
	public abstract class SoomlaEntity<T> {

		private const string TAG = "SOOMLA SoomlaEntity";
		
		public string Name;
		public string Description;
		protected string _id;
		public string ID {
			get { return _id; }
		}

		protected SoomlaEntity (string id) 
			: this(id, "", "")
		{
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="id">unique id.</param>
		protected SoomlaEntity (string id, string name, string description)
		{
			this.Name = name;
			this.Description = description;
			this._id = id;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		protected SoomlaEntity(AndroidJavaObject jniSoomlaEntity) {
			this.Name = jniSoomlaEntity.Call<string>("getName");
			this.Description = jniSoomlaEntity.Call<string>("getDescription");
			this._id = jniSoomlaEntity.Call<string>("getID");
		}
#endif

#if UNITY_WP8 && !UNITY_EDITOR
        protected SoomlaEntity(SoomlaWpCore.SoomlaEntity<T> wpSoomlaEntity)
        {
            this.Name = wpSoomlaEntity.GetName();
            this.Description = wpSoomlaEntity.GetDescription();
            this._id = wpSoomlaEntity.GetId();
		}
#endif
        /// <summary>
		/// Constructor.
		/// Generates an instance of <c>SoomlaEntity</c> from the given <c>JSONObject</c>.
		/// </summary>
		/// <param name="jsonEntity">A JSONObject representation of the wanted <c>SoomlaEntity</c>.</param>
		protected SoomlaEntity(JSONObject jsonEntity) {
			if (jsonEntity[JSONConsts.SOOM_ENTITY_ID] == null) {
				SoomlaUtils.LogError(TAG, "This is BAD! We don't have ID in the given JSONObject. Stopping here. JSON: " + jsonEntity.print());
				return;
			}

			if (jsonEntity[JSONConsts.SOOM_ENTITY_NAME]) {
				this.Name = jsonEntity[JSONConsts.SOOM_ENTITY_NAME].str;
			} else {
				this.Name = "";
			}
			if (jsonEntity[JSONConsts.SOOM_ENTITY_DESCRIPTION]) {
				this.Description = jsonEntity[JSONConsts.SOOM_ENTITY_DESCRIPTION].str;
			} else {
				this.Description = "";
			}
			this._id = jsonEntity[JSONConsts.SOOM_ENTITY_ID].str;
		}
		
		/// <summary>
		/// Converts the current <c>SoomlaEntity</c> to a JSONObject.
		/// </summary>
		/// <returns>A <c>JSONObject</c> representation of the current <c>SoomlaEntity</c>.</returns>
		public virtual JSONObject toJSONObject() {
			if (string.IsNullOrEmpty(this._id)) {
				SoomlaUtils.LogError(TAG, "This is BAD! We don't have ID in the this SoomlaEntity. Stopping here.");
				return null;
			}

			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.SOOM_ENTITY_NAME, this.Name);
			obj.AddField(JSONConsts.SOOM_ENTITY_DESCRIPTION, this.Description);
			obj.AddField(JSONConsts.SOOM_ENTITY_ID, this._id);
			obj.AddField(JSONConsts.SOOM_CLASSNAME, SoomlaUtils.GetClassName(this));
			
			return obj;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		protected static bool isInstanceOf(AndroidJavaObject jniEntity, string classJniStr) {
			System.IntPtr cls = AndroidJNI.FindClass(classJniStr);
			return AndroidJNI.IsInstanceOf(jniEntity.GetRawObject(), cls);
		}
#endif


		// Equality
		
		public override bool Equals(System.Object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}
			
			// If parameter cannot be cast to Point return false.
			SoomlaEntity<T> g = obj as SoomlaEntity<T>;
			if ((System.Object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (_id == g._id);
		}
		
		public bool Equals(SoomlaEntity<T> g)
		{
			// If parameter is null return false:
			if ((object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (_id == g._id);
		}
		
		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}
		
		public static bool operator ==(SoomlaEntity<T> a, SoomlaEntity<T> b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}
			
			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}
			
			// Return true if the fields match:
			return a._id == b._id;
		}
		
		public static bool operator !=(SoomlaEntity<T> a, SoomlaEntity<T> b)
		{
			return !(a == b);
		}

		public virtual T Clone(string newId) {
			JSONObject obj = this.toJSONObject();
			obj.SetField(JSONConsts.SOOM_ENTITY_ID, JSONObject.CreateStringObject(newId));
			return (T) Activator.CreateInstance(this.GetType(), new object[] { obj });
		}
	}
}

