using KellerAg.Shared.Entities.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;

namespace KellerAg.Shared.Entities.Tests
{
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void WhenUsingLocalizedDateTime_ThenItShouldAdaptRespectingDaylightSavingChange()
        {
            System.DateTime beforeDaylightSavingSwitch = new System.DateTime(2018, 10, 27, 17, 10, 42);
            System.DateTime afterDaylightSavingSwitch = new System.DateTime(2018, 10, 28, 17, 10, 42);

            var dth = new DateTimeHelper("Europe/Berlin");
            System.DateTime winterIsComing = dth.LocalizeDateTime("2018-10-27T17:10:42");
            System.DateTime summerIsGone = dth.LocalizeDateTime("2018-10-28T17:10:42");

            winterIsComing.ShouldBe(beforeDaylightSavingSwitch + TimeSpan.FromHours(2));   //Before there was 2h diff to UTC
            summerIsGone.ShouldBe(afterDaylightSavingSwitch + TimeSpan.FromHours(1));   //After it was 1h diff to UTC

            var dth2 = new DateTimeHelper("Etc/GMT-1"); //= CET
            System.DateTime winterIsComing2 = dth2.LocalizeDateTime("2018-10-27T17:10:42");
            System.DateTime summerIsGone2 = dth2.LocalizeDateTime("2018-10-28T17:10:42");

            winterIsComing2.ShouldBe(beforeDaylightSavingSwitch + TimeSpan.FromHours(1));  //Before there was 1h diff to UTC
            summerIsGone2.ShouldBe(afterDaylightSavingSwitch + TimeSpan.FromHours(1));  //After it was 1h diff to UTC

            var dth3 = new DateTimeHelper("UTC");
            System.DateTime winterIsComing3 = dth3.LocalizeDateTime("2018-10-27T17:10:42");
            System.DateTime summerIsGone3 = dth3.LocalizeDateTime("2018-10-28T17:10:42");

            winterIsComing3.ShouldBe(beforeDaylightSavingSwitch + TimeSpan.FromHours(0));  //Should be same as UTC
            summerIsGone3.ShouldBe(afterDaylightSavingSwitch + TimeSpan.FromHours(0));  //Should be same as UTC

            ICollection<string> allNames = DateTimeHelper.GetIanaTimeZoneNames(); // or use directly TZConvert.KnownIanaTimeZoneNames;
            allNames.Count.ShouldBeGreaterThan(594);

            DateTimeHelper.GetTimeText(winterIsComing).ShouldBe("19:10:42");
            DateTimeHelper.GetDateText(winterIsComing).ShouldBe("27.10.2018");

            // Not very good tests for cloud unit test runners as they can be anywhere
            var localTimeZoneId = DateTimeHelper.GetLocalSystemIanaTimeZoneName(); // e.g. "Europe/Berlin"
            var tziDisplayName = DateTimeHelper.GetLocalSystemTimeZoneDisplayName(); // e.g. "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna"
        }

        [TestMethod]
        public void WhenDeGeneralizingIanaNames_ThenItShouldCreateMeaningfulResults()
        {
            DateTimeHelper.GeneralizeIanaName("Europe/Zurich").ShouldBe("W. Europe Standard Time");
            DateTimeHelper.DeGeneralizeIanaName("W. Europe Standard Time").ShouldBe("Europe/Berlin");
            DateTimeHelper.DeGeneralizeIanaName("W. Europe Standard Time").ShouldNotBe("Europe/Zurich");

            DateTimeHelper.GeneralizeIanaName("Etc/GMT-1").ShouldBe("W. Central Africa Standard Time");
            DateTimeHelper.DeGeneralizeIanaName("W. Central Africa Standard Time").ShouldBe("Africa/Lagos");
            DateTimeHelper.DeGeneralizeIanaName("W. Central Africa Standard Time").ShouldNotBe("Etc/GMT-1");
        }


