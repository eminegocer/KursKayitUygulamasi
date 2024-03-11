using System.ComponentModel.DataAnnotations;

namespace KursKayitApp.Data
{
    public class Ogrenci
    {
        // primaryKey => benzersiz 
        [Key]
        public int OgrenciId { get; set; }
        public string? OgrenciAd { get; set; }
        public string? OgrenciSoyad { get; set; }
        public string? OgrenciAdSoyad
        {
            get
            {
                return this.OgrenciAd +" "+this.OgrenciSoyad;
            }
        }

        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        public ICollection<KursKayit> KursKayitlari { get; set; } =new List<KursKayit>();
    }
}