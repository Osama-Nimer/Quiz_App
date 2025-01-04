using Business_Layer;
using Data_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Data_Layer.ChoicesData;
using static Data_Layer.QuestionsData;

namespace Quiz_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoiceController : ControllerBase
    {

        [HttpPost("GetChoice")]
        public ActionResult<ChoicesData.ChoiceDTO> GetChoice(ChoicesData.ChoiceDTO ChoiceDTO)
        {
            Choice choice = Choice.FindChoicebyChoiceID(ChoiceDTO.ChoiceID);
            if (choice == null)
            {
                return NotFound("No Choice Found!");
            }
            return Ok(new ChoicesData.ChoiceDTO(choice.ChoiceID, choice.QuestionID, choice.ChoiceText, choice.IsCorrect));
        }


        [HttpGet("GetAllChoices")]
        public ActionResult<ChoicesData.ChoiceDTO> GetAllChoices()
        {
            List<ChoicesData.ChoiceDTO> choices = Choice.GetAllChoices();
            if (choices.Count == 0)
            {
                return NotFound("No Choices Found!");
            }
            return Ok(choices);
        }


        [HttpPost("AddNewChoice")]
        public ActionResult<ChoicesData.ChoiceDTO> AddNewChoice(ChoicesData.ChoiceDTO choiceDTO)
        {
            if (choiceDTO == null || string.IsNullOrEmpty(choiceDTO.ChoiceText))
            {
                return BadRequest("Invalid data or Choice does not exist.");
            }

            Choice newChoice = new Choice(new ChoicesData.ChoiceDTO(choiceDTO.ChoiceID, choiceDTO.QuestionID, choiceDTO.ChoiceText, choiceDTO.isCorrect));
            newChoice.Save();

            choiceDTO.ChoiceID = newChoice.ChoiceID;

            return CreatedAtAction(nameof(AddNewChoice), new { id = choiceDTO.QuestionID }, choiceDTO);
        }

        [HttpDelete("DeleteChoice")]
        public ActionResult Delete(ChoiceDTO Choice)
        {
            if (Choice.ChoiceID < 1)
            {
                return BadRequest($"Not accepted ID {Choice.ChoiceID}");
            }
            if (Business_Layer.Choice.DeleteChoice(Choice))

                return Ok($"Choice with ID {Choice.ChoiceID} has been deleted.");
            else
                return NotFound($"Choice with ID {Choice.ChoiceID} not found. no rows deleted!");
        }


        [HttpPut("UpdateChoice")]
        public ActionResult<QuestionDTO> Update(ChoiceDTO ChoiceDTO)
        {
            Business_Layer.Choice choice = Business_Layer.Choice.FindChoicebyChoiceID(ChoiceDTO.ChoiceID);
            choice.ChoiceText = ChoiceDTO.ChoiceText;
            if (choice.ChoiceID == null)
            {
                return NotFound($"Choice {ChoiceDTO.ChoiceID} not found.");
            }

            if (!choice.Save())
            {
                return BadRequest("Failed to update the Choice.");
            }

            return Ok(ChoiceDTO);
        }
    }
}
