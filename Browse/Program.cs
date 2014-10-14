using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Browse
{
  static class Program
  {
    public static void Go(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new frmMain());
    }
  }
}
