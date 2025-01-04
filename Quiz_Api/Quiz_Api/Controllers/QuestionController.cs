using Data_Layer;
using Business_Layer;
using Microsoft.AspNetCore.Mvc;
using static Data_Layer.QuestionsData;

namespace Quiz_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        [HttpPost("GetQuestion")]
        public ActionResult<QuestionsData.QuestionDTO> GetQuestion(QuestionsData.QuestionDTO questionDTO)
        {
            Question Question = Question.FindQuestionbyQuestionID(questionDTO.QuestionID);
            if (Question == null)
            {
                return NotFound("No Question Found!");
            }
            return Ok(new QuestionsData.QuestionDTO(Question.QuestionID, Question.QuestionText));
        }


        [HttpGet("GetAllQuestions")]
        public ActionResult<QuestionsData.QuestionDTO> GetAllQuizzes()
        {
            List<QuestionsData.QuestionDTO> quizzes = Question.GetAllQuestions();
            if (quizzes.Count == 0)
            {
                return NotFound("No Questions Found!");
            }
            return Ok(quizzes);
        }


        [HttpPost("AddNewQuestion")]
        public ActionResult<QuestionsData.QuestionDTO> AddNewQuiz(QuestionsData.QuestionDTO questionDTO)
        {
            if (questionDTO == null || string.IsNullOrEmpty(questionDTO.QuestionText))
            {
                return BadRequest("Invalid data or Question does not exist.");
            }

            Question newQuestion = new Question(new QuestionsData.QuestionDTO(questionDTO.QuestionID, questionDTO.QuestionText));
            newQuestion.Save();

            questionDTO.QuestionID = newQuestion.QuestionID;

            return CreatedAtAction(nameof(AddNewQuiz), new { id = questionDTO.QuestionID}, questionDTO);
        }

        [HttpDelete("DeleteQuestion")]
        public ActionResult Delete(QuestionDTO Question)
        {
            if (Question.QuestionID < 1)
            {
                return BadRequest($"Not accepted ID {Question.QuestionID}");
            }
            if (Business_Layer.Question.DeleteQuestion(Question))

                return Ok($"Question with ID {Question.QuestionID} has been deleted.");
            else
                return NotFound($"Question with ID {Question.QuestionID} not found. no rows deleted!");
        }


        [HttpPut("UpdateQuestion")]
        public ActionResult<QuestionDTO> Update(QuestionDTO QuestionDTO)
        {
            Business_Layer.Question question = Business_Layer.Question.FindQuestionbyQuestionID(QuestionDTO.QuestionID);
            question.QuestionText = QuestionDTO.QuestionText;
            if (question.QuestionID == null)
            {
                return NotFound($"Question {QuestionDTO.QuestionID} not found.");
            }

            if (!question.Save())
            {
                return BadRequest("Failed to update the Question.");
            }

            return Ok(QuestionDTO);
        }
    }
}
