using DepartmentChecker.Objects_NS;
using System;
using System.Collections.Generic;
using System.Net;

namespace DepartmentChecker.Helpers_NS
{
    public class DatasetGenerator
    {
        private readonly List<string> Tags = new List<string> {
            "Academy",
            "QA",
            "Self-Study",
            "Lean Trainings",
            "Automation",
            "Logistics",
            "Data Systems"
        };

        public List<CatalogItem> Dataset { get; set; }

        public readonly List<string> Departments;
        public readonly List<string> UnnormalDepartments;

        public DatasetGenerator(int departmentCount = 5000)
        {
            string[] baseWords = { "Sales", "Tech", "Legal", "Finance", "Human", "Operations", "Quality", "Customer", "Supply", "Product", "Internal", "Public", "Risk", "Business", "Data", "Creative", "Digital", "Talent", "Resource", "Security" };
            HashSet<string> uniqueDepartments = new HashSet<string>();
            HashSet<string> unnormalUniqueDepartments = new HashSet<string>();

            Random random = new Random();

            while (uniqueDepartments.Count < departmentCount)
            {
                List<string> departmentWords = new List<string>();
                int numberOfWords = random.Next(1, 5); // Generate between 1 and 4 words

                for (int i = 0; i < numberOfWords; i++)
                {
                    string word = baseWords[random.Next(baseWords.Length)];
                    departmentWords.Add(WebUtility.HtmlEncode(word));
                }

                string department = string.Join(" ", departmentWords);
                string unnormalDepartment = string.Join("&nbsp;", departmentWords);
                unnormalUniqueDepartments.Add(unnormalDepartment);
                uniqueDepartments.Add(department);
            }

            UnnormalDepartments = new List<string>(unnormalUniqueDepartments);
            Departments = new List<string>(uniqueDepartments);
        }


        public (List<CatalogItem>, List<string>, List<string>) GenerateDataset(int numberOfItems)
        {
            var random = new Random();
            var dataset = new List<CatalogItem>();

            for (int i = 0; i < numberOfItems; i++)
            {
                var item = new CatalogItem
                {
                    Name = "Training" + i,
                    Tags = GenerateRandomSubset(Tags, random),
                    TargetAudience = GenerateRandomSubset(UnnormalDepartments, random),
                    TargetAudienceMandatory = GenerateRandomSubset(UnnormalDepartments, random)
                };

                dataset.Add(item);
            }
            Dataset = dataset;
            return (dataset, Tags, UnnormalDepartments);
        }

        private List<string> GenerateRandomSubset(List<string> source, Random random)
        {
            int count = random.Next(1, source.Count + 1);
            HashSet<string> subsetCheck = new HashSet<string>();
            List<string> subset = new List<string>(count);

            while (subset.Count < count)
            {
                var index = random.Next(0, source.Count);
                if (subsetCheck.Add(source[index]))
                {
                    subset.Add(source[index]);
                }
            }

            return subset;
        }

    }
}