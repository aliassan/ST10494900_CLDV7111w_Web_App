using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Context;
using EventEase.Services;

namespace EventEase.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        private readonly IVenueAvailabilityService _venueAvailabilityService;

        public BookingController(
            ApplicationDbContext context, 
            ILogger<HomeController> logger,
            IVenueAvailabilityService venueAvailabilityService
        )
        {
            _logger = logger;
            _context = context;
            _venueAvailabilityService = venueAvailabilityService;
        }

        // GET: Bookings
        // public async Task<IActionResult> Index()
        // {
        //     var bookings = await _context.Bookings
        //         .Include(b => b.Event)
        //         .Include(b => b.Venue)
        //         .ToListAsync();
        //     return View(bookings);
        // }

        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)  // Include Event for name search
                .Include(b => b.Venue)  // Include Venue if you want to display it
                .AsQueryable();

            // In your controller
            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int bookingId))
                {
                    bookings = bookings.Where(b => b.BookingId == bookingId);
                }
                else
                {
                    // Recommended database-efficient version
                    bookings = bookings.Where(b => 
                        EF.Functions.Like(b.Event.EventName, $"%{searchString}%")
                    );
                }
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Creating a new booking");
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            ViewData["FormAction"] = "Create";
            ViewData["SubmitButtonText"] = "Create";
            return View("CreateEdit");
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            ViewData["FormAction"] = "Edit";
            ViewData["SubmitButtonText"] = "Save";
            return View("CreateEdit", booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,VenueId,BookingDate")] Booking booking)
        {
            _logger.LogInformation
            (
                "\nCreating booking with BookingId: {BookingId}, EventId: {EventId}, VenueId: {VenueId}, Date: {Date}\n", 
                booking.BookingId, booking.EventId, booking.VenueId, booking.BookingDate
            );

            var eventToBook = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            if (eventToBook == null)
            {
                return NotFound();
            }

            // Check venue availability for the event's time slot
            if (!await _venueAvailabilityService.IsVenueAvailableAsync(
                booking.VenueId, eventToBook.EventDate, eventToBook.EndDate))
            {
                ModelState.AddModelError("", "Venue is already booked for this event's time slot.");
            }
            if (ModelState.IsValid)
            {
                _logger.LogInformation
                (
                    "\nCreating booking with BookingId: {BookingId}, EventId: {EventId}, VenueId: {VenueId}, Date: {Date}\n", 
                    booking.BookingId, booking.EventId, booking.VenueId, booking.BookingDate
                );
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogWarning("\nModel state is invalid for booking creation\n");
                _logger.LogWarning("Model state is invalid. Errors:");
                foreach (var error in ModelState)
                {
                    if (error.Value.Errors.Count > 0)
                    {
                        _logger.LogWarning($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
        
            }
            
            // Re-populate the dropdown lists
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            ViewData["FormAction"] = "Create";
            ViewData["SubmitButtonText"] = "Create";
            return View("CreateEdit", booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            ViewData["FormAction"] = "Edit";
            ViewData["SubmitButtonText"] = "Save";
            return View("CreateEdit", booking);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            ViewData["ViewMode"] = "Details";
            return View("DetailDelete", booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            ViewData["ViewMode"] = "Delete";
            return View("DetailDelete", booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}