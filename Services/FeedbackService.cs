using BlazorDevTest.Data;
using BlazorDevTest.Interfaces;
using BlazorDevTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorDevTest.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly AuthDbContext _context;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(AuthDbContext context, ILogger<FeedbackService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<Feedback>>> GetAllFeedbackAsync()
        {
            try
            {
                var feedbacks = await _context.Feedbacks
                    .AsNoTracking()
                    .OrderByDescending(f => f.DateSubmitted)
                    .ToListAsync();

                return Result<List<Feedback>>.Ok(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedbacks");
                return Result<List<Feedback>>.Fail("Failed to retrieve feedbacks");
            }
        }

        public async Task<Result<Feedback>> GetByIdAsync(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                    return Result<Feedback>.Fail("Feedback not found");

                return Result<Feedback>.Ok(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback {Id}", id);
                return Result<Feedback>.Fail("Failed to retrieve feedback");
            }
        }

        public async Task<Result<Feedback>> AddFeedbackAsync(Feedback feedback)
        {
            try
            {
                feedback.DateSubmitted = DateTime.UtcNow;
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                return Result<Feedback>.Ok(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding feedback");
                return Result<Feedback>.Fail("Failed to add feedback");
            }
        }

        public async Task<Result<Feedback>> UpdateFeedbackAsync(Feedback feedback)
        {
            try
            {
                var existing = await _context.Feedbacks.FindAsync(feedback.Id);
                if (existing == null)
                    return Result<Feedback>.Fail("Feedback not found");

                existing.CustomerName = feedback.CustomerName;
                existing.Message = feedback.Message;

                await _context.SaveChangesAsync();
                return Result<Feedback>.Ok(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feedback {Id}", feedback.Id);
                return Result<Feedback>.Fail("Failed to update feedback");
            }
        }

        public async Task<Result> DeleteFeedbackAsync(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                    return Result.Fail("Feedback not found");

                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting feedback {Id}", id);
                return Result.Fail("Failed to delete feedback");
            }
        }
    }
}
