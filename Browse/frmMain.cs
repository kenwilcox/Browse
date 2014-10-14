using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Browse
{
  public partial class frmMain : Form
  {
    private Browsers browsers;
    private Browser browser;
    private Worker worker;

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
    }

    private void DoLoad()
    {
      browsers = new Browsers();
      cboBrowsers.DataSource = browsers;
      cboBrowsers.SelectedIndex = browsers.DefaultIndex;
    }

    private void SelectBrowser()
    {
      browser = (Browser)cboBrowsers.SelectedItem;
      Icon ico = IconExtractor.ExtractIconFromExe(browser.DefaultIcon, true);
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
      worker = new Worker(browser, pages, txtRoot.Text, pause, cbPause.Checked);
      worker.Go();
    }

    private void StopWorker()
    {
      if (worker != null)
        worker.Abort();
    }
  }
}
