import { Component, Input } from '@angular/core';
import { EducationalQualificationAliases } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { ApplicationFileDto } from 'src/modules/application/models/application-file.dto';
import { DocumentDto } from 'src/modules/application/models/document.dto';
import { ApplicationFileType } from 'src/modules/nomenclature/models/application-file-type.model';
import { ApplicationFileTypeResource } from 'src/modules/nomenclature/services/application-file-type.resource';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
  selector: 'app-documents-form',
  templateUrl: './documents-form.component.html'
})
export class DocumentsFormComponent extends CommonFormComponent<DocumentDto> {
  educationalQualificationAlias: string;
  date = new Date();
  maxissuedDate = { year: this.date.getFullYear(), month: this.date.getMonth() + 1, day: this.date.getDate() };
  minissuedDate = { year: this.date.getFullYear(), month: this.date.getMonth(), day: this.date.getDate() };

  @Input() showElements: boolean = false;

  @Input() applicantFullName: string;

  @Input('educationalQualificationAlias') set currentName(alias: string) {
    if (alias != null) {
      this.educationalQualificationAlias = alias;

      if (alias == EducationalQualificationAliases.TRAINEE || alias == EducationalQualificationAliases.PROFESSIONALBACHELOR) {
        this.educationalQualificationAlias = EducationalQualificationAliases.BACHELOR
      }

      this.resource.getApplicationFileTypes(this.educationalQualificationAlias).subscribe((fileTypes: ApplicationFileType[]) => {
        this.pushFiles(fileTypes);
      });
    }
  }

  constructor(
    protected resource: ApplicationFileTypeResource,
    public sharedService: SharedService) {
    super()
  }

  pushFiles(fileTypes: ApplicationFileType[]) {
    this.model.files = [];

    fileTypes.forEach(fileType => {
      const newFile = new ApplicationFileDto();
      newFile.type = fileType;
      this.model.files.push(newFile);
    })
  }

  addFile(): void {
    if (!this.model.files) {
      this.model.files = [];
    }

    const newFile = new ApplicationFileDto();
    this.model.files.push(newFile);
  }

  removeFile(index: number): void {
    if (this.model.files.length <= 1) {
      return;
    }

    this.model.files.splice(index, 1);
  }
}
