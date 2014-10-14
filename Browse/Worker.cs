﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Browse
{
  /// <summary>
  /// Handles the work of opening up the pages in a separate thread
  /// </summary>
  public class Worker
  {
    private Browser _browser;
    private string[] _pages; 
    private string _root; 
    private int _pauseLen;
    private bool _pausefirst;
    private Thread thread;

    /// <summary>
    /// Creates a worker object and sets everything up
    /// </summary>
    /// <param name="browser">The browser to use</param>
    /// <param name="pages">The page templates to use</param>
    /// <param name="root">The variable to replace {root} in the templates with</param>
    /// <param name="pauseLen">The amount of time to pause</param>
    /// <param name="pausefirst">If true, it will wait for the user to continue</param>
    public Worker(Browser browser, string[] pages, string root, int pauseLen, bool pausefirst)
    {
      _browser = browser;
      _pages = pages;
      _root = root;
      _pauseLen = pauseLen;
      _pausefirst = pausefirst;

      thread = new Thread(DoIt);
    }
    
    /// <summary>
    /// Starts the process/Thread
    /// </summary>
    public void Go()
    {
      thread.Start();
    }

    /// <summary>
    /// Aborts the process/thread
    /// </summary>
    public void Abort()
    {
      thread.Abort();
    }

    private void DoIt()
    {
      if (_pages.Count() > 0)
      {
        for (int i = 0; i < _pages.Count(); i++)
        {
          string page = _pages[i].Replace("{root}", _root);
          Process.Start(_browser.Command, page);

          if (i == 0)
          {
            if (_pausefirst)
              MessageBox.Show("Press Log In to the web site then press OK to continue", "Paused", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else  // We're going to assume we don't need to pause because of the Login.
              Thread.Sleep(_pauseLen);
          }
          else
          {
            Thread.Sleep(_pauseLen);
          }
        }

        //TODO: Create a delegate to call back to once finished
      }
      else
        MessageBox.Show("Nothing to do!");
    }
  }
}