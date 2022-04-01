import { Component, Input } from '@angular/core';
import { DiplomaTypeAliases, EducationalQualificationAliases, RegExps } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { DiplomaFileDto } from 'src/modules/application/models/diploma-file.dto';
import { DiplomaTypeNomenclatureDto } from 'src/modules/application/models/diploma-type-nomenclature.dto';
import { DiplomaDto } from 'src/modules/application/models/diploma.dto';
import { DiplomaDocumentFile } from 'src/modules/application/models/diplomas/diploma-document-file';
import { EducationalQualificationNomenclatureDto } from 'src/modules/application/models/educational-qualification-nomenclature-dto';
import { NomenclatureResource } from 'src/modules/nomenclature/common/services/nomenclature.resource';
import { DiplomaType } from 'src/modules/nomenclature/models/diploma-type.model';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
  selector: 'app-diploma-form',
  templateUrl: './diploma-form.component.html'
})
export class DiplomaFormComponent extends CommonFormComponent<DiplomaDto> {
  showNacidFiles = false;

  date = new Date();
  minIssuedDate = { year: 1970, month: 1, day: 1 };
  maxIssuedDate = { year: this.date.getFullYear(), month: this.date.getMonth() + 1, day: this.date.getDate() };

  latinAndCyrillicNamesRegExp = RegExps.LATIN_AND_CYRILLIC_NAMES_REGEX;
  lettersAndNumbersRegExp = RegExps.LETTERS_AND_NUMBERS_REGEX;
  diplomaNumberRegExp = RegExps.DIPLOMA_NUMBER_REGEX;

  @Input('educationalQualification') set educationalQualification(educationalQualification: EducationalQualificationNomenclatureDto) {
    if (educationalQualification == undefined) {
      return;
    }

    let hasDiplom = false;
    let diplomaAlias = "";
    const diploma = new DiplomaFileDto();
    diploma.disabled = false;

    if (educationalQualification.alias == EducationalQualificationAliases.BACHELOR
      || educationalQualification.alias == EducationalQualificationAliases.MASTERWITHSECONDARY
      || educationalQualification.alias == EducationalQualificationAliases.PROFESSIONALBACHELOR) {

      hasDiplom = this.hasDiplom(DiplomaTypeAliases.SECONADARY);
      this.showNacidFiles = false;
      this.model.rectorDecisionDocumentFile = null;
      this.model.nacidRecommendation = null;

      if (!hasDiplom) {
        diplomaAlias = DiplomaTypeAliases.SECONADARY;
      }
    }
    else if (educationalQualification.alias == EducationalQualificationAliases.MASTERWITHBACHELOR) {
      hasDiplom = this.hasDiplom(DiplomaTypeAliases.BACHELOR);
      this.showNacidFiles = true;

      if (!hasDiplom) {
        diplomaAlias = DiplomaTypeAliases.BACHELOR;
      }
    }

    else if (educationalQualification.alias == EducationalQualificationAliases.DOCTORAL) {
      hasDiplom = this.hasDiplom(DiplomaTypeAliases.MASTER);
      this.showNacidFiles = true;

      if (!hasDiplom) {
        diplomaAlias = DiplomaTypeAliases.MASTER;
      }
    }

    else if (educationalQualification.alias == EducationalQualificationAliases.TRAINEE) {
      hasDiplom = true;
      this.showNacidFiles = false;
      this.model.diplomaFiles.push(diploma);

    }

    if (!hasDiplom) {
      this.nomenclatureResource.getDiplomaTypeByAlias(diplomaAlias).subscribe((type: DiplomaType) => {
        diploma.type = new DiplomaTypeNomenclatureDto();
        diploma.type.id = type.id;
        diploma.type.name = type.name;
        diploma.type.alias = type.alias;
        diploma.canEdit = false;
      });

      this.model.diplomaFiles.push(diploma);
    }
  }

  constructor(
    private nomenclatureResource: NomenclatureResource<DiplomaType>,
    public sharedService: SharedService
  ) {
    super()
  }

  addDiploma(): void {
    if (!this.model.diplomaFiles) {
      this.model.diplomaFiles = [];
    }

    const diploma = new DiplomaFileDto();
    diploma.disabled = false;
    this.model.diplomaFiles.push(diploma);
  }

  removeDiploma(index: number): void {
    if (this.model.diplomaFiles.length <= 1) {
      return;
    }

    this.model.diplomaFiles.splice(index, 1);
  }

  addAttachedFile(index: number): void {
    if (!this.model.diplomaFiles[index].attachedFiles) {
      this.model.diplomaFiles[index].attachedFiles = [];
    }

    const newAttachedFile = new DiplomaDocumentFile();
    this.model.diplomaFiles[index].attachedFiles.push(newAttachedFile)
  }

  removeAttachedFile(diplomaIndex: number, fileIndex: number): void {
    if (this.model.diplomaFiles[diplomaIndex].attachedFiles.length <= 0) {
      return;
    }

    this.model.diplomaFiles[diplomaIndex].attachedFiles.splice(fileIndex, 1);
  }

  private hasDiplom(alias: string): boolean {
    let hasDiplom = false;

    this.model.diplomaFiles.forEach(diploma => {
      if (diploma?.type?.alias == alias) {
        hasDiplom = true;
      }
    });

    return hasDiplom;
  }
}
