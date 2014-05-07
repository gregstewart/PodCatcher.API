using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Models
{
    class DateParsing
    {
        [Test]
        public void DateParser_WithEDTDate_ReturnsUTCDate()
        {
            String dateToParse = "Fri, 14 Feb 2014 00:00:00 EDT";
            
            DateTime parseDate = DateParser.ParseDate(dateToParse);

            Assert.AreEqual(parseDate, Convert.ToDateTime("2014-02-14 04:00:00.000"));
        }
    }
}

