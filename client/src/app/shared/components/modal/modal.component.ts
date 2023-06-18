import { ChangeDetectorRef, Component, EventEmitter, Output, TemplateRef, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
  @ViewChild('template', { static: false }) template?: TemplateRef<any>;
  @Output() onHide: EventEmitter<any> = new EventEmitter<any>();
  modalRef?: BsModalRef;
  subscriptions: Subscription[] = [];
  
  constructor(private modalService: BsModalService, private changeDetection: ChangeDetectorRef) { }

  ngOnInit(): void {
  }

  show(): void {
    this.modalRef = this.modalService.show(this.template);
    let _combine: Subscription | undefined;
    if (this.modalRef?.onHide && this.modalRef?.onHidden) {
      _combine = combineLatest([
        this.modalRef.onHide,
        this.modalRef.onHidden
      ]).subscribe(() => {this.changeDetection.markForCheck()});
    }

    if(this.modalRef?.onHide) {
      this.subscriptions.push(this.modalRef?.onHide.subscribe(() => {
          this.onHide.emit();
        })
      );
    }

    if  (this.modalRef?.onHidden) {
      this.subscriptions.push(
        this.modalRef.onHidden.subscribe((reason: string | any) => {
          this.unsubscribe();
        })
      );
    }

    if (_combine) {
      this.subscriptions.push(_combine);      
    }
  }

  unsubscribe(): void {
    this.subscriptions.forEach((subscription: Subscription) => {
      subscription.unsubscribe();
    });
    this.subscriptions = [];
  }
 
  hide(): void {
    // this.onHide.emit();
    this.modalRef?.hide();
  }
}