import { Injectable } from '@angular/core';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export const CalendarLocalization = {
  weekdays: ['пн', 'вт', 'ср', 'чт', 'пт', 'сб', 'нд'],
  months: ['яну', 'фев', 'мар', 'апр', 'май', 'юни', 'юли', 'авг', 'сеп', 'окт', 'ное', 'дек']
};

@Injectable()
export class CustomDatepickerI18n extends NgbDatepickerI18n {

  constructor() {
    super();
  }

  getWeekdayShortName(weekday: number): string {
    return CalendarLocalization.weekdays[weekday - 1];
  }

  getMonthShortName(month: number): string {
    return CalendarLocalization.months[month - 1];
  }

  getMonthFullName(month: number): string {
    return this.getMonthShortName(month);
  }

  getDayAriaLabel(date: NgbDateStruct): string {
    return `${date.day}-${date.month}-${date.year}`;
  }
}
