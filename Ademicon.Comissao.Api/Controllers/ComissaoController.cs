using Ademicon.Comissao.Service.ComissaoService;
using Microsoft.AspNetCore.Mvc;

namespace Ademicon.Comissao.Api;

[ApiController]
[Route("api/[controller]")]
public class ComissaoController : ControllerBase
{

    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");
    public ComissaoService ComissaoService { get; set; }

    public ComissaoController(ComissaoService comissaoService)
    {
        ComissaoService = comissaoService;

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
            return BadRequest("Nenhum arquivo enviado!");
        }

        var filePath = Path.Combine(_uploadPath, file.FileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        var extension = Path.GetExtension(filePath);
        var idFileName = new Random().NextInt64(1000000, 9999999);
        var newFileName = $"{fileNameWithoutExtension}_{idFileName}{extension}";

        filePath = Path.Combine(_uploadPath, newFileName);

        if (extension != ".html")
        {
            return BadRequest("Arquivo com extensão diferente de .html!");
        }

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var result = await ComissaoService.Processar(filePath);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }


            return Ok(result);

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao Processar o Arquivo: {ex.Message}");
        }
    }
}
