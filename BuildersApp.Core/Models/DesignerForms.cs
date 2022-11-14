namespace BuildersApp.Core.Models;

public class BaseDesignerForm
{
    public int ProjectId { get; set; }
    public bool IsSigned { get; set; }
}

public class WaterForm : BaseDesignerForm
{
    public string Diameter { get; set; }
    public string Performance { get; set; }
    public string KNS { get; set; }
    public string Cost { get; set; }
    public string Duration { get; set; }
}

public class GasForm : BaseDesignerForm
{
    public string Diameter { get; set; }
    public string Diameter2 { get; set; }
    public string Cost { get; set; }
    public string Duration { get; set; }
}