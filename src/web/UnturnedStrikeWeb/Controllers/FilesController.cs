using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnturnedStrikeAPI.Constants;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository filesRepository;

        public FilesController(IFilesRepository filesRepository)
        {
            this.filesRepository = filesRepository;
        }

        [Authorize(Roles = RoleConstants.AdminRoleId)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm(Name = "file")] IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                return Ok(await filesRepository.AddFileAsync(new UnturnedStrikeAPI.File(file.Name, file.ContentType, ms.ToArray(), ms.Length)));
            }
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetAsync(int fileId)
        {
            var file = await filesRepository.GetFileAsync(fileId);
            return File(file.Data, file.MimeType, file.Name);
        }
    }
}
