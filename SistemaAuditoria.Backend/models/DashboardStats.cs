namespace SistemaAuditoria.Backend.Models
{
    public class DashboardStats
    {
        public int TotalFranchises { get; set; }
        public int MonthlyAudits { get; set; }
        public string UserName { get; set; } = string.Empty; // Inicializado para evitar null
    }
}