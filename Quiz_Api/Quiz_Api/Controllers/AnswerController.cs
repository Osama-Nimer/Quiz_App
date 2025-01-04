using Business_Layer;
using Data_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Data_Layer.AnswersData;
using static Data_Layer.ChoicesData;
using static Data_Layer.QuestionsData;

namespace Quiz_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        [HttpPost("GetAnswer")]
        public ActionResult<AnswersData.AnswerDTO> GetAnswer(AnswersData.AnswerDTO AnswerDTO)
        {
            Answer answer = Answer.FindAnswerbyAnswerID(AnswerDTO.AnswerID);
            if (answer == null)
            {
                return NotFound("No Answer Found!");
            }
            return Ok(new AnswersData.AnswerDTO(answer.AnswerID, answer.QuizID, answer.QuestionID, answer.ChoiceID));
        }


        [HttpGet("GetAllAnswers")]
        public ActionResult<AnswersData.AnswerDTO> GetAllAnswers()
        {
            List<AnswersData.AnswerDTO> answers = Answer.GetAllAnswers();
            if (answers.Count == 0)
            {
                return NotFound("No Answers Found!");
            }
            return Ok(answers);
        }


        [HttpPost("AddNewAnswer")]
        public ActionResult<AnswersData.AnswerDTO> AddNewAnswer(AnswersData.AnswerDTO AnswerDTO)
        {
            if (AnswerDTO == null )
            {
                return BadRequest("Invalid data or Answer does not exist.");
            }

            Answer newAnswer = new Answer(new AnswersData.AnswerDTO(AnswerDTO.AnswerID, AnswerDTO.QuizID, AnswerDTO.QuestionID, AnswerDTO.ChoiceID));
            newAnswer.Save();

            AnswerDTO.ChoiceID = newAnswer.AnswerID;

            return CreatedAtAction(nameof(AddNewChoice), new { id = AnswerDTO.AnswerID }, AnswerDTO);
        }

        [HttpDelete("DeleteAnswer")]
        public ActionResult Delete(AnswerDTO Answer)
        {
            if (Answer.AnswerID < 1)
            {
                return BadRequest($"Not accepted ID {Answer.AnswerID}");
            }
            if (Business_Layer.Answer.DeleteAnswer(Answer))

                return Ok($"Answer with ID {Answer.AnswerID} has been deleted.");
            else
                return NotFound($"Answer with ID {Answer.AnswerID} not found. no rows deleted!");
        }


        
    }
}
