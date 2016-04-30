/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;

#if !UNITY_WEBGL
using System.Threading;
#endif

/// <summary>
/// This class manages the background threads.\n
/// <strong>Please do not use it if you do not know what you're doing.</strong>
/// </summary>
public static class OnlineMapsThreadManager
{
#if !UNITY_WEBGL
    private static Thread thread;
    private static List<Action> threadActions;
#endif

    /// <summary>
    /// Adds action queued for execution in a separate thread.
    /// </summary>
    /// <param name="action">Action to be executed.</param>
    public static void AddThreadAction(Action action)
    {
#if !UNITY_WEBGL
        if (threadActions == null) threadActions = new List<Action>();

        lock (threadActions)
        {
            threadActions.Add(action);
        }

        if (thread == null)
        {
            thread = new Thread(StartNextAction);
            thread.Start();
        }
#else
        throw new Exception("AddThreadAction not supported for WebGL.");
#endif
    }

    /// <summary>
    /// Disposes of thread manager.
    /// </summary>
    public static void Dispose()
    {
#if !UNITY_WEBGL
        if (thread != null)
        {
#if UNITY_IOS
            thread.Interrupt();
#else
            thread.Abort();
#endif
        }
        thread = null;
        threadActions = null;
#endif
    }

    private static void StartNextAction()
    {
#if !UNITY_WEBGL
        while (true)
        {
            bool actionInvoked = false;
            lock (threadActions)
            {
                if (threadActions.Count > 0)
                {
                    Action action = threadActions[0];
                    threadActions.RemoveAt(0);
                    action();
                    actionInvoked = true;
                }
            }
            if (!actionInvoked) Thread.Sleep(1);
        }
#endif
    }
}