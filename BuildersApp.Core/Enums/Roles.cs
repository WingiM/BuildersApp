using System.ComponentModel.DataAnnotations;

namespace BuildersApp.Core.Enums;

public enum Roles
{
    [Display(Name = "Застройщик")] Developer = 1,
    [Display(Name = "Заказчик")] Customer = 2,
    [Display(Name = "Проектировщик")] Designer = 3
}