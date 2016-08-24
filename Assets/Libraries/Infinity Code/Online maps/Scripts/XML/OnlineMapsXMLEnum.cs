/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections;

public class OnlineMapsXMLEnum : IEnumerator
{
    private readonly OnlineMapsXML el;
    private int position = -1;

    public OnlineMapsXMLEnum(OnlineMapsXML el)
    {
        this.el = el;
    }

    public bool MoveNext()
    {
        position++;
        return position < el.count;
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
                return el[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}