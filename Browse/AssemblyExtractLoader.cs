using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Browse
{
  public class AssemblyExtractLoader
  {
    private static Dictionary<string, Assembly> libs = new Dictionary<string, Assembly>();
    // set this to the directory name for you assemblies (include the dots)
    // if you don't have one (just in the main bundle) set this to blank
    private const string LIBDIR = ".Libs.";

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      AppDomain.CurrentDomain.AssemblyResolve += FindAssembly;
      Program.Go(args);
    }

    static Assembly FindAssembly(object sender, ResolveEventArgs args)
    {
      string shortName = new AssemblyName(args.Name).Name;
      if (libs.ContainsKey(shortName)) return libs[shortName];
      string strNameSpace = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();

      using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(strNameSpace + LIBDIR + shortName + ".dll"))
      {
        byte[] data = new BinaryReader(s).ReadBytes((int)s.Length);
        Assembly a = Assembly.Load(data);
        libs[shortName] = a;
        return a;
      }
    }
  }
}
