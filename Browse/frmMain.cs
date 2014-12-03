using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CC.Common.JSON;

namespace Browse
{
  public partial class frmMain : Form
  {
    private Browsers _browsers;
    private Browser _browser;
    private Worker _worker;
    private CCPreferences _prefs;

    public frmMain()
    {
      InitializeComponent();
    }

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
      StartWorker();
    }

    private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      StopWorker();
      SavePreferences();
    }

    private void DoLoad()
    {
      _browsers = new Browsers();
      cboBrowsers.DataSource = _browsers;
      cboBrowsers.SelectedIndex = _browsers.DefaultIndex;

      LoadPreferences();
    }

    private void SelectBrowser()
    {
      _browser = (Browser)cboBrowsers.SelectedItem;
      Icon ico = IconExtractor.ExtractIconFromExe(_browser.DefaultIcon, true);
      if (ico != null)
      {
        btnOpen.Image = ico.ToBitmap();
        btnOpen.Text = "";
      }
    }

    private void StartWorker()
    {
      string[] pages = txtPages.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      int pause = (int)udPause.Value;
      _worker = new Worker(_browser, pages, txtRoot.Text, pause, cbPause.Checked);
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
    }

    private void SavePreferences()
    {
      _prefs.Set("root", txtRoot.Text);
      _prefs.Set("pause", cbPause.Checked);
      _prefs.Set("pauseTime", udPause.Value);

      string[] lines = txtPages.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
      ArrayList list = new ArrayList(lines);
      _prefs.Set("pages", list);

      _prefs.Save();
    }
  }
}
