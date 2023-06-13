using System.Text;

namespace HRProject.UI.Helper
{
    public class PasswordGenerator
    {
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string SpecialCharacters = "!@#$%^&*()-_=+";
        private const string Numbers = "0123456789";

        private static Random random = new Random();

        public static string GeneratePassword()
        {
            StringBuilder passwordBuilder = new StringBuilder();



            passwordBuilder.Append(GetRandomCharacter(LowercaseLetters));



            passwordBuilder.Append(GetRandomCharacter(UppercaseLetters));



            passwordBuilder.Append(GetRandomCharacter(SpecialCharacters));



            passwordBuilder.Append(GetRandomCharacter(Numbers));



            for (int i = 0; i < 4; i++)
            {
                string allCharacters = LowercaseLetters + UppercaseLetters + SpecialCharacters + Numbers;
                passwordBuilder.Append(GetRandomCharacter(allCharacters));
            }



            string password = passwordBuilder.ToString();



            password = new string(password.ToCharArray().OrderBy(x => random.Next()).ToArray());



            return password;
        }

        private static char GetRandomCharacter(string characterString)
        {
            int randomIndex = random.Next(characterString.Length);
            return characterString[randomIndex];
        }
    }
}
