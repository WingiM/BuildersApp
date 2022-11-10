using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models;

[Serializable]
public class CustomerData : Data
{
    public IndustryTypes IndustryType { get; init; }
}