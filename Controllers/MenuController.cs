﻿using _26._02_sushi_market_back.Models;
using _26._02_sushi_market_back.Services;
using Microsoft.AspNetCore.Mvc;

namespace _26._02_sushi_market_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly BlobService _blobService;
        private readonly DatabaseMenuService _db;

        public MenuController(BlobService blobService, DatabaseMenuService db)
        {
            _blobService = blobService;
            _db = db;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDto dto)
        {
            var file = dto.File;

            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var url = await _blobService.UploadFileAsync(file);

            var menu = new MenuModel
            {

                BlobUrl = url,
                Count = dto.Count,
                Title = dto.Title,
               
            };

            _db.SaveProduct(menu);

            return Ok(menu);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var menu = _db.GetProducts();
            return Ok(menu);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _db.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] FileUploadDto dto)
        {
            var existing = _db.GetProductById(id);
            if (existing == null)
                return NotFound();

            string? url = existing.BlobUrl;

            if (dto.File != null && dto.File.Length > 0)
            {

                url = await _blobService.UploadFileAsync(dto.File);


                await _blobService.DeleteFileAsync(existing.BlobUrl);
            }

            existing.BlobUrl = url;
            existing.Count = dto.Count;
            existing.Title = dto.Title;


            _db.UpdateProduct(existing);

            return Ok(existing);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var menu = _db.GetProductById(id);
            if (menu == null)
                return NotFound();

            await _blobService.DeleteFileAsync(menu.BlobUrl);

            _db.DeleteProduct(id);
            return NoContent();
        }
    }
}
