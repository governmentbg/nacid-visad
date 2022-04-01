import { ApplicationFileDto } from './application-file.dto';

export class DocumentDto {
  files: ApplicationFileDto[] = [];
  areIdenticalFiles: boolean;
  description: string;

  // constructor() {
  //   this.files = [new ApplicationFileDto()];
  // }
}
