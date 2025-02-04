using Microsoft.AspNetCore.Mvc;

namespace Ademicon.Comissao.Api;

[ApiController]
[Route("api/[controller]")]
public class ComissaoController : ControllerBase
{
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

    public ComissaoController()
    {
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Nenhum arquivo foi enviado.");
        }

        try
        {
            string filePath = Path.Combine(_uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { Message = "Arquivo enviado com sucesso!", FileName = file.FileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar o arquivo: {ex.Message}");
        }
    }
}
