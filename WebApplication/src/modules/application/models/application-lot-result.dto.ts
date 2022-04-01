import { ApplicationLotResultType } from '../enums/application-lot-result-type.enum';

export class ApplicationLotResultDto {
  id: number;
  type: ApplicationLotResultType;
  attachedFilePath: string;
  note: string;
  certificateNumber: string;
  accessCode: string;
  isSigned: boolean;
}
