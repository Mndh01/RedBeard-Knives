import { Component, ViewChild } from '@angular/core';
import { NgbCalendar, NgbDate, NgbDateAdapter, NgbDateNativeAdapter, NgbDatepicker, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss'], 
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }]
})
export class DatepickerComponent {
  @ViewChild('d1') datePopup? : NgbDatepicker;
  model: NgbDateStruct;
  // date: { year: number, month: number };
  completed: boolean;

  constructor(private calendar: NgbCalendar) {
    this.model = { year: 1990, month: 1, day: 1 };

    // this.datePopup.minDate = { year: 1900, month: 1, day: 1 };
    // this.datePopup.maxDate = this.selectToday();
  }

  selectToday() {
    return this.calendar.getToday();
  }

  closeFix(event, datePicker) {
    if(event.target.offsetParent == null)
      datePicker.close();
    else if(event.target.offsetParent.nodeName != "NGB-DATEPICKER")
      datePicker.close();
  }
}