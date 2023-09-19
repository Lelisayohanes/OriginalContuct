using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{
    public class CRUDDBContext : DbContext
    {
        public CRUDDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Contuct> Contucts { get; set; }
    }
}
