using EncryptionServiceApi.Models;
using EncryptionServiceApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace EncryptionServiceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EncryptionController : ControllerBase
    {
        private readonly IEncrypt _encrypt;

        public EncryptionController(IEncrypt encrypt)
        {
            _encrypt = encrypt;
        }

        [HttpGet("RsaKey")]
        public IActionResult GetRsaKey()
        {
            var rsaKey = _encrypt.GetRSA();
            
            return Ok(rsaKey);
        }

        [HttpPost("EncryptData")]
        public IActionResult PostEncrypt(EncryptRequest request)
        {
            var response = _encrypt.Decrypt(request.Key);
            return Ok(response);
        }


    }
}
