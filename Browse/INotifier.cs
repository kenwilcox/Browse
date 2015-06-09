namespace Browse
{
  public enum MessageType
  {
    Normal,
    Error
  }

  public interface INotifier
  {
    bool ShowMessage(string message, MessageType type);
  }
}