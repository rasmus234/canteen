using System.Globalization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Controllers;

[AllowAnonymous] // TODO: Change to [Authorize] after testing
[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly CanteenContext _context;
    private readonly IMapper _mapper;

    public EmployeesController(CanteenContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesAsync()
    {
        var employeeDtos = await _context.Employees.ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider).ToListAsync();

        if (employeeDtos == null!) return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(employeeDtos);
    }

    [HttpGet("token")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeFromJwtAsync()
    {
        var employeeIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "id")!;

        if (!int.TryParse(employeeIdClaim.Value, out var employeeId)) return BadRequest();

        var employeeDto = await _context.Employees.Include(employee => employee.Items)
                                        .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                                        .FirstOrDefaultAsync(employee => employee.EmployeeId == employeeId);

        if (employeeDto == null) return NotFound();

        return Ok(employeeDto);
    }

    [HttpGet("cake/{id:int}")]
    public async Task<IActionResult> GetEmployeeCakeAsync(int id)
    {
        var currenWeek = (short) ISOWeek.GetWeekOfYear(DateTime.Now);
        var currentYear = (short) DateTime.Now.Year;

        var employeeCakeDto = await _context.EmployeeCakes.Where(empCake => empCake.EmployeeId == id && empCake.Year == currentYear && empCake.Number == currenWeek)
                                            .ProjectTo<EmployeeCakeDto>(_mapper.ConfigurationProvider)
                                            .ToListAsync();

        return Ok(employeeCakeDto);
    }

    [HttpPost, Route("favourites")]
    public async Task<IActionResult> AddFavouriteItemAsync(int itemId)
    {
        var item = await _context.Items.FindAsync(itemId);

        var employeeIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "id")!;

        if (!int.TryParse(employeeIdClaim.Value, out var employeeId)) return BadRequest();

        var employee = await _context.Employees.FindAsync(employeeId);

        if (employee == null) return NotFound();

        employee.Items.Add(item!);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete, Route("favourites")]
    public async Task<IActionResult> RemoveFavouriteItemAsync(int itemId)
    {
        var item = await _context.Items.FindAsync(itemId);

        var employeeIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "id")!;

        if (!int.TryParse(employeeIdClaim.Value, out var employeeId)) return BadRequest();

        var employee = await _context.Employees.Include(employee => employee.Items).FirstAsync(employee => employee.EmployeeId == employeeId);

        if (employee == null!) return NotFound();

        employee.Items.Remove(item!);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost, Route("cakes")]
    public async Task<IActionResult> AddEmployeeCakeAsync(EmployeeCakeDto employeeCakeDto)
    {
        var employeeCake = _mapper.Map<EmployeeCake>(employeeCakeDto);
        await _context.EmployeeCakes.AddAsync(employeeCake);

        await _context.SaveChangesAsync();

        return Ok();
    }
}