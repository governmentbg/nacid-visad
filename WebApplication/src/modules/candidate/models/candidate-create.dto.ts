import { CandidateDto } from './candidate.dto';

export class CandidateCreateDto {
  candidate: CandidateDto;

  constructor() {
    this.candidate = new CandidateDto();
  }
}
