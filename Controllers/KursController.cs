using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using KursKayitApp.Models;
using Microsoft.EntityFrameworkCore;
using KursKayitApp.Data;

namespace KursKayitApp.Controllers;

public class KursController : Controller
{
    private readonly DataContext _context;
    public KursController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var kurslar = await _context.Kurslar.ToListAsync();
        return View(kurslar);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Kurs model)
    {
        _context.Kurslar.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var kurs = await _context
        .Kurslar
        .Include(o => o.KursKayitlari)
        .ThenInclude(o => o.Ogrenci)
        .FirstOrDefaultAsync(m => m.KursId == id);

        if (kurs == null)
        {
            return NotFound();
        }

        return View(kurs);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, Kurs model)
    {
        if (id != model.KursId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var kurs = await _context.Kurslar.FirstOrDefaultAsync(m => m.KursId == id);
        if (kurs == null)
        {
            return NotFound();
        }
        return View(kurs);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int? id,Kurs model)
    {
        if(id==null)
        {
            return NotFound();
        }
        
        var kurs = await _context.Kurslar.FirstOrDefaultAsync(m => m.KursId==model.KursId);
        
        if(kurs==null)
        {
            return NotFound();
        }
        if(id!=model.KursId)
        {
            return NotFound();

        }
        _context.Kurslar.Remove(kurs);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}

