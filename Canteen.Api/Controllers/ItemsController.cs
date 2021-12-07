﻿using System.Text.Json;
using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Controllers;

[AllowAnonymous] // TODO: Change to [Authorize] after testing
[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly CanteenContext _context;
    private readonly IMapper _mapper;

    public ItemsController(CanteenContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsAsync()
    {
        IEnumerable<Item> items = await _context.Items.Include(item => item.Category).ToListAsync();
        var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);
        
        if (itemDtos == null)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        return Ok(itemDtos);
    }

    [HttpGet("{categoryId:int}")]
    public async Task<ActionResult<List<ItemDto>>> GetItemsByCategoryIdAsync(int categoryId)
    {
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteItemAsync(int id)
    {
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync([FromBody] JsonElement json)
    {
        return Ok();
    }
}