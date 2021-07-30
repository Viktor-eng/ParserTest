namespace ParserTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal class Parser
    {
        public void Parse()
        {
            var inputPath = Path.Combine(Environment.CurrentDirectory, "TextFile", "InputTextFile.txt");
            var outputPath = Path.Combine(Environment.CurrentDirectory, "TextFile", "OutputTextFile.txt");
            var dataList = new List<Data>();

            using (var sr = new StreamReader(inputPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    SplitData(line);
                    dataList.Add(SplitData(line));
                }
            }

            dataList = CombineSamePerson(dataList);

            dataList = dataList
                .Select(data => new Data(data.Name, data.RequestsPerHour, data.Hours, data.Info)
            {
                SumRequests = GetSumRequests(int.Parse(data.Hours),  int.Parse(data.RequestsPerHour)).ToString()
            })
                .ToList();
            
            using (var sw = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                foreach (var data in dataList)
                {
                    sw.WriteLine(string.Join(" ", data.Name, data.RequestsPerHour, data.Hours, data.SumRequests, data.Info));
                }
            }
        }

        private Data SplitData(string line)
        {
            var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new Data(
                words[0],
                words[1],
                words[2],
                ReplaceInfo(words[3]));
        }

        private string ReplaceInfo(string info)
        {
            var replaceInfo = info.Replace("\"", string.Empty);
            return replaceInfo;
        }

        private List<Data> CombineSamePerson(List<Data> dataList)
        {
            var sameNamesList = dataList.GroupBy(c => c.Name).Where(g => g.Count() > 1).Select(data => data.Key).ToList();

            foreach (var sameName in sameNamesList)
            {
                var sameNamePersons = dataList.FindAll(data => data.Name == sameName);

                var name = sameNamePersons.Select(person => person.Name).FirstOrDefault();
                var requestsSum = GetRequestsSum(sameNamePersons.Select(person => person.RequestsPerHour).ToList());
                var hoursSum = GetHoursSum(sameNamePersons.Select(person => person.Hours).ToList());
                var info = sameNamePersons.Select(person => person.Info).FirstOrDefault();

                var newPerson = new Data(name, requestsSum.ToString(), hoursSum.ToString(), info);

                dataList.RemoveAll(data => data.Name == sameName);
                dataList.Add(newPerson);
            }

            return dataList;
        }

        private int GetRequestsSum(List<string> requests)
        {
            var intRequests = requests.Select(int.Parse);
            return intRequests.Aggregate((x, y) => x + y);
        }

        private int GetHoursSum(List<string> hours)
        {
            var intHours = hours.Select(int.Parse);
            return intHours.Aggregate((x, y) => x + y);
        }

        private int GetSumRequests(int hours, int requests)
        {
            return hours * requests;
        }

    }
}



