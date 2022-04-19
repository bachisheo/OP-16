using Microsoft.Toolkit.Mvvm.Input;

namespace obshepit_form_16.ViewModels;

public class SignViewModel
{
    public string FIO { get; set; } = "Денисов Иван Петрович";
    public string Post { get; set; } = "Кладовщик";
    public string FIOCheck { get; set; } = "Павлова Елена Викторовна";
    public string PostCheck { get; set; } = "Бухгалтер";
}