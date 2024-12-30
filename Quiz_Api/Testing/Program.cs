using Data_Layer;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UsersData.UserDTO user = new UsersData.UserDTO(-1,"test","test.com","1234");
            

            Business_Layer.User newUser = new Business_Layer.User(user);
            newUser.Save();
        }
    }
}