        [TestMethod]
        public void WhenLocalDateTimeToUtcDateTime_ThenFindCorrectTimeSpan()
        {
            var dth = new DateTimeHelper("Europe/Zurich");
            System.DateTime deviceDateTime = new System.DateTime(2018, 11, 15, 14, 45, 00);
            System.DateTime dateTimeInUtc = dth.DeLocalizeDateTime(deviceDateTime);
            dateTimeInUtc.ShouldBe(deviceDateTime - TimeSpan.FromHours(1));


            deviceDateTime = new System.DateTime(2018, 10, 15, 14, 45, 00);
            dateTimeInUtc = dth.DeLocalizeDateTime(deviceDateTime);
            dateTimeInUtc.ShouldBe(deviceDateTime - TimeSpan.FromHours(2));
        }

        [TestMethod]
        public void WhenUtcDateTimeIsGiven_ConvertToSpecialCloudFormatIsPossible()
        {
            /*
                Needed format: YYYY-MM-ddThh:mm:ss (1970-01-01T00:00:00)

                UTC: 2019-10-28T12:00:00 => 
                Washington: 2019-10-28T07:00:00 (without UTC offset format 'z' or '-0400')
                (example: 'America/New_York')
             */

            var ianaTimeZoneName = "America/New_York";
            DateTimeHelper.IsValidIanaTimeZone(ianaTimeZoneName).ShouldBe(true);
            DateTimeHelper.IsValidIanaTimeZone("invalid" + ianaTimeZoneName).ShouldBe(false);

            var dth = new DateTimeHelper(ianaTimeZoneName);

            var utcDateTime = new System.DateTime(2019, 10, 28, 12, 00, 00, DateTimeKind.Utc);

            System.DateTime dateTimeLocalized = dth.LocalizeDateTime(utcDateTime);
            var dateTimeLocalizedText = dateTimeLocalized.ToString("yyyy-MM-ddTHH:mm:ss");

            dateTimeLocalizedText.ShouldBe("2019-10-28T08:00:00");
        }

        [TestMethod]
        public void WhenUtcTimeAroundDayLightSavingTimeIsGiven_ConvertToSpecialCloudFormatIsPossible()
        {
            var deLocalizedDateTimesTexts = new List<string>();

            var someMomentsAroundDayTimeSavingTime = new List<System.DateTime>(5)
            {
                new System.DateTime(2019, 10, 26, 20, 00, 00, DateTimeKind.Utc), //CET:22:00  +2
                new System.DateTime(2019, 10, 27, 00, 30, 00, DateTimeKind.Utc), //CET:02:30  +2
                new System.DateTime(2019, 10, 27, 01, 30, 00, DateTimeKind.Utc), //CET:02:30  +1
                new System.DateTime(2019, 10, 27, 02, 30, 00, DateTimeKind.Utc), //CET:03:30  +1
                new System.DateTime(2019, 10, 27, 04, 00, 00, DateTimeKind.Utc)  //CET:05:00  +1
            };

            var dth = new DateTimeHelper("Europe/Zurich");
            foreach (System.DateTime dt in someMomentsAroundDayTimeSavingTime)
            {
                System.DateTime dateTimeInUtc = dth.LocalizeDateTime(dt);

                deLocalizedDateTimesTexts.Add(dateTimeInUtc.ToString("yyyy-MM-ddTHH:mm:ss"));
            }

            deLocalizedDateTimesTexts.Count.ShouldBe(5);

            deLocalizedDateTimesTexts[0].ShouldBe("2019-10-26T22:00:00");
            deLocalizedDateTimesTexts[1].ShouldBe("2019-10-27T02:30:00");
            deLocalizedDateTimesTexts[2].ShouldBe("2019-10-27T02:30:00");
            deLocalizedDateTimesTexts[3].ShouldBe("2019-10-27T03:30:00");
            deLocalizedDateTimesTexts[4].ShouldBe("2019-10-27T05:00:00");
        }

        [TestMethod]
        public void WhenLocalizingDateTimeThatIsMinValue_ThenItShouldNotCrash()
        {
            var dateTimeHelper = new DateTimeHelper("America/New_York");
            var dateTime = dateTimeHelper.LocalizeDateTime(DateTime.MinValue);
        }
    }
}
