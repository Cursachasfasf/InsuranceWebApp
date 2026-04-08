using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;

namespace InsuranceWebApp.Models // или ваше пространство имён, например InsuranceWebApp.Models
{
    public class InsuranceDBContext : DbContext
    {
        public InsuranceDBContext(DbContextOptions<InsuranceDBContext> options)
            : base(options)
        {
        }

        // DbSet для всех сущностей
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<InsuranceProduct> InsuranceProducts { get; set; }
        public DbSet<InsuranceSituation> InsuranceSituations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Payout> Payouts { get; set; }

        // Если вы используете представление v_ActiveContracts как модель ActiveContractView
        // public DbSet<ActiveContractView> ActiveContractViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Явное указание имён таблиц (в единственном числе, как в вашем SQL-скрипте) ---
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Contract>().ToTable("Contract");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<InsuranceProduct>().ToTable("Insurance_Product");
            modelBuilder.Entity<InsuranceSituation>().ToTable("Insurance_Situation");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Payout>().ToTable("Payout");

            // --- Настройка первичных ключей (если EF неправильно определяет) ---
            modelBuilder.Entity<Client>().HasKey(c => c.client_id);
            modelBuilder.Entity<Contract>().HasKey(c => c.contract_id);
            modelBuilder.Entity<Employee>().HasKey(e => e.employee_id);
            modelBuilder.Entity<InsuranceProduct>().HasKey(p => p.product_id);
            modelBuilder.Entity<InsuranceSituation>().HasKey(s => s.situation_id);
            modelBuilder.Entity<Payment>().HasKey(p => p.payment_id);
            modelBuilder.Entity<Payout>().HasKey(p => p.payout_id);

            // --- Связи (Foreign Keys) ---

            // Contract -> Client
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Contracts)
                .HasForeignKey(c => c.client_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Contract -> InsuranceProduct
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.product_id)
                .OnDelete(DeleteBehavior.Restrict); // или Cascade, но осторожно

            // Contract -> Employee
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Employee)
                .WithMany()
                .HasForeignKey(c => c.employee_id)
                .OnDelete(DeleteBehavior.Restrict);

            // InsuranceSituation -> Contract
            modelBuilder.Entity<InsuranceSituation>()
                .HasOne(s => s.Contract)
                .WithMany(c => c.Situations)
                .HasForeignKey(s => s.contract_id)
                .OnDelete(DeleteBehavior.Cascade);

            // InsuranceSituation -> Employee
            modelBuilder.Entity<InsuranceSituation>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.employee_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment -> Contract
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Contract)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.contract_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Payout -> InsuranceSituation
            modelBuilder.Entity<Payout>()
                .HasOne(p => p.Situation)
                .WithMany(s => s.Payouts)
                .HasForeignKey(p => p.situation_id)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Индексы (опционально, из ваших SQL-скриптов) ---
            modelBuilder.Entity<Contract>().HasIndex(c => c.contract_id); // PK уже индекс
            modelBuilder.Entity<Client>().HasIndex(c => c.phone);
            // Если в Contract есть поле Number (как в ваших ранних обсуждениях) – расскомментируйте
            // modelBuilder.Entity<Contract>().HasIndex(c => c.Number).IsUnique();
        }
    }
}