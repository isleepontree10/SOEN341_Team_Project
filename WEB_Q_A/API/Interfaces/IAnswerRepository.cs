using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IAnswerRepository
    {
        void Update(Answer Answer);
        void Create(Answer Answer);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Answer>> GetEAnswersAsync();
        Task<Answer> GetEAnswerByIdAsync(int id);
        Task<IEnumerable<Answer>> GetEAnswersByUsernameAsync(string username);
        Task<IEnumerable<AnswerDto>> GetAnswersAsync();
        Task<AnswerDto> GetAnswerByIdAsync(int id);
        Task<IEnumerable<AnswerDto>> GetAnswersByUsernameAsync(string username);
        Task<bool> IsQuestionExists(int questionId);
    }
}