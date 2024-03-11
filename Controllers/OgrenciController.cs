using KursKayitApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KursKayitApp.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly DataContext _context;

        public OgrenciController(DataContext context)  // DataContext uzerindeki datasetler aracılığı ile veritabanına erişim sağlanır ve gerekli veri alışverişi bu şekilde sağlanır.
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ogrenciler=await _context.Ogrenciler.ToListAsync();
            return View(ogrenciler);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model)
        {
            _context.Ogrenciler.Add(model);
            await _context.SaveChangesAsync(); //await veritabanına sorgu gidecegi zaman kullanlır
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit( int? id) // id bilgisini listelenmiş kayıtlardaki asp-route-id den alır
        {
            if(id==null)
            {
                return NotFound();
            }
            // var ogr=await _context.Ogrenciler.FindAsync(id);
            var ogr=await _context
            .Ogrenciler
            .Include(o => o.KursKayitlari)
            .ThenInclude(o => o.Kurs) // birden fazla tablo değiştirilrse thenInclude kullanılır
            .FirstOrDefaultAsync(o => o.OgrenciId==id);
             if(ogr==null)
            {
                return NotFound();
            }
            return View(ogr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // güvenlik önlemi(get ve post eden kişi aynı mı?)
        public async Task<IActionResult> Edit(int id, Ogrenci model) // id URLden gelen, model ise cshtml sayfasında hidden özellikte olan öğrenci bilgisi
        {
            if(id != model.OgrenciId)
            {
                return NotFound();
            }

            if(ModelState.IsValid) // gerekli alanlar dolduruldu mu?
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) // aynı anda kaydı guncellerken kaydın olmamsı durumu
                {
                    if(!_context.Ogrenciler.Any(o => o.OgrenciId==model.OgrenciId))
                    {
                        return NotFound();
                    }         
                    else
                    {
                        throw;
                    }       
                }
                return RedirectToAction("Index");
            }
            return View(model); // gereklı alanlar doldurulmadıysa modelin mevcut bilgileri sayfada gösterilir
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var ogr=await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId==id);
            if(ogr == null)
            {
                return NotFound();
            }
            return View(ogr);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, Ogrenci model)
        {
            if(id==null)
            {
                return NotFound();
            }
            var ogr=await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId==model.OgrenciId);
            if(ogr ==null)
            {
                return NotFound();
            }

            if(id != model.OgrenciId)
            {
                return NotFound();
            }
            _context.Ogrenciler.Remove(ogr);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
    }
    
}