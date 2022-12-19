using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace Utility.Web.InputProcessors
{
    public class InputProcessor
    {
        private Dictionary<string, string> queryList = new Dictionary<string, string>();
        private Dictionary<string, string> appSettingList = new Dictionary<string, string>();

        private HttpRequest requestContext;

        public InputProcessor(HttpRequest newRequestContext)
        {
            requestContext = newRequestContext;
        }

        public string AddQueryInput(string inputName)
        {
            return AddQueryInput(inputName, null);
        }

        public string AddQueryInput(string inputName, string defaultVal)
        {
            if (requestContext != null)
            {
                string inputVal = requestContext.Query[inputName];
                if (inputVal == ""){ inputVal = null; }
                if (inputVal == null) { inputVal = defaultVal; }
                queryList.Add(inputName, inputVal);
                return inputVal;
            }
            else 
            {
                string inputVal = defaultVal;
                queryList.Add(inputName, inputVal);
                return inputVal;
            }
        }

        public string AddAppSettingInput(string inputName)
        {
            return AddAppSettingInput(inputName, null);
        }

        public string AddAppSettingInput(string inputName, string defaultVal)
        {
            string inputVal = System.Environment.GetEnvironmentVariable(inputName);
            if (inputVal == null) { inputVal = defaultVal; }
            appSettingList.Add(inputName, inputVal);
            return inputVal;
        }

        public void ValidateInputs()
        {
            bool isValid = true;

            var InvalidQueryList = new List<string>();
            var InvalidAppSettingList = new List<string>();

            foreach (var i in queryList)
            {
                if (i.Value == null) { 
                    InvalidQueryList.Add(i.Key); 
                    isValid = false;
                }
            }
            foreach (var i in appSettingList)
            {
                if (i.Value == null) { 
                    InvalidAppSettingList.Add(i.Key); 
                    isValid = false; 
                }
            }

            if (isValid == false)
            {
                StringBuilder exceptionText = new StringBuilder("MissingInputs:");

                if (InvalidQueryList.Count > 0) 
                {
                    exceptionText.Append("\n");
                    exceptionText.Append("Missing Queries: ");
                    exceptionText.Append(string.Join(", ", InvalidQueryList));
                }

                if (InvalidAppSettingList.Count > 0) 
                {
                    exceptionText.Append("\n");
                    exceptionText.Append("Missing App Settings: ");
                    exceptionText.Append(string.Join(", ", InvalidAppSettingList));
                }

                throw new Exception(exceptionText.ToString());
            }
        }
    }
}