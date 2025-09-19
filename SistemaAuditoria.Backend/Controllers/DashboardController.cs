using Microsoft.AspNetCore.Mvc;
using SistemaAuditoria.Backend.Models;
using SistemaAuditoria.Backend.Services;

namespace SistemaAuditoria.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboardService;

        public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStats>> GetDashboardStats()
        {
            _logger.LogInformation("Requisição recebida para GetDashboardStats.");

            var totalFranchises = await _dashboardService.GetTotalFranchisesAsync(); 
            var monthlyAudits = await _dashboardService.GetMonthlyAuditsAsync();     

            var stats = new DashboardStats
            {
                TotalFranchises = totalFranchises,
                MonthlyAudits = monthlyAudits,
                UserName = "Usuário Logado"
            };

            return Ok(stats);
        }

        [HttpGet("user-name")]
        public ActionResult<string> GetUserName()
        {
            _logger.LogInformation("Requisição recebida para GetUserName.");
            return Ok("Usuário Logado");
        }
    }
}