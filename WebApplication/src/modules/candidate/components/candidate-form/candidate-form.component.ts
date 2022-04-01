import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { CandidateDto } from 'src/modules/candidate/models/candidate.dto';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

@Component({
  selector: 'app-candidate-form',
  templateUrl: './candidate-form.component.html'
})
export class CandidateFormComponent extends CommonFormComponent<CandidateDto> implements OnInit {
  latinNamesRegExp = RegExps.LATIN_NAMES_REGEX;
  emailRegExp = RegExps.EMAIL_REGEX;
  cyrillicRegExp = RegExps.CYRILLIC_NAMES_REGEX;
  latinAndCyrillicRegExp = RegExps.LATIN_AND_CYRILLIC_NAMES_REGEX;
  passportRegExp = RegExps.LATIN_AND_NUMBER_REGEX;

  maxBirthdate = { year: new Date().getFullYear() - 16, month: 12, day: 31 };
  minBirthdate = { year: new Date().getFullYear() - 100, month: 1, day: 1 };

  isApplicationRoute: boolean = false;

  nationalities: NomenclatureDto[] = [];

  @Input() isModalWindow: boolean = false;
  @Input() lockInput: boolean = true;

  constructor(
    private router: Router,
    private toastrService: ToastrService,
    public sharedService: SharedService
  ) {
    super();
  }

  ngOnInit(): void {
    if (this.router.url.includes('application')) {
      this.isApplicationRoute = true;
    }
  }

  togglePreviousApplication(hasPreviousApplication: boolean): void {
    this.model.hasPreviousApplication = hasPreviousApplication;
    if (!hasPreviousApplication) {
      this.model.previousApplicationRegisterNumber = null;
    }
  }

  addNationality(): void {
    if (!this.model.otherNationalities) {
      this.model.otherNationalities = [];
    }

    this.model.otherNationalities.push(null);
  }

  removeNationality(index: number): void {
    if (this.model.otherNationalities.length < 1) {
      return;
    }

    this.model.otherNationalities.splice(index, 1);
  }

  checkNationality(nationality: NomenclatureDto) {
    this.model.otherNationalities.forEach(element => {
      if (element?.id == nationality.id) {
        this.model.nationality = new NomenclatureDto();
        this.toastrService.error("Кандидатът не може да има две еднакви гражданства")
      }
    });
  }

  checkOtherNationality(otherNationality: NomenclatureDto) {
    if (this.model.nationality?.id == otherNationality.id) {
      const index = this.model.otherNationalities.indexOf(otherNationality, 0);
      if (index > -1) {
        this.model.otherNationalities[index] = new NomenclatureDto();
        this.toastrService.error("Кандидатът не може да има две еднакви гражданства")
      }
    }

    let count = 0;
    this.model.otherNationalities.forEach(nationality => {
      if (nationality.id == otherNationality.id) {
        count++;
      }
    })

    if (count > 1) {
      const index = this.model.otherNationalities.indexOf(otherNationality, 0);
      if (index > -1) {
        this.model.otherNationalities[index] = new NomenclatureDto();
        this.toastrService.error("Кандидатът не може да има две еднакви гражданства")
      }
    }
  }
}
