namespace BlogApp.Models
{
  public class HeaderModel
  {
    public string HeaderText { get; set; }

  }

  public class FooterModel
  {
    public string FooterText { get; set; }
  }

  public class PrivacyViewModel
  {
    public HeaderModel Header { get; set; }
    public FooterModel Footer { get; set; }

  }
}
