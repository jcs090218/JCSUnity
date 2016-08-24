//  Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
// This software is provided 'as-is', without any express or implied warranty. In
// no event will the authors be held liable for any damages arising from the use
// of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim
// that you wrote the original software. If you use this software in a product,
// an acknowledgment in the product documentation would be appreciated but is not
// required.
//
// 2. Altered source versions must be plainly marked as such, and must not be
// misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.

using UnityEngine;
using System.Collections.Generic;
using System;
[System.Serializable]
public sealed class ObjectKvp : UnityNameValuePair<string> {
	public string value = null;

	override public string Value {
		get { return this.value; }
		set { this.value = value; }
	}

	public ObjectKvp(string key, string value) : base(key, value) {
	}
}

[System.Serializable]
public class ObjectDictionary : UnityDictionary<string> {
	public List<ObjectKvp> values;

	override protected List<UnityKeyValuePair<string, string>> KeyValuePairs {
		get {
			if (values == null) {
				values = new List<ObjectKvp>();
			}

            List<UnityKeyValuePair<string, string>> valuesConverted = new List<UnityKeyValuePair<string, string>>();
            foreach (ObjectKvp okvp in values)
            {
                valuesConverted.Add(ConvertOkvp(okvp));
            }

            return valuesConverted;
		}
		set {
			if (value == null) {
				values = new List<ObjectKvp>();
				return;
			}

            foreach(UnityKeyValuePair<string,string> ukvp in value)
            {
                values.Add(ConvertUkvp(ukvp));
            }
		}
	}

    public new ObjectKvp ConvertUkvp(UnityKeyValuePair<string,string> ukvp)
    {
        return new ObjectKvp(ukvp.Key, ukvp.Value);

    }
    public UnityKeyValuePair<string, string> ConvertOkvp(ObjectKvp okvp)
    {
        return new UnityKeyValuePair<string, string>(okvp.Key,okvp.Value);
    }
	override protected void SetKeyValuePair(string k, string v) {
		var index = values.FindIndex(x => {
			return x.Key == k;});

		if (index != -1) {
			if (v == null) {
				values.RemoveAt(index);
				return;
			}

			values[index] = new ObjectKvp(k, v);
			return;
		}

		values.Add(new ObjectKvp(k, v));
	}
}
