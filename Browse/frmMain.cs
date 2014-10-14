using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Browse
{
  public partial class frmMain : Form
  {
    private Browsers browsers;
    private Browser browser;

    public frmMain()
    {
      InitializeComponent();
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
      browsers = new Browsers();
      cboBrowsers.DataSource = browsers;
      cboBrowsers.SelectedIndex = browsers.DefaultIndex;
    }

    private void cboBrowsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      browser = (Browser)cboBrowsers.SelectedItem;
      Icon ico = IconExtractor.ExtractIconFromExe(browser.DefaultIcon, true);
      if (ico != null)
      {
        btnOpen.Image = ico.ToBitmap();
        btnOpen.Text = "";
      }
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
      string[] pages = txtPages.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
      int pause = (int)udPause.Value;
      if (pages.Count() > 0)
      {
        for (int i = 0; i < pages.Count(); i++)
        {
          string page = pages[i].Replace("{root}", txtRoot.Text);
          Process.Start(browser.Command, page);

          if (i == 0)
          {
            if (cbPause.Checked)
              MessageBox.Show("Press Log In to the web site then press OK to continue", "Paused", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else  // We're going to assume we don't need to pause because of the Login.
              Thread.Sleep(pause);
          }
          else
          {
            Thread.Sleep(pause);
          }
        }
      }
      else
        MessageBox.Show("Nothing to do!");
    }
  }
}
