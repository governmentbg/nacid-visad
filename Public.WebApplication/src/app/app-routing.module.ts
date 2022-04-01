import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EAuthRedirectResponseComponent } from './e_authentication/eAuth-redirect-response.component';
import { HomeComponent } from './home/home/home.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'eAuth',
    component: EAuthRedirectResponseComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
