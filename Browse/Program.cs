using System.Windows.Forms;

namespace Browse
{
  internal static class Program
  {
    public static void Go(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new frmMain());
    }
  }
}