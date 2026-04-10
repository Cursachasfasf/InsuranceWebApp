using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class Client
    {
        [Key]
        public int client_id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Имя должно содержать только буквы, пробелы и дефисы")]
        [StringLength(50, ErrorMessage = "Имя не может быть длиннее 50 символов")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Фамилия обязательна")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Фамилия должна содержать только буквы, пробелы и дефисы")]
        [StringLength(50, ErrorMessage = "Фамилия не может быть длиннее 50 символов")]
        public string Step_Name { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]*$", ErrorMessage = "Отчество должно содержать только буквы, пробелы и дефисы")]
        [StringLength(50, ErrorMessage = "Отчество не может быть длиннее 50 символов")]
        public string? S_Step_Name { get; set; }

        [Required(ErrorMessage = "Телефон обязателен")]
        [Phone(ErrorMessage = "Введите корректный номер телефона (например, +7(123)456-78-90)")]
        public string phone { get; set; } = null!;

        [Required(ErrorMessage = "Адрес обязателен")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\.,\-]+$", ErrorMessage = "Адрес может содержать буквы, цифры, пробелы и знаки . , -")]
        public string address { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Введите корректный email (например, name@domain.com)")]
        public string? email { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}