using Data_Layer;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Business_Layer.Choice c = Business_Layer.Choice.FindChoicebyChoiceID(1);
            Console.WriteLine(c.QuestionInfo.QuestionText);
        }
    }
}
