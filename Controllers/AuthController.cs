using CinemaApi.Data;
using CinemaApi.DTOs;
using CinemaApi.Models;
using CinemaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context)
    {
        _context = context;
        _tokenService = new TokenService();
    }

    [HttpPost("register")]
    public IActionResult Resgister(RegisterDTO dto)
    {
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
        var usuario = new Usuario
        {
            Email = dto.Email,
            Senha = senhaHash
        };

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return Ok(usuario);
    }
    [HttpPost("login")]
    public IActionResult Login(LoginDTO dto)
    {
        var user = _context.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, user.Senha))
        return Unauthorized();
        var token = _tokenService.GenerateToken(user.Email);
        return Ok(new { token });
    }
    
}
