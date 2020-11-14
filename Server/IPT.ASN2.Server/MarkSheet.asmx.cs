using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.XPath;

namespace IPT.ASN2.Server
{
    /// <summary>
    /// Summary description for MarkSheet
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MarkSheet : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string Calculate()
        {
            string data = HttpContext.Current.Request["SubjectMarks"];
            SubjectMark[] subjectMarks = JsonConvert.DeserializeObject<SubjectMark[]>(data);

            SubjectMark MinSubjectMarks = subjectMarks.First(x => x.ObtainMarks == subjectMarks.Min(y => y.ObtainMarks));
            SubjectMark MaxSubjectMarks = subjectMarks.First(x => x.ObtainMarks == subjectMarks.Max(y => y.ObtainMarks));

            decimal subjectNo = subjectMarks.Count();
            decimal totalMarks = 100 * subjectNo;
            decimal totalObtainMarks = subjectMarks.Sum(x => x.ObtainMarks);
            decimal percent = (totalObtainMarks / totalMarks) * 100;

            Result result = new Result()
            {
                minSubject = MinSubjectMarks,
                maxSubject = MaxSubjectMarks,
                Percentage = percent
            };

            return JsonConvert.SerializeObject(result);
        }

        public class SubjectMark
        {
            public string Name { get; set; }
            public int ObtainMarks { get; set; }
        }

        public class Result
        {
            public SubjectMark minSubject { get; set; }
            public SubjectMark maxSubject { get; set; }
            public decimal Percentage { get; set; }
        }
    }
}
