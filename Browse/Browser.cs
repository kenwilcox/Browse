using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Browse
{
  public class Browser
  {
    public string Name { get; set; }
    public string DefaultIcon { get; set; }
    public string Command { get; set; }
    public bool Default { get; set; }

    public override string ToString()
    {
      return this.Name;
    }
  }
}
