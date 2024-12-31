using Data_Layer;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Business_Layer.User user = Business_Layer.User.FindUserbyUserNameAndPassword("Osama Nimer","osama@454");
            Console.WriteLine(user.Role);
        }
    }
}
