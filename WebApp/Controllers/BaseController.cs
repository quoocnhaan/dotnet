using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppDBContext _context;
        public readonly UserService _userService;
        public const string CARTKEY = "cart";


        public BaseController(IDbContextFactory<AppDBContext> context, UserService userService)
        {
            _context = context.CreateDbContext();
            _userService = userService;
        }
        protected async Task<AppUser?> GetUserAsync()
        {
            return await _userService.GetUser();
        }

    }
}
