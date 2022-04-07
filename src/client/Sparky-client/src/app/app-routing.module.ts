import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'intro', // hier pas je de homepage aan,standaard intro
    pathMatch: 'full'
  },
  {
      path: 'intro',
      loadChildren: './modules/intro/intro.module#IntroModule'
  },
  {
      path: 'main',
      loadChildren: './modules/main/main.module#MainModule',
      canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
