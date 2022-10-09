using System;
namespace KeymashPPBasic.Classes
{
    public class Text
    {
        //Data is parsed from the same data as the API request
        public int TextId;
        public string Content;
        public string Source;
        public string Author;
        public string Contributor;
        public string Locale;
        public int Enabled;
        //Calculated properties
        public float StarRating;
        public float Obscurity;
        public float Density;
        public float LengthR;

        public Text(int textId, string content, string source, string author, string contributor, string locale, int enabled, float starRating = 0, float obscurity = 0, float density = 0, float lengthr = 0)
        {
            this.TextId = textId;
            this.Content = content;
            this.Source = source;
            this.Author = author;
            this.Contributor = contributor;
            this.Locale = locale;
            this.Enabled = enabled;
            //Calculated properties
            this.StarRating = starRating;
            this.Obscurity = obscurity;
            this.Density = density;
            this.LengthR = lengthr;
        }
    }
}
