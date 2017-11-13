using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContactUs.Models
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false)]
    sealed public class ProperTime : ValidationAttribute
    {

        public TimeSpan EarliestTime { get; set; }
        public TimeSpan LatestTime { get; set; }
        public int Interval { get; set; }

        public ProperTime(int earliestValue, int latestValue, int intervalValue)
        {
            EarliestTime = new TimeSpan(earliestValue,0,0);
            LatestTime = new TimeSpan(latestValue,0,0);

            // need to make sure we weren't passed 0 as that could cause the end of the world when we try to mod by it later
            if(intervalValue == 0)
            {
                Interval = 60; // this is essentially same as 0, meaning the time intervals will just be every hour
            }
            else
            {
                Interval = intervalValue;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                string strValue = value.ToString();
                var chosenTime = DateTime.Parse(strValue).TimeOfDay;
                if((chosenTime < EarliestTime) || (chosenTime > LatestTime))
                {
                    return new ValidationResult($"Something went wrong, please choose a time between {EarliestTime} and {LatestTime}");
                }
                else if(chosenTime.Minutes % Interval != 0)
                {
                    return new ValidationResult($"Something went wrong, please choose a time at {Interval} minute intervals");
                }
                return ValidationResult.Success;
            
            }
            catch(Exception)
            {
                return new ValidationResult($"Something went wrong, please choose a time between {EarliestTime} and {LatestTime} at {Interval} intervals");
            }
        }

    }

}