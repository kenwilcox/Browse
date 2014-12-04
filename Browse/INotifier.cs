using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Browse
{
  public interface INotifier
  {
    void ShowMessage(string message, string caption = "");
  }
}
