import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { ProductEditComponent } from './components/product-edit/product-edit.component';
import { PreventUnsavedChangesGuard } from './guards/prevent-unsaved-changes.guard';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { ShopComponent } from './shop/shop.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminGuard } from './guards/admin.guard';

const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'home', component: HomeComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'login', component: LoginComponent},
    {path: 'shop', component: ShopComponent},
    {path: 'shop/:id', component: ProductDetailsComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'server-error', component: ServerErrorComponent},
    {
    path: '', 
    runGuardsAndResolvers: "always",
    canActivate: [AdminGuard],
    children: [
      {path: 'admin', component: AdminPanelComponent},
      {path: 'product/edit/:id', component: ProductEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},
    ]
    },
    {
      path: '',
      runGuardsAndResolvers: "always",
      canActivate: [AuthGuard],
      children: [
        {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod =>
          mod.BasketModule), data: {breadcrumb: 'Basket'}},
        ]
      },
    {path: '**', component: NotFoundComponent, pathMatch: 'full'},
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
