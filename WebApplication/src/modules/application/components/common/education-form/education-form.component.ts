import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { EducationalQualificationAliases } from 'src/infrastructure/constants/constants';
import { EducationDto } from 'src/modules/application/models/education.dto';
import { EducationalQualificationNomenclatureDto } from 'src/modules/application/models/educational-qualification-nomenclature-dto';
import { SpecialityInformationDto } from 'src/modules/application/models/speciality-information.dto';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
  selector: 'app-education-form',
  templateUrl: './education-form.component.html'
})
export class EducationFormComponent extends CommonFormComponent<EducationDto> implements OnInit, OnChanges {
  currentYear = new Date().getFullYear();
  @Input() isTrainee: boolean = false;

  constructor() {
    super();
  }

  ngOnInit(): void {
    if (this.model.specialization != null) {
      this.isTrainee = true;
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.model != undefined) {
      this.isTrainee = changes.model?.currentValue.specialization != null ? true : false;
    }
    else {
      this.isTrainee = this.model.specialization != null ? true : false;
    }
  }

  @Input() universityId: number;

  setEducationalQualification(qualification: EducationalQualificationNomenclatureDto): void {
    this.model.educationalQualification = qualification;
    if (qualification.alias == EducationalQualificationAliases.TRAINEE) {
      this.isTrainee = true;
    }
    else {
      this.isTrainee = false;
    }

    this.model.educationSpecialityLanguages = null;
    this.model.disableSpecialityLanguage = false;

    this.model.faculty = null;
    this.model.speciality = null;
    this.model.duration = null;
    this.model.form = null;
    this.model.specialization = null;
    this.model.traineeDuration = null;
  }

  setFacultyId(facultyId: number): void {
    this.model.faculty.id = facultyId;

    this.model.educationSpecialityLanguages = null;
    this.model.disableSpecialityLanguage = false;

    this.model.speciality = null;
    this.model.duration = null;
    this.model.form = null;
    this.model.specialization = null;
    this.model.traineeDuration = null;
  }

  setSpecialityInformation(dto: SpecialityInformationDto): void {
    this.model.educationSpecialityLanguages = null;
    this.model.disableSpecialityLanguage = false;

    this.model.duration = dto.duration;
    this.model.form = dto.form;
    this.model.speciality = {};
    this.model.speciality.id = dto.id;
    this.model.speciality.name = dto.name;
    this.model.speciality.form = { ...dto.form };

    if (dto.specialityLanguage.length == 1 && dto.specialityLanguage[0] != undefined) {
      this.model.educationSpecialityLanguages = dto.specialityLanguage;
      this.model.disableSpecialityLanguage = true;
    }
    else if (dto.specialityLanguage.length > 1) {
      this.model.educationSpecialityLanguages = [];

      this.model.educationSpecialityLanguages.push(null);
    }
    else if (dto.specialityLanguage.length == 0) {
      this.model.educationSpecialityLanguages = [];

      let bulgarianLanguage = new NomenclatureDto();
      bulgarianLanguage.id = 11;
      bulgarianLanguage.name = "Български"
      this.model.disableSpecialityLanguage = true;

      this.model.educationSpecialityLanguages.push(bulgarianLanguage);
    }
  }

  emitLanguageToTraining(language: NomenclatureDto) {
    this.model.educationSpecialityLanguages = [];
    this.model.educationSpecialityLanguages.push(language);
  }
}
