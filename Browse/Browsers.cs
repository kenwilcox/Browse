using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace Browse
{
  /// <summary>
  ///   Figures out what Browsers are installed on a computer, makes a list of them
  /// </summary>
  public class Browsers : List<Browser>
  {
    /// <summary>
    ///   The Only Constructor
    /// </summary>
    public Browsers()
    {
      var defaultKey = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command");
      if (defaultKey != null) DefaultPath = (string) defaultKey.GetValue(null);
      DefaultPath = Path.GetFullPath(DefaultPath.Replace("\"", ""));

      var browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet") ??
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");

      if (browserKeys == null) return;
      var browserNames = browserKeys.GetSubKeyNames();
      for (var i = 0; i < browserNames.Length; i++)
      {
        var browser = new Browser();
        var browserKey = browserKeys.OpenSubKey(browserNames[i]);
        if (browserKey != null)
        {
          browser.Name = (string) browserKey.GetValue(null);
          var browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
          if (browserKeyPath != null) browser.Command = (string) browserKeyPath.GetValue(null);
          browser.Command = browser.Command.Replace("\"", "");
          var browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
          if (browserIconPath != null) browser.DefaultIcon = (string) browserIconPath.GetValue(null);
        }

        if (DefaultPath.StartsWith(browser.Command))
        {
          browser.Default = true;
          DefaultIndex = i;
          DefaultName = browser.Name;
          DefaultPath = browser.Command;
        }

        Add(browser);
      }
    }

    /// <summary>
    ///   Returns the index in the list of the default browser
    /// </summary>
    public int DefaultIndex { get; private set; }

    /// <summary>
    ///   Returns the name of the default browser
    /// </summary>
    public string DefaultName { get; private set; }

    /// <summary>
    ///   Returns the path to the default browser
    /// </summary>
    public string DefaultPath { get; private set; }
  }
}
