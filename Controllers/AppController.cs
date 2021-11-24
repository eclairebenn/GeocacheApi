using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeocacheAPI.Data;

namespace GeocacheAPI.Controllers
{
    public class AppController : ControllerBase
    {
        private readonly IGeocacheRepository _repository;

        public AppController(IGeocacheRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Geocaches
        //[HttpGet]
        //public IEnumerable<Geocache> GetGeocaches()
        //{
        //    var results = _repository.GetAllGeocaches();

        //    return results;
        //}

        //// GET: api/Geocaches/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Geocache>> GetGeocache(long id)
        //{
        //    var geocache = await _repository.Geocaches.FindAsync(id);

        //    if (geocache == null)
        //    {
        //        return NotFound();
        //    }

        //    return geocache;
        //}

        //// PUT: api/Geocaches/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGeocache(long id, Geocache geocache)
        //{
        //    if (id != geocache.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(geocache).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GeocacheExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Geocaches
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Geocache>> PostGeocache(Geocache geocache)
        //{
        //    _context.Geocaches.Add(geocache);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetGeocache), new { id = geocache.ID }, geocache);
        //}

        //// DELETE: api/Geocaches/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGeocache(long id)
        //{
        //    var geocache = await _context.Geocaches.FindAsync(id);
        //    if (geocache == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Geocaches.Remove(geocache);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool GeocacheExists(long id)
        //{
        //    return _context.Geocaches.Any(e => e.ID == id);
        //}
    }
}
