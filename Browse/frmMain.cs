using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Browse.Properties;
using CC.Common.JSON;

namespace Browse
{
  public partial class frmMain : Form, INotifier
  {
    private Browser _browser;
    private Browsers _browsers;
    private int _executionCount;
    private CCPreferences _prefs;
    private bool _showDialog;
    private Worker _worker;

    public frmMain()
    {
      InitializeComponent();
    }

    #region INotifier Members

    public bool ShowMessage(string message, MessageType type)
    {
      //MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      _showDialog = true;
      Invoke((MethodInvoker) delegate
      {
        switch (type)
        {
          case MessageType.Normal:
            pnlMsg.BackColor = SystemColors.Highlight;
            break;
          case MessageType.Error:
            pnlMsg.BackColor = Color.Red;
            break;
        }
        lblMessage.Text = message;
        pnlMsg.Visible = true;
      });

      while (_showDialog)
        Thread.Sleep(100);
      return true;
    }

    #endregion

    private void frmMain_Load(object sender, EventArgs e)
    {
      DoLoad();
    }

    private void cboBrowsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      SelectBrowser();
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
      _executionCount = (int) udRepeat.Value;
      StartWorker();
    }

    private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      StopWorker();
      SavePreferences();
    }

    private void DoLoad()
    {
      Icon = Resources.moon;

      _browsers = new Browsers();
      cboBrowsers.DataSource = _browsers;
      cboBrowsers.SelectedIndex = _browsers.DefaultIndex;

      LoadPreferences();
    }

    private void SelectBrowser()
    {
      _browser = (Browser) cboBrowsers.SelectedItem;
      var ico = IconExtractor.ExtractIconFromExe(_browser.DefaultIcon, true);
      if (ico != null)
      {
        btnOpen.Image = ico.ToBitmap();
        btnOpen.Text = "";
      }
    }

    private void StartWorker()
    {
      var pages = txtPages.Text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
      var pause = (int) udPause.Value;
      _worker = new Worker(this, _browser, pages, txtRoot.Text, pause, cbPause.Checked);
      _worker.ThreadDone += ThreadDone;
      _worker.Go();
    }

    private void StopWorker()
    {
      if (_worker != null)
        _worker.Abort();
    }

    private void LoadPreferences()
    {
      _prefs = new CCPreferences();
      _prefs.Load();

      txtRoot.Text = _prefs.Get("root", "");
      cbPause.Checked = _prefs.Get("pause", false);
      udPause.Value = _prefs.Get("pauseTime", 1000);
      cbRepeat.Checked = _prefs.Get("repeat", false);
      udRepeat.Value = _prefs.Get("repeatTimes", 10);
      Opacity = _prefs.Get("opacity", Opacity);
      TopMost = _prefs.Get("topMost", TopMost);

      var list = new ArrayList();
      var text = String.Empty;
      list = (ArrayList) _prefs.Get("pages", list);
      if (list != null)
      {
        foreach (string line in list)
        {
          text += line + Environment.NewLine;
        }
      }
      txtPages.Text = text;
    }

    private void SavePreferences()
    {
      _prefs.Set("root", txtRoot.Text);
      _prefs.Set("pause", cbPause.Checked);
      _prefs.Set("pauseTime", udPause.Value);
      _prefs.Set("repeat", cbRepeat.Checked);
      _prefs.Set("repeatTimes", udRepeat.Value);

      var lines = txtPages.Text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
      if (lines.Length > 0)
      {
        var list = new ArrayList(lines);
        _prefs.Set("pages", list);
      }
      else
      {
        _prefs.Set("pages", new ArrayList(0));
      }
      _prefs.Save();
    }

    private void ThreadDone(object sender, EventArgs e)
    {
      if (cbRepeat.Checked)
      {
        _executionCount--;
        if (_executionCount >= 1)
          StartWorker();
      }
    }

    private void btnAccept_Click(object sender, EventArgs e)
    {
      pnlMsg.Visible = false;
      _showDialog = false;
    }
  }
}