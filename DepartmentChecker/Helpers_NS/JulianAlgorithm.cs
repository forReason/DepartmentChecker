using DepartmentChecker.Objects_NS;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Helpers_NS
{
    public class JulianAlgorithm
    {
        public static async Task<List<CatalogItem>> SetupCatalogState(string department, bool isCadre, List<CatalogItem> items)
        {
            HashSet<string> allTags = new HashSet<string>();
            List<CatalogItem> cadreItems = new List<CatalogItem>();
            List<CatalogItem> catalogItems = new List<CatalogItem>();
            List<CatalogItem> filteredItems = new List<CatalogItem>();
            int departmentLength = department.Length;
            int doubleDepartmentLength = department.Length * 2;
            department = WebUtility.HtmlDecode(department).Replace(' ', '\u00A0');

            // normalize all relevant columns of the dataset entry
            Dictionary<string,string> precomputations = new Dictionary<string,string>();
            string computation;
            foreach (CatalogItem item in items)
            {
                foreach(string tag in item.Tags)
                {
                    allTags.Add(tag);
                }
                if (item.IsCadre) cadreItems.Add(item); // cadre only?
                else
                {
                    catalogItems.Add(item);
                    if (item.ForAllMandatory) catalogItems.Add(item); // for all?
                    else
                    {
                        // check if department applicable
                        for (int i = 0; i < item.TargetAudienceMandatory.Count; i++)
                        {
                            if (item.TargetAudienceMandatory[i].Length >= departmentLength
                                && item.TargetAudienceMandatory[i].Length <= doubleDepartmentLength) // performance optimizing pre-check
                            {
                                if (!precomputations.TryGetValue(item.TargetAudienceMandatory[i], out computation)) // lazy decode
                                {
                                    computation = WebUtility.HtmlDecode(item.TargetAudienceMandatory[i]);
                                    precomputations[item.TargetAudienceMandatory[i]] = computation;
                                }
                                if (department == computation) // department comparison
                                {
                                    filteredItems.Add(item);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            // html decompile unique tags
            HashSet<string> tags = new HashSet<string>();
            foreach (string tag in allTags)
            {
                tags.Add(WebUtility.HtmlDecode(tag));
            }

            /// set state (not included)
            return filteredItems;
        }
    }
}
