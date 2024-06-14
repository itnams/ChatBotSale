using ChatBot.Data;
using ChatBot.Entities;
using ChatBot.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ChatBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatBotController : Controller
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;

        public ChatBotController(DataContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }
        [HttpPost("completions")]
        public async Task<ActionResult> CompletionsRequest([FromForm] CompletionRequest data)
        {
            var products = await _context.Product.Where(p => p.ProductTypeID == data.ProductTypeValue).ToListAsync();
            var jsonString = JsonSerializer.Serialize(products);
            var requestBody = new
            {
                model = "uonlp/Vistral-7B-Chat-gguf",
                messages = new[]
                {
                    new { role = "system", content = "Trả lời như một người bán hàng và chỉ giời tư vấn những sản phẩm trong list sau:" + jsonString},
                    new { role = "user", content = data.Message }
                },
                temperature = 0.7,
                max_tokens = -1,
                stream = false
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:1234/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return Ok(responseString);
            }
            return StatusCode((int)response.StatusCode, responseString);
        }
        [HttpGet("product-type")]
        public async Task<ActionResult> GetProductType()
        {
            var data = await _context.ProductType.ToListAsync();
            return Ok(data);
        }
    }
}
