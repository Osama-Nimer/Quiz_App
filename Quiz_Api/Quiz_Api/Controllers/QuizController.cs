using Data_Layer;
using Business_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        [HttpPost("GetQuiz")]
        public ActionResult<QuizzesData.QuizDTO> GetQuiz(QuizzesData.QuizDTO QuizDTO)
        {
            Quiz quiz = Quiz.FindQuizbyQuizID(QuizDTO);
            if (quiz == null)
            {
                return NotFound("No Quiz Found!");
            }
            return Ok(new QuizzesData.QuizDTO(quiz.QuizID, quiz.Subject));
        }


        [HttpGet("GetAllQuizzes")]
        public ActionResult<QuizzesData.QuizDTO> GetAllQuizzes()
        {
            List<QuizzesData.QuizDTO> quizzes = Quiz.GetAllQuizzes();
            if (quizzes.Count == 0)
            {
                return NotFound("No Quizzes Found!");
            }
            return Ok(quizzes);
        }


        [HttpPost("AddNewQuiz")]
        public ActionResult<QuizzesData.QuizDTO> AddNewQuiz(QuizzesData.QuizDTO QuizDTO)
        {
            if (QuizDTO == null || string.IsNullOrEmpty(QuizDTO.Subject))
            {
                return BadRequest("Invalid task data or user does not exist.");
            }

            Quiz newQuiz = new Quiz(new QuizzesData.QuizDTO(QuizDTO.QuizID, QuizDTO.Subject));
            newQuiz.Save();

            QuizDTO.QuizID = newQuiz.QuizID;

            return CreatedAtAction(nameof(AddNewQuiz), new { id = QuizDTO.QuizID}, QuizDTO);
        }
    }
}
