using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoviePro.Data;
using MoviePro.Models.Database;
using MoviePro.Models.Settings;

namespace MoviePro.Controllers;
public class CollectionsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AppSettings _appSettings;

    public CollectionsController(ApplicationDbContext context, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    // GET: Collections
    public async Task<IActionResult> Index()
    {
        var defaultCollectionName = _appSettings.MovieProSettings.DefaultCollection.Name;
        var collections = await _context.Collection.Where(c => c.Name != defaultCollectionName).ToListAsync();

        return Ok(collections);
    }

    // POST: Collections/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] Collection collection)
    {
        _context.Add(collection);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "MovieCollections", new { id = collection.Id });
    }

    // GET: Collections/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var collection = await _context.Collection.FindAsync(id);
        if (collection == null)
        {
            return NotFound();
        }
        return Ok(collection);
    }

    // POST: Collections/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Collection collection)
    {
        if (id != collection.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (collection.Name == _appSettings.MovieProSettings.DefaultCollection.Name)
                {
                    return RedirectToAction("Index", "Collections");
                }

                _context.Update(collection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionExists(collection.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return Ok(collection);
    }

    // GET: Collections/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var collection = await _context.Collection
            .FirstOrDefaultAsync(m => m.Id == id);
        if (collection == null)
        {
            return NotFound();
        }

        if (collection.Name == _appSettings.MovieProSettings.DefaultCollection.Name)
        {
            return RedirectToAction("Index", "Collections");
        }

        return Ok(collection);
    }

    // POST: Collections/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var collection = await _context.Collection.FindAsync(id);
        _context.Collection.Remove(collection);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "MovieCollections");
    }

    private bool CollectionExists(int id)
    {
        return _context.Collection.Any(e => e.Id == id);
    }
}
