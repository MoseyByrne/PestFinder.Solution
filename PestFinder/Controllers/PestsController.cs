using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using PestFinder.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PestFinder.Controllers
{
  [Authorize]
  public class PestsController : Controller
  {
    private readonly PestFinderContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public PestsController(UserManager<ApplicationUser> userManager, PestFinderContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    // [AllowAnonymous] <- allows anyone to view the corresponding method
    [AllowAnonymous]
    public async Task<ActionResult> Index()
    {
      ViewBag.Title = "Pests";
      ViewBag.Subtitle = "All Pests";
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (userId != null)
      {
        var currentUser = await _userManager.FindByIdAsync(userId);
        var userPests = _db.Pests.Where(entry => entry.User.Id == currentUser.Id).ToList();
        return View(userPests);
      }
      else
      {
        return View(_db.Pests.ToList());        
      }
    }
    
    public ActionResult Create()
    {
      ViewBag.Title = "Pests";
      ViewBag.Subtitle = "Log New Pest";
      ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Pest pest, int LocationId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      pest.User = currentUser;
      _db.Pests.Add(pest);
      _db.SaveChanges();
      if (LocationId != 0)
      {
        _db.PestLocation.Add(new PestLocation() { LocationId = LocationId, PestId = pest.PestId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisPest = _db.Pests
        .FirstOrDefault(pest => pest.PestId == id);
      ViewBag.Title = "Pests";
      ViewBag.Subtitle = "Details for " + thisPest.Type;  
      return View(thisPest);
    }

    public ActionResult Edit(int id)
    {
      var thisPest = _db.Pests.FirstOrDefault(pest => pest.PestId == id);
      ViewBag.Title = "Pests";
      ViewBag.Subtitle = "Edit " + thisPest.Type;
      ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
      return View(thisPest);
    }

    [HttpPost]
    public ActionResult Edit(Pest pest, int LocationId)
    {
      if (LocationId != 0)
      {
        _db.PestLocation.Add(new PestLocation() { LocationId = LocationId, PestId = pest.PestId });
      }
      _db.Entry(pest).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisPest = _db.Pests.FirstOrDefault(pest => pest.PestId == id);
      ViewBag.Title = "Pests";
      ViewBag.Subtitle = "Delete " + thisPest.Type;
      return View(thisPest);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisPest = _db.Pests.FirstOrDefault(pest => pest.PestId == id);
      _db.Pests.Remove(thisPest);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> AddLocation(int id)
    {
      var thisPest = _db.Pests.FirstOrDefault(pest => pest.PestId == id);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userLocations = _db.Locations.Where(entry => entry.User.Id == currentUser.Id).ToList();      
      ViewBag.Title = "Pests";
      ViewBag.Subtitle = "Add Location to " + thisPest.Type;
      ViewBag.LocationId = new SelectList(userLocations, "LocationId", "Name");
      return View(thisPest);
    }

    [HttpPost]
    public ActionResult AddLocation(Pest pest, int LocationId)
    {
      if (LocationId != 0)
      {
        _db.PestLocation.Add(new PestLocation() { LocationId = LocationId, PestId = pest.PestId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteLocation(int joinId)
    {
      var joinEntry = _db.PestLocation.FirstOrDefault(entry => entry.PestLocationId == joinId);
      _db.PestLocation.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}