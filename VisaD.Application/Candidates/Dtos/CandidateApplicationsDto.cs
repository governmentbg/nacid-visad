using System.Collections.Generic;
using VisaD.Application.Applications.Dtos;

namespace VisaD.Application.Candidates.Dtos
{
    public class CandidateApplicationsDto
    {
        public CandidateCommitDto CandidateCommit { get; set; }
        public IEnumerable<ApplicationSearchResultItemDto> Applications { get; set; }
        public bool HasOtherCommits { get; set; }
    }
}
