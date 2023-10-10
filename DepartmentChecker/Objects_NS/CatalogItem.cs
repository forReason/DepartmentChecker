using System.Collections.Generic;

namespace DepartmentChecker.Objects_NS
{
    public class CatalogItem
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public List<string> TargetAudience { get; set; }
        public List<string> TargetAudienceMandatory { get; set; }
        public bool IsCadre { get; set; }
        public bool ForAllMandatory { get; set; }
        public string TagsAsString => string.Join(", ", Tags);
        public string TargetAudienceAsString => string.Join(", ", TargetAudience);
        public string TargetAudienceMandatoryAsString => string.Join(", ", TargetAudienceMandatory);
    }
}
