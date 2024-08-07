using System;
using System.Text.RegularExpressions;

namespace Validators.Models
{
    public static class BirthNumber
    {
        static readonly string BIRTH_NUMBER_PATTERN = @"^[0-9]{2}[01235678][0-9][0-3][0-9]/?([0-9]{4}|[0-9]{3} ?)$";

        public static BirthNumberValidationDto Validate(string input)
        {
            BirthNumberValidationDto falseResult = new() { Input = input, IsValid = false };

            if (!Regex.IsMatch(input, BIRTH_NUMBER_PATTERN))
            {
                falseResult.InvalidityReason = "Input doesn't match birth number pattern.";
                return falseResult;
            }

            string inputWithoutSlash = input.Trim().Replace("/", "");

            int inputLength = inputWithoutSlash.Length;
            string suffix = inputWithoutSlash[6..];
            decimal firstNine;
            decimal modulo;
            decimal last;
            int yearPart, monthPart;
            int year, month, day;
            DateOnly birthDate;
            string sex;

            // Check digit for birth numbers since 1954
            if (inputLength == 10)
            {
                firstNine = decimal.Parse(inputWithoutSlash[0..9]);
                last = decimal.Parse(inputWithoutSlash[9..10]);
                modulo = firstNine % 11;

                // Exception
                if (modulo == 10)
                {
                    if (last != 0)
                    {
                        falseResult.InvalidityReason = $"Validity check failed - the number formed by first nine digits gives remainder 10 when divided by 11, but the check digit is {last}, not 0.";
                        return falseResult;
                    }
                }
                else
                {
                    if (modulo != last)
                    {
                        falseResult.InvalidityReason = $"Validity check failed - the number formed by first nine digits gives remainder {modulo} when divided by 11, but the check digit is {last}.";
                        return falseResult;
                    }
                }
            }
            else
            {
                if (suffix == "000")
                {
                    falseResult.InvalidityReason = $"Validity check failed - suffix 000 is not valid.";
                    return falseResult;
                }
            }

            yearPart = int.Parse(inputWithoutSlash[0..2]);

            if (inputLength == 10)
            {
                if (yearPart < 54)
                    year = 2000 + yearPart;
                else
                    year = 1900 + yearPart;
            }
            else
            {
                if (yearPart < 54)
                    year = 1900 + yearPart;
                else
                    year = 1800 + yearPart;
            }

            monthPart = int.Parse(inputWithoutSlash[2..4]);

            if (monthPart > 50)
            {
                sex = "F";
                if (monthPart > 70)
                    month = monthPart - 70;
                else
                    month = monthPart - 50;
            }
            else
            {
                sex = "M";
                if (monthPart > 20)
                    month = monthPart - 20;
                else
                    month = monthPart;
            }

            day = int.Parse(inputWithoutSlash[4..6]);

            if (!DateOnly.TryParse($"{year:D4}-{month:D2}-{day:D2}", out birthDate))
            {
                falseResult.InvalidityReason = $"The input conforms to formal validity criteria, but the encoded birth date '{year:D4}-{month:D2}-{day:D2}' is not a valid date.";
                return falseResult;
            }

            return new BirthNumberValidationDto
            {
                Input = input,
                IsValid = true,
                CanonicalBirthNumber = $"{yearPart:D2}{monthPart:D2}{day:D2}/{suffix, -4}",
                BirthDate = birthDate,
                Sex = sex
            };
        }
    }
}