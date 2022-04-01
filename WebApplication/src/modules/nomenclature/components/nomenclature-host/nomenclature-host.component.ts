import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-nomenclature-host',
  templateUrl: './nomenclature-host.component.html',
  styleUrls: ['./nomenclature-host.component.css']
})
export class NomenclatureHostComponent {
  links = [
    // { fragment: 1, title: 'Вид продължителност', route: 'durationType' },
    // { fragment: 1, title: 'Вид такса', route: 'taxType' },
    { fragment: 1, title: 'Валута', route: 'currencyType' },
    { fragment: 2, title: 'Прикачени документи', route: 'applicationFileType' },
    // { fragment: 4, title: 'Форма на обучение', route: 'educationFormType' },
    { fragment: 3, title: 'Шаблони', route: 'template' },
    // { fragment: 6, title: 'Ниво владеене на език', route: 'languageDegree' },
    // { fragment: 7, title: 'Тип подготовка', route: 'trainingType' },
    // { fragment: 9, title: 'Тип на диплома', route: 'diplomaType' },
    { fragment: 4, title: 'Езици', route: 'language' }
    // { fragment: 5, title: 'Учебна година', route: 'schoolYear' }
  ];

  constructor(
    public route: ActivatedRoute
  ) { }
}
