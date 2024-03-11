using Microsoft.EntityFrameworkCore;

namespace KursKayitApp.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }
        public DbSet<Ogrenci> Ogrenciler => Set<Ogrenci>();
        public DbSet<Kurs> Kurslar { get; set; } = null!;
        public DbSet<KursKayit> KursKayitlari { get; set; } = null!;
    }
}