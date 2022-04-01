export class ApplicationReportItemDto {
	modificationCommitsCount: number;
	unsignedCommitsCount: number;
	pendingCommitsCount: number;
	certificateCommitsCount: number;
	rejectedCommitsCount: number;
	annulledCommitsCount: number;

	nationality: string;
	country: string;
	educationalQualification: string;
	institution: string;
	schoolYear: string;

	candidateLatinName: string;
	candidateCiryllicName: string;
	candidateNationality: string;
	candidateCountry: string;
	candidateBirthPlace: string;
	candidateBirthDate: Date;
	candidateCertficatesCount: number;
}
