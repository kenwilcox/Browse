using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace Browse
{
  public class IconExtractor
  {
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    static extern uint ExtractIconEx(string szFileName, int nIconIndex,
       IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

    [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
    private static extern int DestroyIcon(IntPtr hIcon);

    public static Icon ExtractIconFromExe(string file, int index, bool large)
    {
      uint readIconCount = 0;
      IntPtr[] hDummy = new IntPtr[1] { IntPtr.Zero };
      IntPtr[] hIconEx = new IntPtr[1] { IntPtr.Zero };

      try
      {
        if (large)
          readIconCount = ExtractIconEx(file, index, hIconEx, hDummy, 1);
        else
          readIconCount = ExtractIconEx(file, index, hDummy, hIconEx, 1);

        if (readIconCount > 0 && hIconEx[0] != IntPtr.Zero)
        {
          // GET FIRST EXTRACTED ICON
          Icon extractedIcon = (Icon)Icon.FromHandle(hIconEx[0]).Clone();

          return extractedIcon;
        }
        else // NO ICONS READ
          return null;
      }
      catch (Exception ex)
      {
        /* EXTRACT ICON ERROR */

        // BUBBLE UP
        throw new ApplicationException("Could not extract icon", ex);
      }
      finally
      {
        // RELEASE RESOURCES
        foreach (IntPtr ptr in hIconEx)
          if (ptr != IntPtr.Zero)
            DestroyIcon(ptr);

        foreach (IntPtr ptr in hDummy)
          if (ptr != IntPtr.Zero)
            DestroyIcon(ptr);
      }
    }

    public static Icon ExtractIconFromExe(string file, bool large)
    {
      int index = 0;
      string[] parts = file.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
      if (parts.Count() == 2)
      {
        file = parts[0];
        index = Int32.Parse(parts[1]);
      }
      return ExtractIconFromExe(file, index, large);
    }
  }
}
