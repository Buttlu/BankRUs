using BankRUs.Application.Identity;
using BankRUs.Application.Services;
using BankRUs.Infrastructure.Authentication;
using BankRUs.Infrastructure.Identity;
using BankRUs.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BankRUs.Infrastructure.Services;

public class CustomerService(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context,
    IOptions<PaginationOptions> pageOptions
) : ICustomerService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly PaginationOptions _pageOptions = pageOptions.Value;

    public async Task<IReadOnlyList<CustomerDto>> GetAll(int page = 1, int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 1;
        else if (pageSize > _pageOptions.MaxPageSize) pageSize = _pageOptions.MaxPageSize;
        return await _context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new CustomerDto(
                CustomerId: Guid.Parse(a.Id),
                FirstName: a.FirstName,
                LastName: a.LastName,
                Email: a.Email!,
                BankAccounts: null
            ))
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetById(Guid Id)
    {
        var user = await _userManager.FindByIdAsync(Id.ToString());
        
        if (user is null) 
            return null;
        
        return new CustomerDto(
            CustomerId: Guid.Parse(user.Id),
                FirstName: user.FirstName,
                LastName: user.LastName,
                Email: user.Email!,
                BankAccounts: null
        );
    }
}
