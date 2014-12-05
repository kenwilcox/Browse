using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Browse
{
  public enum MessageType
  {
    Normal,
    Error,
  }

  public interface INotifier
  {
    bool ShowMessage(string message, MessageType type);
  }
}
