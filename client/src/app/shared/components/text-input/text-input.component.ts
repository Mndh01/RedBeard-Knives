import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit {
  @Input() label: string = 'Placeholder';
  @Input() type: "text" | string;
  @Input() content: "" | string;
  @Input() class: "" | string;

  constructor() { }

  ngOnInit(): void {
  }

}
