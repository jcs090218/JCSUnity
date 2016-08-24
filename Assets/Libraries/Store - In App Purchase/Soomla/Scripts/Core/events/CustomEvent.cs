using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla
{
	public class CustomEvent : SoomlaEvent
	{
		private string Name;
		private Dictionary<String, String> Extra;

		public CustomEvent(String name, Dictionary<String, String> extra) : this(name, extra, null)
		{
            
		}

		public CustomEvent (String name, Dictionary<String, String> extra, Object sender): base(sender)
		{
			this.Name = name;
			this.Extra = extra;
		}
	
		public String GetName ()
		{
			return this.Name;
		}

		public Dictionary<String, String> GetExtra ()
		{
			return this.Extra;
		}
	}
}
