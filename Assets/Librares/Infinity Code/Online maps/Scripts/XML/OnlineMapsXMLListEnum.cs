/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections;

public class OnlineMapsXMLListEnum : IEnumerator
{
    private readonly OnlineMapsXMLList list;
    private int position = -1;

    public OnlineMapsXMLListEnum(OnlineMapsXMLList list)
    {
        this.list = list;
    }

    public bool MoveNext()
    {
        position++;
        return position < list.count;
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public OnlineMapsXML Current
    {
        get
        {
            try
            {
                return list[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}