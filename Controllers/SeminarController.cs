using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using System.Globalization;
using System.Security.Claims;

namespace SeminarHub.Controllers
{
    public class SeminarController : BaseController
    {
        private readonly SeminarHubDbContext _data;

        public SeminarController(SeminarHubDbContext context)
        {
            _data = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var seminar = await _data.Seminars
                .Select(s => new SeminarAllViewModel
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime.ToString(DataConstants.DateFormat),
                    Category = s.Category.Name,
                    Organizer = s.Organizer.UserName

                })
                .AsNoTracking()
                .ToListAsync();

            return View(seminar);
        }

        [HttpGet]

        public async Task<IActionResult> Add()
        {
            var seminar = new SeminarFormViewModel();
            seminar.Categories = await GetCategories();

            return View(seminar);
        }

        [HttpPost]

        public async Task<IActionResult> Add(SeminarFormViewModel model)
        {
            DateTime DateAndTimeCheck = DateTime.Now;

            if (!DateTime.TryParseExact(
               model.DateAndTime,
               DataConstants.DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out DateAndTimeCheck))
            {
                ModelState
                    .AddModelError(nameof(model.DateAndTime), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();

                return View(model);
            }

            var seminar = new Seminar()
            {
                DateAndTime = DateAndTimeCheck,
                CategoryId = model.CategoryId,
                Details = model.Details,
                Lecturer = model.Lecturer,
                OrganizerId = GetUserId(),
                Duration = model.Duration,
                Topic = model.Topic,
            };

            await _data.Seminars.AddAsync(seminar);
            await _data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]

        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var seminar = await _data.SeminarsParticipants
                .Where(sp => sp.ParticipantId == userId)
                .AsNoTracking()
                .Select(sp => new SeminarJoinedViewModel
                {
                    DateAndTime = sp.Seminar.DateAndTime.ToString(DataConstants.DateFormat),
                    Id = sp.SeminarId,
                    Lecturer = sp.Seminar.Lecturer,
                    Organizer = userId,
                    Topic = sp.Seminar.Topic

                }
                ).ToListAsync();

            return View(seminar);
        }

        [HttpPost]

        public async Task<IActionResult> Join(int id)
        {
            var seminar = await _data.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();

            if (!seminar.SeminarsParticipants.Any(e => e.ParticipantId == userId))
            {
                seminar.SeminarsParticipants.Add(new SeminarParticipant
                {
                    SeminarId = id,
                    ParticipantId = userId
                });

                await _data.SaveChangesAsync();
                return RedirectToAction(nameof(Joined));
            }

            return RedirectToAction(nameof(All));
        }
        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var seminar = await _data.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var sp = seminar.SeminarsParticipants
                .FirstOrDefault(ep => ep.ParticipantId == userId);

            if (sp == null)
            {
                return BadRequest();
            }

            seminar.SeminarsParticipants.Remove(sp);

            await _data.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _data.Seminars
                .FindAsync(id);

            if (model == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();

            if (model.OrganizerId != userId)
            {
                return Unauthorized();
            }

            var seminar = new SeminarFormViewModel()
            {
                DateAndTime = model.DateAndTime.ToString(DataConstants.DateFormat),
                CategoryId = model.CategoryId,
                Details = model.Details,
                Lecturer = model.Lecturer,
                Duration = model.Duration,
                Topic = model.Topic
            };

            seminar.Categories = await GetCategories();

            return View(seminar);

        }

        [HttpPost]

        public async Task<IActionResult> Edit(SeminarFormViewModel model, int id)
        {
            var seminar = await _data.Seminars
                .FindAsync(id);

            if (seminar == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();

            if (seminar.OrganizerId != userId)
            {
                return Unauthorized();
            }


            DateTime DateAndTimeCheck = DateTime.Now;

            if (!DateTime.TryParseExact(
               model.DateAndTime,
               DataConstants.DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out DateAndTimeCheck))
            {
                ModelState
                    .AddModelError(nameof(model.DateAndTime), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();

                return View(model);
            }

            seminar.DateAndTime = DateAndTimeCheck;
            seminar.CategoryId = model.CategoryId;
            seminar.Details = model.Details;
            seminar.Lecturer = model.Lecturer;
            seminar.Duration = model.Duration;
            seminar.Topic = model.Topic;

            await _data.SaveChangesAsync();

            return RedirectToAction(nameof(All));

        }

        [HttpGet]

        public async Task<IActionResult> Details(int id)
        {
            var seminar = await _data.Seminars
                .Where(s => s.Id == id)
                .AsNoTracking().Select(s => new SeminarDetailsViewModel
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime.ToString(DataConstants.DateFormat),
                    Category = s.Category.Name,
                    Organizer = s.Organizer.UserName,
                    Duration = s.Duration,
                    Details = s.Details
                })
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            return View(seminar);
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var seminar = await _data.Seminars
                .FindAsync(id);

            if (seminar == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();

            if (seminar.OrganizerId != userId)
            {
                return Unauthorized();
            }
            var model = new SeminarDeleteViewModel
            {
                DateAndTime = seminar.DateAndTime.ToString(DataConstants.DateFormat),
                Id = seminar.Id,
                Topic = seminar.Topic,
            };

            return View(model);

        }

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seminar = await _data.Seminars
                .FindAsync(id);

            if (seminar == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();

            if (seminar.OrganizerId != userId)
            {
                return Unauthorized();
            }

            var sp = await _data.SeminarsParticipants
                .FirstOrDefaultAsync(p => p.SeminarId == id);

            if (sp != null)
            {
                _data.SeminarsParticipants.Remove(sp);
            }

            _data.Seminars.Remove(seminar);
            await _data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }
        private async Task<IEnumerable<CategoriesViewModel>> GetCategories()
        {
            var categories = await _data.Categories
                .Select(c => new CategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .AsNoTracking()
                .ToListAsync();

            return categories;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }


    }
}
