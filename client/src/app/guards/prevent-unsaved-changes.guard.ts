import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { ProductEditComponent } from '../admin/components/products/product-edit/product-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(component: ProductEditComponent): boolean {
    if (component.editform.dirty) {
      return confirm("Are you sure you want to continue ? Unsaved changes will be lost");
    }
    return true;
  }
}
