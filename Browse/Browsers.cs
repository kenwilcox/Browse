using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace Browse
{
  public class Browsers: List<Browser>
  {
    private int defaultIndex;
    private string defaultName;
    private string defaultPath;

    public Browsers()
    {
      RegistryKey defaultKey = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command");
      defaultPath = (string)defaultKey.GetValue(null);
      defaultPath = Path.GetFullPath(defaultPath.Replace("\"", ""));
      
      RegistryKey browserKeys;
      browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
      if (browserKeys == null)
        browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");

      string[] browserNames = browserKeys.GetSubKeyNames();
      for (int i = 0; i < browserNames.Length; i++)
      {
        Browser browser = new Browser();
        RegistryKey browserKey = browserKeys.OpenSubKey(browserNames[i]);
        browser.Name = (string)browserKey.GetValue(null);
        RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
        browser.Command = (string)browserKeyPath.GetValue(null);
        browser.Command = browser.Command.Replace("\"", "");
        RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
        browser.DefaultIcon = (string)browserIconPath.GetValue(null);

        if (defaultPath.StartsWith(browser.Command))
        {
          browser.Default = true;
          defaultIndex = i;
          defaultName = browser.Name;
          defaultPath = browser.Command;
        }
        
        this.Add(browser);
      }
    }

    public int DefaultIndex
    {
      get { return this.defaultIndex; }
    }

    public string DefaultName
    {
      get { return this.defaultName; }
    }

    public string DefaultPath
    {
      get { return this.defaultPath; }
    }
  }
}
