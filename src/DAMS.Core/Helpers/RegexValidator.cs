using System.Text.RegularExpressions;

namespace DAMS.Helpers
{
    public class RegexValidator
    {
        public bool Email(string emailRecipient)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

            if (Regex.IsMatch(emailRecipient, pattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}