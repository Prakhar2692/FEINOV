using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class UserService(Context dbContext, IDateTimeService dateTimeService) : IUserService
{
    public async Task<AuthenticatedUser> GetOrCreateUserAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
            throw new ArgumentException("Mobile number is required.", nameof(mobileNumber));

        var user = await dbContext.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.MobileNumber == mobileNumber && x.IsActive, cancellationToken);

        if (user != null)
        {
            return new AuthenticatedUser(
                user.UserId,
                user.MobileNumber,
                string.IsNullOrWhiteSpace(user.FirstName) && string.IsNullOrWhiteSpace(user.LastName)
                    ? null
                    : string.Join(' ', new[] { user.FirstName, user.LastName }.Where(x => !string.IsNullOrWhiteSpace(x))),
                user.Role.RoleName);
        }

        var role = await dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == "Customer", cancellationToken);
        if (role == null)
        {
            role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Customer",
                CreatedDate = dateTimeService.UtcNow.UtcDateTime
            };

            dbContext.Roles.Add(role);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var now = dateTimeService.UtcNow.UtcDateTime;
        var newUser = new User
        {
            UserId = Guid.NewGuid(),
            MobileNumber = mobileNumber,
            IsActive = true,
            IsEmailVerified = false,
            IsMobileVerified = true,
            RoleId = role.RoleId,
            CreatedDate = now
        };

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AuthenticatedUser(newUser.UserId, newUser.MobileNumber, null, role.RoleName);
    }
}
