using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Browse
{
  /// <summary>
  ///   Handles the work of opening up the pages in a separate thread
  /// </summary>
  public class Worker
  {
    private readonly Browser _browser;
    private readonly INotifier _notifier;
    private readonly string[] _pages;
    private readonly bool _pausefirst;
    private readonly int _pauseLen;
    private readonly string _root;
    private readonly Thread _thread;

    /// <summary>
    ///   Creates a worker object and sets everything up
    /// </summary>
    /// <param name="notifier">Object to send notifications to</param>
    /// <param name="browser">The browser to use</param>
    /// <param name="pages">The page templates to use</param>
    /// <param name="root">The variable to replace {root} in the templates with</param>
    /// <param name="pauseLen">The amount of time to pause</param>
    /// <param name="pausefirst">If true, it will wait for the user to continue</param>
    public Worker(INotifier notifier, Browser browser, string[] pages, string root, int pauseLen, bool pausefirst)
    {
      _browser = browser;
      _pages = pages;
      _root = root;
      _pauseLen = pauseLen;
      _pausefirst = pausefirst;
      _notifier = notifier;

      _thread = new Thread(DoIt);
    }

    public EventHandler ThreadDone { get; set; }

    /// <summary>
    ///   Starts the process/Thread
    /// </summary>
    public void Go()
    {
      _thread.Start();
    }

    /// <summary>
    ///   Aborts the process/thread
    /// </summary>
    public void Abort()
    {
      _thread.Abort();
    }

    private void DoIt()
    {
      if (_pages.Count() > 0)
      {
        for (var i = 0; i < _pages.Count(); i++)
        {
          var page = _pages[i].Replace("{root}", _root);
          Process.Start(_browser.Command, page);

          if (i == 0)
          {
            if (_pausefirst)
              if (
                _notifier.ShowMessage("Please Log In to the web site\r\nThen press OK to continue", MessageType.Normal) ==
                false)
                break;
            //else  // We're going to assume we don't need to pause because of the Login.
            Thread.Sleep(_pauseLen);
          }
          else
          {
            Thread.Sleep(_pauseLen);
          }
        }

        if (ThreadDone != null)
          ThreadDone(this, EventArgs.Empty);
      }
      else
        _notifier.ShowMessage("Nothing to do!", MessageType.Error);
    }
  }
}
