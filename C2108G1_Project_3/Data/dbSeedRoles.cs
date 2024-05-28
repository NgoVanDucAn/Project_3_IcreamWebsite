using Microsoft.AspNetCore.Identity;

namespace C2108G1_Project_3.Data
{
        public class dbSeedRole
        {
            private readonly RoleManager<IdentityRole> _manager;
            public dbSeedRole(RoleManager<IdentityRole> manager)
            {
                _manager = manager;
            }
            public async Task RoleData()
            {
                await _manager.CreateAsync(new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" });
                await _manager.CreateAsync(new IdentityRole { Name = "SUBUSERS", NormalizedName = "SUBUSERS" });
                await _manager.CreateAsync(new IdentityRole { Name = "MANAGER", NormalizedName = "MANAGER" });
            }
        }
    
}

