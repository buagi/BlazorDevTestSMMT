using BlazorDevTest.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorDevTest.Interfaces
{
    public interface IFeedbackService
    {
        Task<Result<List<Feedback>>> GetAllFeedbackAsync();
        Task<Result<Feedback>> GetByIdAsync(int id);
        Task<Result<Feedback>> AddFeedbackAsync(Feedback feedback);
        Task<Result<Feedback>> UpdateFeedbackAsync(Feedback feedback);
        Task<Result> DeleteFeedbackAsync(int id);
    }
}
