import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MainComponent } from './components/main/main.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { RegisterComponent } from './components/register/register.component';
import { AdminGuard } from './core/guards/admiin.guard';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login',
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'profile',
    component: MainComponent,
    children: [
      {
        path: '',
        canActivate: [AuthGuard],
        loadChildren: () =>
          import('./modules/employee/modules/employee.module').then(
            (m) => m.EmployeeModule
          ),
      },
    ],
  },
  {
    path: 'admin',
    component: MainComponent,
    children: [
      {
        path: '',
        canActivate: [AdminGuard],
        loadChildren: () =>
          import('./modules/admin/modules/admin.module').then(
            (m) => m.AdminModule
          ),
      },
    ],
  },

  { path: '**', component: NotFoundComponent },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
