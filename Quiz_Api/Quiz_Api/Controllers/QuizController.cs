using Business_Layer;
using Data_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Data_Layer.QuestionsData;
using static Data_Layer.QuizzesData;

namespace Quiz_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        [HttpPost("GetQuiz")]
        public ActionResult<QuizzesData.QuizDTO> GetQuiz(QuizzesData.QuizDTO QuizDTO)
        {
            Quiz Quiz = Quiz.FindQuizbyQuizID(QuizDTO.QuizID);
            if (Quiz == null)
            {
                return NotFound("No Quiz Found!");
            }
            return Ok(new QuizzesData.QuizDTO(Quiz.QuizID, Quiz.UserID , Quiz.Score));
        }

        [HttpGet("GetAllQuiz")]
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
        public ActionResult<QuizzesData.QuizDTO> AddNewQuiz(QuizzesData.QuizDTO quizDTO)
        {
            if (quizDTO == null)
            {
                return BadRequest("Invalid data or Question does not exist.");
            }

            Quiz newQuiz = new Quiz(new QuizzesData.QuizDTO(quizDTO.QuizID, quizDTO.UserID , quizDTO.Score));
            newQuiz.Save();

            quizDTO.QuizID = newQuiz.QuizID;

            return CreatedAtAction(nameof(AddNewQuiz), new { id = quizDTO.QuizID }, quizDTO);
        }


        [HttpDelete("DeleteQuiz")]
        public ActionResult Delete(QuizDTO quiz)
        {
            if (quiz.QuizID < 1)
            {
                return BadRequest($"Not accepted ID {quiz.QuizID}");
            }
            if (Business_Layer.Quiz.DeleteQuiz(quiz))

                return Ok($"Question with ID {quiz.QuizID} has been deleted.");
            else
                return NotFound($"Question with ID {quiz.QuizID} not found. no rows deleted!");
        }
    }
}
