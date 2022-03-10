import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { ProductEditComponent } from './components/product-edit/product-edit.component';
import { PreventUnsavedChangesGuard } from './guards/prevent-unsaved-changes.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'home', component: HomeComponent},
  {path: 'admin-panel', component: AdminPanelComponent},
  {path: 'products/:id', component: ProductDetailsComponent},
  {path: 'product/edit/:id', component: ProductEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},
  {path: '**', component: NotFoundComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
