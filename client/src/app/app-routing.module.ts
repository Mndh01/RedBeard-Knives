import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminGuard } from './guards/admin.guard';

const routes: Routes = [
    {path: '', component: HomeComponent , pathMatch: 'full'},
    {path: 'register', component: RegisterComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'server-error', component: ServerErrorComponent},
    {path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule)},
    {
      path: 'admin',
      runGuardsAndResolvers: 'always',
      canActivate: [AdminGuard],
      loadChildren: () => import('./admin/admin.module').then(mod => mod.AdminModule),
    },
    {
      path: '',
      runGuardsAndResolvers: "always",
      canActivate: [AuthGuard],
      children: [
        {path: 'profile', loadChildren: () => import('./user-profile/user-profile.module').then(mod => 
          mod.UserProfileModule)},
        {path: 'checkout', loadChildren: () => import('./checkout/checkout.module').then(mod =>
          mod.CheckoutModule)},
        {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod =>
          mod.BasketModule)},
        ]
    },
    {path: '**', redirectTo: 'not-found' , pathMatch: 'full'},
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
