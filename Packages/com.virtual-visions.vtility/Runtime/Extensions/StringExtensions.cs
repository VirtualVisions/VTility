using System.Text;

namespace VirtualVisions.VTility
{
    public static class StringExtensions
    {
        /// <summary>
        /// Capitalize the first letter of every word, while lowercase-ing the rest.
        /// </summary>
        public static string ToTitleCase(this string text)
        {
            string lower = text.ToLowerInvariant();
            
            StringBuilder builder = new StringBuilder(text.Length);

            bool foundNewWord = false;

            for (int i = 0; i < lower.Length; i++)
            {
                char character = lower[i];
                if (char.IsWhiteSpace(character))
                {
                    foundNewWord = true;
                    builder.Append(character);
                }
                else
                {
                    if (foundNewWord || i == 0)
                    {
                        builder.Append(char.ToUpperInvariant(character));
                        foundNewWord = false;
                    }
                    else
                    {
                        builder.Append(character);
                    }
                }
            }

            return builder.ToString();
        }
    }
}