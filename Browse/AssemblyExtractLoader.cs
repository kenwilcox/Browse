using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Browse
{
  public class AssemblyExtractLoader
  {
    // set this to the directory name for you assemblies (include the dots)
    // if you don't have one (just in the main bundle) set this to blank
    private const string Libdir = ".Libs.";
    private static readonly Dictionary<string, Assembly> Libs = new Dictionary<string, Assembly>();

    /// <summary>
    ///   The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
      AppDomain.CurrentDomain.AssemblyResolve += FindAssembly;
      Program.Go(args);
    }

    private static Assembly FindAssembly(object sender, ResolveEventArgs args)
    {
      var shortName = new AssemblyName(args.Name).Name;
      if (Libs.ContainsKey(shortName)) return Libs[shortName];
      var strNameSpace = Assembly.GetExecutingAssembly().GetName().Name;

      using (
        var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(strNameSpace + Libdir + shortName + ".dll"))
      {
        if (s == null) return null;
        var data = new BinaryReader(s).ReadBytes((int) s.Length);
        var a = Assembly.Load(data);
        Libs[shortName] = a;
        return a;
      }
    }
  }
}
