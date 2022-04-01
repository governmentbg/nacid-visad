import { Component, Input } from '@angular/core';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { LanguageProficiencyDto } from 'src/modules/application/models/language-proficiency.dto';
import { TrainingDto } from 'src/modules/application/models/training.dto';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
  selector: 'app-training-form',
  templateUrl: './training-form.component.html'
})
export class TrainingFormComponent extends CommonFormComponent<TrainingDto> {
  @Input('specialityLanguage') set specialityLanguage(languages: NomenclatureDto[]) {
    if (!this.model.languageProficiencies) {
      this.model.languageProficiencies = [];
    }

    if (languages?.length < 1 || languages == undefined || !languages[0] || languages[0].name == null) {
      return;
    }

    let isExisting = false;
    languages.forEach(language => {
      for (let languageProficiency of this.model.languageProficiencies) {
        if (language.name == languageProficiency.language.name || language.name == "Български") {
          isExisting = true;
        }
        else {
          languageProficiency.disable = false;
        }
      }

      if (!isExisting) {
        const newLanguageProficiency = new LanguageProficiencyDto();
        newLanguageProficiency.language = language;
        newLanguageProficiency.disable = true;
        newLanguageProficiency.required = true;
        this.model.languageProficiencies.push(newLanguageProficiency);
      }
    })
  }

  constructor(public sharedService: SharedService) {
    super()
  }

  addLanguageProficiency(): void {
    if (!this.model.languageProficiencies) {
      this.model.languageProficiencies = [];
    }

    const newLanguageProficiency = new LanguageProficiencyDto();
    this.model.languageProficiencies.push(newLanguageProficiency);
  }

  removeLanguageProficiency(index: number): void {
    if (this.model.languageProficiencies.length <= 1) {
      return;
    }

    this.model.languageProficiencies.splice(index, 1);
  }
}
