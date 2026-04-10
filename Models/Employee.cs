using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class Employee
    {
        [Key]
        public int employee_id { get; set; }

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
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        public string phone { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Должность обязательна")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Должность должна содержать только буквы, пробелы и дефисы")]
        public string position { get; set; } = null!;
    }
}